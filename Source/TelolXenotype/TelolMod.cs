using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TelolRace
{
    [StaticConstructorOnStartup]
    public class TelolModStatic
    {
        static TelolModStatic()
        {
            //Run harmony patches
            var harmony = new HarmonyLib.Harmony("TelolXenotype");
            harmony.PatchAll();

            Log.Message("[TelolXenotype] loaded!");
        }
    }

    public class TelolMod : Mod
    {
        public TelolMod(ModContentPack content) : base(content)
        {
        }
        
        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listingStandard = new Listing_Standard();
            
            var gapWidth = 12f;
            
            listingStandard.Begin(inRect);
            
            listingStandard.Label("TelolXenotype_Settings_Genes_Label".Translate());
            
            listingStandard.GapLine();
            
            listingStandard.Indent(gapWidth);
            listingStandard.ColumnWidth -= gapWidth;
            
            listingStandard.Label("TelolXenotype_Settings_Genes_CognitiveInjuryResponse_Label".Translate());
            
            listingStandard.GapLine();
            
            listingStandard.Indent(gapWidth);
            listingStandard.ColumnWidth -= gapWidth;
            
            if (listingStandard.ButtonTextLabeledPct((string)"TelolXenotype_Settings_Genes_CognitiveInjuryResponse_Cooldown".Translate(),
                    TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Cooldown.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft))
            {
                var options = new List<FloatMenuOption>();
                foreach (ProgressionSpeed progressionSpeed in Enum.GetValues(typeof(ProgressionSpeed)))
                {
                    var localProgressionSpeed = progressionSpeed;
                    options.Add(new FloatMenuOption(localProgressionSpeed.ToStringHuman(),
                        (Action)(() => TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Cooldown = localProgressionSpeed)));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }
            
            TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Chance = listingStandard.SliderLabeled(
                "TelolXenotype_Settings_Genes_CognitiveInjuryResponse_Chance".Translate(
                    TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Chance.ToStringPercent().Named("CHANCE")),
                TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_Chance, 0f, 1f, 0.5f,
                "TelolXenotype_Settings_Genes_CognitiveInjuryResponse_Chance_Tooltip".Translate());
            
            TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_MaxTraits = Mathf.RoundToInt(listingStandard.SliderLabeled(
                "TelolXenotype_Settings_Genes_CognitiveInjuryResponse_MaxTraits".Translate(
                    TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_MaxTraits.Named("MAX")),
                TelolXenotypeModSettings.Gene_CognitiveInjuryResponse_MaxTraits, 1f, 6f, 0.5f,
                "TelolXenotype_Settings_Genes_CognitiveInjuryResponse_MaxTraits_Tooltip".Translate()));
            
            listingStandard.Outdent(gapWidth);
            listingStandard.ColumnWidth += gapWidth;
            
            listingStandard.Outdent(gapWidth);
            listingStandard.ColumnWidth += gapWidth;

            listingStandard.GapLine();
            
            listingStandard.End();
            
            base.DoSettingsWindowContents(inRect);
        }
        
        public override string SettingsCategory()
        {
            return "TelolXenotypeModName".Translate();
        }
    }

}