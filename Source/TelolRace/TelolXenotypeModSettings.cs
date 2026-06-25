using Verse;

namespace TelolRace
{
    public class TelolXenotypeModSettings : ModSettings
    {
        public static ProgressionSpeed Gene_CognitiveInjuryResponse_Cooldown = ProgressionSpeed.QUADRUM;
        public static int Gene_CognitiveInjuryResponse_MaxTraits = 3;
        public static float Gene_CognitiveInjuryResponse_Chance = 1f;

        public override void ExposeData()
        {
            Scribe_Values.Look<ProgressionSpeed>(ref Gene_CognitiveInjuryResponse_Cooldown, "Gene_CognitiveInjuryResponse_Cooldown", ProgressionSpeed.QUADRUM);
            Scribe_Values.Look<int>(ref Gene_CognitiveInjuryResponse_MaxTraits, "Gene_CognitiveInjuryResponse_MaxTraits", 3);
            Scribe_Values.Look<float>(ref Gene_CognitiveInjuryResponse_Chance, "Gene_CognitiveInjuryResponse_Chance", 1f);

            base.ExposeData();
        }
    }
}