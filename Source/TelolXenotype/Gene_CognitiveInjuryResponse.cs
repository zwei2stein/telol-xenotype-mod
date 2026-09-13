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
        
    }
}