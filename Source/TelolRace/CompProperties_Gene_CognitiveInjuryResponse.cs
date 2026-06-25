using Verse;

namespace TelolRace
{
    public class CompProperties_Gene_CognitiveInjuryResponse : CompProperties
    {
        public CompProperties_Gene_CognitiveInjuryResponse()
        {
            this.compClass = typeof (Gene_CognitiveInjuryResponse);
        }
    }

    public class Gene_CognitiveInjuryResponse : ThingComp
    {
        public int lastTriggerTick = -1;
        public int traitsGranted = 0;
        
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref this.lastTriggerTick, "lastTriggerTick");
            Scribe_Values.Look<int>(ref this.traitsGranted, "traitsGranted");
        }
    }
}