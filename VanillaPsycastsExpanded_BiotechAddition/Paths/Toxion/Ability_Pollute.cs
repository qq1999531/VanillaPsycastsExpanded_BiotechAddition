using System.Collections.Generic;
using Verse;
using RimWorld.Planet;
using RimWorld;
using UnityEngine;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_Pollute : Ability
    {
        private float radius;
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            if (pawn.Spawned && def.HasModExtension<AbilityExtension_Radius>())
            {
                radius = def.GetModExtension<AbilityExtension_Radius>().radius;
                foreach (GlobalTargetInfo target in targets)
                {
                    IEnumerable<IntVec3> targetsloc = GenRadial.RadialCellsAround(target.Cell, radius, true);
                    foreach (IntVec3 intVec in targetsloc)
                    {
                        if (!intVec.IsPolluted(target.Map) && intVec.CanPollute(target.Map))
                        {
                            intVec.Pollute(target.Map, false);
                            target.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, target.Map, Vector3.zero, 1f), intVec, 45);
                            if (pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_PollutionAccumulation))
                            {
                                pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Heal(0.01f);
                            }
                        }
                    }
                }
            }
        }
    }
}
