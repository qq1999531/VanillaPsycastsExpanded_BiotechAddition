using System.Collections.Generic;
using Verse;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class HediffComp_GreyGoo : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (parent.Severity > 0)
            {
                using (IEnumerator<BodyPartRecord> enumerator = Pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        List<Hediff_Injury> arg_BB_0 = null; ;
                        Pawn.health.hediffSet.GetHediffs(ref arg_BB_0);
                        foreach (Hediff_Injury current in arg_BB_0)
                        {
                            if (parent.Severity > 0 && (current.CanHealFromTending() || current.CanHealNaturally()))
                            {
                                current.Heal(current.def.maxSeverity);
                                HealthUtility.AdjustSeverity(parent.pawn,parent.def,-1f);
                            }
                        }
                    }
                }
            }
        }
    }
}
