using RimWorld.QuestGen;
using Verse;

namespace TelolRace
{
    public class QuestNode_Root_TelolPodCrash : QuestNode_Root_RefugeePodCrash
    {
        public override Pawn GeneratePawn()
        {
            var pawn = TelolXenotypeHelper.GeneratePawn();
            HealthUtility.DamageUntilDowned(pawn);
            return pawn;
        }
    }
}