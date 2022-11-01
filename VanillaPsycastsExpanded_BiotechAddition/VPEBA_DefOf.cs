using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    [DefOf]
    public static class VPEBA_DefOf
    {
        public static HediffDef VPEBA_PollutionAccumulation;
        public static HediffDef VPEBA_GreyGoo;
        public static HediffDef VPEBA_Poisoned;
        public static HediffDef VPEBA_EmpowerMech;
        public static HediffDef VPEBA_DegradeMech;
        public static ThingDef VPEBA_Mote_Poison;
        public static ThingDef VPEBA_Fog_Tox;
        public static ThingDef VPEBA_PollutionRainMaker;
        public static XenotypeDef VPEBA_GoodGeneTemplate;
        public static XenotypeDef VPEBA_BadGeneTemplate;

        [DefAlias("VPEBA_PollutionRain")] public static WeatherDef VPEBA_PollutionRain_Weather;
        [DefAlias("VPEBA_PollutionRain")] public static GameConditionDef VPEBA_PollutionRain_Condition;
    }
}
