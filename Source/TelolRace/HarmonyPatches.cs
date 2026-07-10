using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace TelolRace
{
    public readonly struct PawnInjuryData
    {
        public Pawn Pawn { get; }

        public Hediff_Injury Injury { get; }

        public PawnInjuryData(Pawn pawn, Hediff_Injury injury)
        {
            Pawn = pawn;
            Injury = injury;
        }
    }

    [HarmonyPatch(typeof(DamageWorker_AddInjury), "FinalizeAndAddInjury", new Type[]
    {
        typeof(Pawn),
        typeof(Hediff_Injury),
        typeof(DamageInfo),
        typeof(DamageWorker.DamageResult)
    })]
    public class Patch
    {
        public static void Prefix(ref Pawn pawn, ref Hediff_Injury injury, out PawnInjuryData __state)
        {
            __state = new PawnInjuryData(pawn, injury);
        }

        public static void Postfix(PawnInjuryData __state)
        {
            if (!Rand.Chance(TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Chance))
                return;

            var pawn = __state.Pawn;

            if (pawn.TryGetComp<Gene_CognitiveInjuryResponse>(out var response))
            {
                if (response == null)
                    return;
                
                //Log.Message("[TelolXenotype] response.lastTriggerTick=" + response.lastTriggerTick);
                //Log.Message("[TelolXenotype] Find.TickManager.TicksGame=" + Find.TickManager.TicksGame );
                //cooldown between injury responses.
                if (response.lastTriggerTick > -1 && response.lastTriggerTick > Find.TickManager.TicksGame - TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Cooldown.ToTicks())
                {
                    //Log.Message("[TelolXenotype] Skipping, too early");
                    return;
                }
            }
            else
            {
                // not human?
                return;
            }

            if (response.traitsGranted >= TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_MaxTraits)
                // already got max traits.
                return;
            
            pawn.health.hediffSet.TryGetHediff(HediffDefOf.Anesthetic, out var hediffAnesthetic);
            if (hediffAnesthetic != null)
            {
                return;
                // planned operation is not the right kind of trauma
            }

            Hediff_Injury injury = __state.Injury;

            if (!pawn.health.Dead && pawn.genes.HasActiveGene(TelolXenotypeDefOf.TelolXenotype_CognitiveInjuryResponse))
            {
                
                var permanentInjury = pawn.health.hediffSet.GetPartHealth(injury.Part) <= 0f;
                var hediffCompPermanent = HediffUtility.TryGetComp<HediffComp_GetsPermanent>(injury);
                if (hediffCompPermanent != null)
                {
                    permanentInjury = permanentInjury || hediffCompPermanent.IsPermanent;
                }

                permanentInjury = permanentInjury || HediffUtility.IsPermanent(injury);

                if (permanentInjury)
                {
                    var candidateTraits = CognitiveInjuryResponseCache.GetCandidateTraits(pawn);

                    if (candidateTraits.Count > 0)
                    {
                        var selectedTrait = candidateTraits.RandomElement();

                        pawn.story.traits.GainTrait(selectedTrait);
                        
                        response.lastTriggerTick = Find.TickManager.TicksGame;
                        response.traitsGranted++;

                        var letter = LetterMaker.MakeLetter(
                            "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Letter_Title".Translate(
                                pawn.Named("PAWN")),
                            "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Letter_Text".Translate(
                                pawn.Named("PAWN"), selectedTrait.Label.Named("TRAIT"), injury.Part.Label.Named("INJURY")),
                            LetterDefOf.PositiveEvent,
                            new LookTargets((Thing)(object)pawn));

                        Find.LetterStack.ReceiveLetter(letter);
                    }
                    
                }
            }
        }
    }
}