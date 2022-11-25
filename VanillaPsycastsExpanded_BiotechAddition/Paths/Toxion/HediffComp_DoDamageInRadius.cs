using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class HediffComp_DoDamageInRadius : HediffComp
    {
        HediffCompProperties_DoDamageInRadius Props => props as HediffCompProperties_DoDamageInRadius;
        public int tickCounter = 0;
        public override void CompPostTick(ref float severityAdjustment)
        {
            tickCounter++;
            if (tickCounter > Props.tickInterval && Pawn.Spawned)
            {
                IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(Pawn.Position, Props.radius, true);
                foreach(IntVec3 curCell in targets)
                {
                    if (curCell.IsValid)
                    {
                        foreach (Thing affected in curCell.GetThingList(Pawn.Map))
                        {
                            if (affected is Pawn victim)
                            {
                                if (victim != null && !victim.Dead && victim.RaceProps.IsFlesh && victim != Pawn)
                                {
                                    {
                                        victim.TakeDamage(new DamageInfo(Props.damageType, Props.damageAmount, Props.armorPenetration, -1, Pawn));
                                    }
                                }
                            }
                        }
                    }
                }
                tickCounter = 0;
            }
        }
    }
    class HediffCompProperties_DoDamageInRadius : HediffCompProperties
    {
        public int tickInterval = 60;
        public int damageAmount = -1;
        public float radius = 1f;
        public float armorPenetration = 1f;
        public DamageDef damageType;
        public HediffCompProperties_DoDamageInRadius()
        {
            this.compClass = typeof(HediffComp_DoDamageInRadius);
        }
    }
}
