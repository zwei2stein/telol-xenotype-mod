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
    }
}