using Verse;
using RimWorld.Planet;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_ApplyEmpowerToControlledMech : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            foreach (GlobalTargetInfo target in targets)
            {
                if(target.Thing is Pawn pawn && !pawn.mechanitor.ControlledPawns.NullOrEmpty())
                {
                    foreach(Pawn controlledPawn in pawn.mechanitor.ControlledPawns)
                    {
                        Hediff localHediff = HediffMaker.MakeHediff(VPEBA_DefOf.VPEBA_EmpowerMech, controlledPawn, null);
                            localHediff.Severity = 1f;
                        if (localHediff is HediffWithComps hwc)
                            foreach (HediffComp hediffComp in hwc.comps)
                            {
                                if (hediffComp is HediffComp_Disappears hcd)
                                    hcd.ticksToDisappear = GetDurationForPawn();
                            }
                        controlledPawn.health.AddHediff(localHediff);
                    }
                }
            }
        }
    }
}
