using RimWorld;
using Verse;

namespace TelolRace
{
    [DefOf]
    public static class TelolXenotypeDefOf
    {
        public static XenotypeDef TelolRace_Telol;

        static TelolXenotypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TelolXenotypeDefOf));
        }
    }
}