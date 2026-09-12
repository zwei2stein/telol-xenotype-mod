using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TelolRace
{
    [StaticConstructorOnStartup]
    public static class CognitiveInjuryResponseCache
    {
        private static readonly Dictionary<TraitDef, List<int>> ValidTraitDegrees = new Dictionary<TraitDef, List<int>>();

        private static readonly List<TraitDef> CachedTraitDefs = new List<TraitDef>();

        static CognitiveInjuryResponseCache()
        {
            Log.Message("[Telol Xenotype] Initializing 'Cognitive Injury Gene'.");
            RebuildCache();
        }

        private static void RebuildCache()
        {
            ValidTraitDegrees.Clear();
            CachedTraitDefs.Clear();

            foreach (var traitDef in DefDatabase<TraitDef>.AllDefs)
            {
                var extension = traitDef.GetModExtension<CognitiveInjuryResponseExtension>();
                if (extension == null)
                    continue;

                CachedTraitDefs.Add(traitDef);

                var validDegrees = new List<int>();
                if (traitDef.degreeDatas.Count > 1)
                {
                    foreach (var degreeData in traitDef.degreeDatas)
                    {
                        if (degreeData.degree <= extension.degreeMax && degreeData.degree >= extension.degreeMin)
                        {
                            validDegrees.Add(degreeData.degree);
                        }
                    }
                }
                else
                {
                    validDegrees.Add(0); // Single-degree traits use degree 0
                }

                if (validDegrees.Count > 0)
                {
                    ValidTraitDegrees[traitDef] = validDegrees;
                }
            }
        }
        
        public static List<Trait> GetCandidateTraits(Pawn pawn)
        {
            var candidateTraits = new List<Trait>();

            foreach (var traitDef in CachedTraitDefs)
            {
                // Skip if pawn already has this trait
                if (pawn.story.traits.HasTrait(traitDef))
                    continue;

                if (ValidTraitDegrees.TryGetValue(traitDef, out var degrees))
                {
                    if (degrees.Count == 1)
                    {
                        candidateTraits.Add(new Trait(traitDef, degrees[0]));
                    }
                    else
                    {
                        // Randomly select one of the valid degrees
                        var randomDegree = degrees.RandomElement();
                        candidateTraits.Add(new Trait(traitDef, randomDegree));
                    }
                }
            }

            return candidateTraits;
        }
    }
}