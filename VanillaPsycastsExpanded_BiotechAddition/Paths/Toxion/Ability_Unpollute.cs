using System.Collections.Generic;
using Verse;
using RimWorld.Planet;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_Unpollute : Ability
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
                        if (intVec.IsPolluted(target.Map) && intVec.CanUnpollute(target.Map))
                        {
                            intVec.Unpollute(target.Map);
                            if (!pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_PollutionAccumulation))
                            {
                                pawn.health.AddHediff(VPEBA_DefOf.VPEBA_PollutionAccumulation);
                            }
                            if (pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_PollutionAccumulation))
                            {
                                pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity += 0.01f;
                            }
                        }
                    }
                }
            }
        }
    }
}
