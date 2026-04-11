using RimWorld.QuestGen;
using Verse;

namespace TelolRace
{
    public class QuestNode_Root_TelolJoin_WalkIn : QuestNode_Root_WandererJoin_WalkIn
    {
        public override Pawn GeneratePawn()
        {
            return TelolXenotypeHelper.GeneratePawn();
        }
    }
}