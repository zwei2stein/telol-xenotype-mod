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
    
    public class HediffTrigger
    {
        public HediffDef hediff;
        public bool ignoreCooldown = false;
        public float? chance; // null = global settings chance
    }

    public class GeneDef_CognitiveInjuryResponse : GeneDef
    {
        public TraitCandidateList traitCandidates = new TraitCandidateList();
        public List<HediffTrigger> triggerHediffs = new List<HediffTrigger>();

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            
            if (customEffectDescriptions == null)
                customEffectDescriptions = new List<string>();

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

            customEffectDescriptions.Add(
                "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Description_TraitsConsidered".Translate(
                    traitLabels.ToLineList(prefix: " • ").Named("TRAITS")));
            
            var hediffLabels = new List<string>();
            foreach (var trigger in triggerHediffs)
            {
                if (trigger.hediff == null)
                    continue;

                hediffLabels.Add(trigger.hediff.LabelCap.Resolve());
            }

            if (hediffLabels.Count > 0)
            {
                customEffectDescriptions.Add(
                    "TelolXenotype_Gene_TelolXenotype_CognitiveInjuryResponse_Description_TriggerHediffs".Translate(
                        hediffLabels.ToCommaList().Named("HEDIFFS")));
            }
            
        }
        
    }
}