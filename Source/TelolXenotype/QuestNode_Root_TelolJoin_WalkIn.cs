using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace TelolRace
{
    public class QuestNode_Root_TelolJoin_WalkIn : QuestNode_Root_WandererJoin_WalkIn
    {
        public override Pawn GeneratePawn() => GeneratePawn_NewTemp(null);

        public override Pawn GeneratePawn_NewTemp(Map map)
        {
            return TelolXenotypeHelper.GeneratePawn();
        }
        
        public override void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
        {
            TaggedString title = "TelolXenotype_Letter_WalkIn_Label".Translate();
            TaggedString text = "TelolXenotype_Letter_WalkIn_Text".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);

            AppendCharityInfoToLetter("JoinerCharityInfo".Translate((NamedArgument)(Thing)pawn), ref text);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref title, pawn);

            if (pawn.DevelopmentalStage.Juvenile())
            {
                string ageString = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod();
                text += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), ageString.Named("AGE"));
            }

            ApplyBestSkillInfoToLetter(ref text, pawn);

            var let = (ChoiceLetter_AcceptJoiner)LetterMaker.MakeLetter(title, text, LetterDefOf.AcceptJoiner);
            let.signalAccept = QuestGenUtility.HardcodedSignalWithQuestID("Accept");
            let.signalReject = QuestGenUtility.HardcodedSignalWithQuestID("Reject");
            let.quest = quest;
            let.overrideMap = map;
            let.StartTimeout(60000);
            Find.LetterStack.ReceiveLetter(let);
        }
        
    }
}