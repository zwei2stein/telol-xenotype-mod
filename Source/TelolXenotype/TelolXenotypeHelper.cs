using RimWorld;
using RimWorld.Planet;
using Verse;

namespace TelolRace
{
    public class TelolXenotypeHelper
    {
        public static Pawn GeneratePawn()
        {
            var request = new PawnGenerationRequest(
                PawnKindDefOf.Villager,
                fixedIdeo: Faction.OfPlayer?.ideos?.PrimaryIdeo,
                faction: Faction.OfPlayer ?? Faction.OfPlayerSilentFail,
                forcedXenotype: TelolXenotypeDefOf.TelolRace_Telol,
                colonistRelationChanceFactor: 20f,
                allowPregnant: true,
                forceNoGear: true,
                forceRecruitable: true);
            if (Find.Storyteller.difficulty.ChildrenAllowed)
                request.AllowedDevelopmentalStages |= DevelopmentalStage.Child;
            
            var pawn = PawnGenerator.GeneratePawn(request);

            if (!pawn.IsWorldPawn())
                Find.WorldPawns.PassToWorld(pawn);

            return pawn;
        }
    }
}