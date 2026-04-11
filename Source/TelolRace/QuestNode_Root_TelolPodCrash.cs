using RimWorld.QuestGen;
using Verse;

namespace TelolRace
{
    public class QuestNode_Root_TelolPodCrash : QuestNode_Root_RefugeePodCrash
    {
        public override Pawn GeneratePawn()
        {
            return TelolXenotypeHelper.GeneratePawn();
        }
    }
}