using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace TelolRace
{
    public class QuestNode_Root_TelolPodCrash : QuestNode_Root_RefugeePodCrash
    {
        public override Pawn GeneratePawn() => GeneratePawn_NewTemp(null);

        public override Pawn GeneratePawn_NewTemp(Map map)
        {
            var pawn = TelolXenotypeHelper.GeneratePawn();
            
            if (pawn.Faction != Faction.OfPlayer)
            {
                Log.Warning("[TelolXenotype] This is okay, but notable: correcting generated pawn to player faction.");
                pawn.SetFaction(Faction.OfPlayer);
            }
            HealthUtility.DamageUntilDowned(pawn);
            return pawn;
        }
        
        public override void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
        {
            TaggedString title = "TelolXenotype_Letter_PodCrash_Label".Translate();
            TaggedString text = "TelolXenotype_Letter_PodCrash_Text".Translate(pawn.Named("PAWN")).AdjustedFor(pawn);

            if (pawn.DevelopmentalStage.Juvenile())
            {
                string ageString = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod();
                text += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), ageString.Named("AGE"));
            }

            QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter(
                "JoinerCharityInfo".Translate((NamedArgument)(Thing)pawn), ref text);
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref title, pawn);

            Find.LetterStack.ReceiveLetter(title, text, LetterDefOf.PositiveEvent, new TargetInfo(pawn));
        }
        
    }
}