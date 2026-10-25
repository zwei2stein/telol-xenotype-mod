using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TelolRace
{
    public class Gene_CognitiveInjuryResponse : Gene
    {
        public int lastTriggerTick = -1;
        public int traitsGranted = 0;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.lastTriggerTick, "lastTriggerTick");
            Scribe_Values.Look<int>(ref this.traitsGranted, "traitsGranted");
        }
        
        public List<Trait> GetCandidateTraits()
        {
            var geneDef = (GeneDef_CognitiveInjuryResponse)def;
            var candidates = new List<Trait>();

            foreach (var candidate in geneDef.traitCandidates)
            {
                if (pawn.story.traits.HasTrait(candidate.trait))
                    continue;

                candidates.Add(new Trait(candidate.trait, candidate.degree));
            }

            return candidates;
        }
        
        public HediffTrigger FindTrigger(HediffDef hediffDef)
        {
            var geneDef = (GeneDef_CognitiveInjuryResponse)def;
            foreach (var trigger in geneDef.triggerHediffs)
            {
                if (trigger.hediff == hediffDef)
                    return trigger;
            }
            return null;
        }

        public bool TryGrantTrait(bool ignoreCooldown, float? chanceOverride, out Trait grantedTrait)
        {
            grantedTrait = null;

            if (!Rand.Chance(chanceOverride ?? TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Chance))
                return false;

            if (!ignoreCooldown && lastTriggerTick > -1 &&
                lastTriggerTick > Find.TickManager.TicksGame - TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Cooldown.ToTicks())
                return false;

            if (traitsGranted >= TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_MaxTraits)
                return false;

            var candidateTraits = GetCandidateTraits();
            if (candidateTraits.Count == 0)
                return false;

            grantedTrait = candidateTraits.RandomElement();
            pawn.story.traits.GainTrait(grantedTrait);
            lastTriggerTick = Find.TickManager.TicksGame;
            traitsGranted++;
            return true;
        }

        public void SendLetter(Trait trait, string injuryLabel)
        {
            if (!pawn.IsColonist && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony)
                return;
            
            var letter = LetterMaker.MakeLetter(
                "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Letter_Title".Translate(
                    pawn.Named("PAWN")),
                "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Letter_Text".Translate(
                    pawn.Named("PAWN"), trait.Label.Named("TRAIT"), injuryLabel.Named("INJURY")),
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn));

            Find.LetterStack.ReceiveLetter(letter);
        }
        
    }
}