using System.Collections.Generic;
using Verse;
using RimWorld.Planet;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class AbilityExtension_Unpollute : AbilityExtension_AbilityMod
    {
        public float radius = 1f;
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (GlobalTargetInfo target in targets)
            {
                IEnumerable<IntVec3> targetsloc = GenRadial.RadialCellsAround(target.Cell, radius, true);
                foreach (IntVec3 intVec in targetsloc)
                {
                    if (intVec.IsPolluted(target.Map) && intVec.CanUnpollute(target.Map))
                    {
                        intVec.Unpollute(target.Map);
                    }
                }
            }
        }
    }
}
