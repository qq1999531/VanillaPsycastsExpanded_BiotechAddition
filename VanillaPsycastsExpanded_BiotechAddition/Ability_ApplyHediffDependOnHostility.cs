using RimWorld;
using Verse;
using RimWorld.Planet;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_ApplyHediffDependOnHostility : Ability
    {
        private HediffDef goodhediff;
        private HediffDef badhediff;
        private float severity;
        private StatDef durationMultiplier;
        private bool applyToMech;
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            foreach (AbilityExtension_HediffDependOnHostility abilityExtension_HediffDependOnHostility in abilityModExtensions)
            {
                goodhediff = abilityExtension_HediffDependOnHostility.goodhediff;
                badhediff = abilityExtension_HediffDependOnHostility.badhediff;
                severity = abilityExtension_HediffDependOnHostility.severity;
                applyToMech = abilityExtension_HediffDependOnHostility.applyToMech;
                if (abilityExtension_HediffDependOnHostility.durationMultiplierFromCaster)
                {
                    durationMultiplier = abilityExtension_HediffDependOnHostility.durationMultiplier;
                }
            }
            foreach (GlobalTargetInfo target in targets)
            {
                if (target.Thing is Pawn pawn && !pawn.mechanitor.ControlledPawns.NullOrEmpty())
                {
                    foreach (Pawn controlledPawn in pawn.mechanitor.ControlledPawns)
                    {
                        Hediff localHediff;
                        if (pawn.HostileTo(base.pawn))
                        {
                            localHediff = HediffMaker.MakeHediff(badhediff, controlledPawn, null);
                        }
                        else
                        {
                            localHediff = HediffMaker.MakeHediff(goodhediff, controlledPawn, null);
                        }
                        localHediff.Severity = severity;
                        if (localHediff is HediffWithComps hwc)
                            foreach (HediffComp hediffComp in hwc.comps)
                            {
                                if (hediffComp is HediffComp_Disappears hcd)
                                {
                                    hcd.ticksToDisappear = GetDurationForPawn();
                                    if (durationMultiplier != null) hcd.ticksToDisappear = (int)(hcd.ticksToDisappear * base.pawn.GetStatValue(durationMultiplier));
                                }
                            }
                        controlledPawn.health.AddHediff(localHediff);
                    }
                }
                if (target.Thing is Pawn mechPawn && !mechPawn.RaceProps.IsMechanoid && applyToMech)
                {
                    Hediff localHediff;
                    if (mechPawn.HostileTo(base.pawn))
                    {
                        localHediff = HediffMaker.MakeHediff(badhediff, mechPawn, null);
                    }
                    else
                    {
                        localHediff = HediffMaker.MakeHediff(goodhediff, mechPawn, null);
                    }
                    localHediff.Severity = severity;
                    if (localHediff is HediffWithComps hwc)
                        foreach (HediffComp hediffComp in hwc.comps)
                        {
                            if (hediffComp is HediffComp_Disappears hcd)
                            {
                                hcd.ticksToDisappear = GetDurationForPawn();
                                if (durationMultiplier != null) hcd.ticksToDisappear = (int)(hcd.ticksToDisappear * base.pawn.GetStatValue(durationMultiplier));
                            }
                        }
                    mechPawn.health.AddHediff(localHediff);
                }
            }
        }
    }
}
