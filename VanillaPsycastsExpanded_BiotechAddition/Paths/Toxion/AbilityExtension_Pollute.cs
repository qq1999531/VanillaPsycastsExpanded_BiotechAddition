using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld.Planet;
using RimWorld;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class AbilityExtension_Pollute : AbilityExtension_AbilityMod
    {
        public float radius = 1f;
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (GlobalTargetInfo target in targets)
            {
                IEnumerable<IntVec3> targetsloc = GenRadial.RadialCellsAround(target.Cell, radius, true);
                foreach(IntVec3 intVec in targetsloc)
                {
                    if (!intVec.IsPolluted(target.Map) && intVec.CanPollute(target.Map))
                    {
                        intVec.Pollute(target.Map, false);
                        target.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, target.Map, Vector3.zero, 1f), intVec, 45);
                    }
                }
            }
        }
    }
}
