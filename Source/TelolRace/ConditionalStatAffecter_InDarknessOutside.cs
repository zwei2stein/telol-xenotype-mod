using RimWorld;
using Verse;

namespace TelolRace
{
    public class ConditionalStatAffecter_InDarknessOutside : ConditionalStatAffecter
    {
        public override string Label => (string)"TelolXenotype_StatsReport_InDarknessOutside".Translate();

        public override bool Applies(StatRequest req)
        {
            return ModsConfig.BiotechActive && req.HasThing && req.Thing.Spawned &&
                   DarknessCombatUtility.IsOutdoorsAndDark(req.Thing);
        }
    }
}