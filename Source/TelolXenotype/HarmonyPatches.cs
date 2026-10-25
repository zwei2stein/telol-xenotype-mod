using HarmonyLib;
using RimWorld;
using System;
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
            var pawn = __state.Pawn;

            var response = pawn.genes?.GetFirstGeneOfType<Gene_CognitiveInjuryResponse>();
            if (response == null)
                return;

            pawn.health.hediffSet.TryGetHediff(HediffDefOf.Anesthetic, out var hediffAnesthetic);
            if (hediffAnesthetic != null)
                return;

            Hediff_Injury injury = __state.Injury;

            if (!pawn.health.Dead)
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
                    if (response.TryGrantTrait(false, null, out var selectedTrait))
                        response.SendLetter(selectedTrait, injury.Part.Label);
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.AddHediff), new Type[]
    {
        typeof(Hediff),
        typeof(BodyPartRecord),
        typeof(DamageInfo?),
        typeof(DamageWorker.DamageResult)
    })]
    public class HediffAddedPatch
    {
        public static void Postfix(Hediff hediff)
        {
            var pawn = hediff.pawn;

            var response = pawn?.genes?.GetFirstGeneOfType<Gene_CognitiveInjuryResponse>();
            if (response == null)
                return;

            var trigger = response.FindTrigger(hediff.def);
            if (trigger == null)
                return;

            if (pawn.health.Dead)
                return;

            // not first of its def on this pawn, or add was rejected
            if (pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def) != hediff)
                return;

            if (response.TryGrantTrait(trigger.ignoreCooldown, trigger.chance, out var selectedTrait))
            {
                var injuryLabel = hediff.Part != null ? hediff.Part.Label : (string)"WholeBody".Translate();
                response.SendLetter(selectedTrait, injuryLabel);
            }
        }
    }
}