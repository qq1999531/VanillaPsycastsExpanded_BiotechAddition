using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class HediffComp_PolluteRadius : HediffComp
    {
        HediffCompProperties_PolluteRadius Props => props as HediffCompProperties_PolluteRadius;
        public int tickCounter = 0;
        public override void CompPostTick(ref float severityAdjustment)
        {
            tickCounter++;
            if (tickCounter > Props.tickInterval && Pawn.Spawned)
            {
                GasUtility.AddGas(Pawn.Position, Pawn.Map, GasType.ToxGas, Props.radius);
                IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(Pawn.Position, Props.radius, true);
                foreach(IntVec3 intVec in targets)
                {
                    if (!intVec.IsPolluted(Pawn.Map) && intVec.CanPollute(Pawn.Map))
                    {
                        if (Pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_PollutionAccumulation))
                        {
                            Pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Heal(1f);
                            intVec.Pollute(Pawn.Map, false);
                            Pawn.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, Pawn.Map, Vector3.zero, 1f), intVec, 45);
                        }
                    }
                }
                tickCounter = 0;
            }
        }
    }
    class HediffCompProperties_PolluteRadius : HediffCompProperties
    {
        public int tickInterval = 60;
        public float radius = 1f;
        public HediffCompProperties_PolluteRadius()
        {
            this.compClass = typeof(HediffComp_PolluteRadius);
        }
    }
}
