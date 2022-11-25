using RimWorld;
using Verse;
using UnityEngine;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class HediffComp_PolluteWhenOverCapacity : HediffComp
    {
        public int tickCounter = 0;
        HediffCompProperties_PolluteWhenOverCapacity Props => props as HediffCompProperties_PolluteWhenOverCapacity;
        public override void CompPostTick(ref float severityAdjustment)
        {
            if(tickCounter > Props.tickInterval)
            {
                if(Props.polluteChance > Rand.Range(0f, 1f) && Pawn.health.hediffSet.GetFirstHediffOfDef(Def).Severity > Props.severityCapacity)
                {
                    if (Pawn.Position.IsPolluted(Pawn.Map) && Pawn.Position.CanPollute(Pawn.Map))
                    {
                        Pawn.Position.Pollute(Pawn.Map);
                        Pawn.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(Pawn.Position, Pawn.Map, Vector3.zero, 1f), Pawn.Position, 45);
                        Pawn.health.hediffSet.GetFirstHediffOfDef(Def).Heal(1);
                    }
                }
                tickCounter = 0;
            }
        }
    }
    class HediffCompProperties_PolluteWhenOverCapacity : HediffCompProperties
    {
        public int tickInterval = 60;
        public float severityCapacity = 100;
        public float polluteChance = 1;
        public HediffCompProperties_PolluteWhenOverCapacity()
        {
            compClass = typeof(HediffComp_PolluteWhenOverCapacity);
        }
    }
}
