using System;
using Verse;

namespace TelolRace
{
    public enum ProgressionSpeed : byte
    {
        SECCOND,
        DAY,
        QUADRUM,
        YEAR
    }

    public static class ProgressionSpeedExtensions
    {
        public static string ToStringHuman(this ProgressionSpeed mode)
        {
            switch (mode)
            {
                case ProgressionSpeed.SECCOND:
                    return "TelolXenotype_ProgressionSpeed_SECOND".Translate();
                case ProgressionSpeed.DAY:
                    return "TelolXenotype_ProgressionSpeed_DAY".Translate();
                case ProgressionSpeed.QUADRUM:
                    return "TelolXenotype_ProgressionSpeed_QUADRUM".Translate();
                case ProgressionSpeed.YEAR:
                    return "TelolXenotype_ProgressionSpeed_YEAR".Translate();
                default:
                    throw new NotImplementedException();
            }
        }
        
        public static int ToTicks(this ProgressionSpeed mode)
        {
            switch (mode)
            {
                case ProgressionSpeed.SECCOND:
                    return 60;
                case ProgressionSpeed.DAY:
                    return 60000;
                case ProgressionSpeed.QUADRUM:
                    return 900000;
                case ProgressionSpeed.YEAR:
                    return 3600000;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}