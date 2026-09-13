using System.Collections.Generic;
using System.Linq;
using System.Xml;
using RimWorld;
using Verse;

namespace TelolRace
{
    public class TraitCandidate
    {
        public TraitDef trait;
        public int degree;
        public string traitDefName; // kept for diagnostics, in case cross-ref genuinely fails to resolve
    }

    public class TraitCandidateList : List<TraitCandidate>
    {
        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            foreach (XmlNode node in xmlRoot.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                var mayRequire = node.Attributes?["MayRequire"]?.Value;
                if (!mayRequire.NullOrEmpty() &&
                    !mayRequire.Split(',').All(id => ModsConfig.IsActive(id.Trim())))
                {
                    continue; // required mod(s) not active, skip silently, same as vanilla MayRequire
                }

                var candidate = new TraitCandidate
                {
                    traitDefName = node.Name
                };
                int.TryParse(node.InnerText, out candidate.degree); // 0 for empty tags like <Nimble/>

                DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(candidate, "trait", node.Name);

                Add(candidate);
            }
        }
    }

    public class GeneDef_CognitiveInjuryResponse : GeneDef
    {
        public TraitCandidateList traitCandidates = new TraitCandidateList();

        public override void ResolveReferences()
        {
            base.ResolveReferences();

            var traitLabels = new List<string>();
            foreach (var candidate in traitCandidates)
            {
                var degreeData = candidate.trait?.DataAtDegree(candidate.degree);
                if (degreeData == null || degreeData.label.NullOrEmpty())
                {
                    Log.Warning("[TelolXenotype] - GeneDef_CognitiveInjuryResponse traitCandidates: trait '" + candidate.traitDefName + "' failed to resolve or has invalid degree " + candidate.degree);
                    continue;
                }

                traitLabels.Add(degreeData.label.CapitalizeFirst());
            }

            if (customEffectDescriptions == null)
                customEffectDescriptions = new List<string>();

            customEffectDescriptions.Add(
                "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Description_TraitsConsidered".Translate(
                    traitLabels.ToLineList(prefix: " • ").Named("TRAITS")));
        }
    }
}