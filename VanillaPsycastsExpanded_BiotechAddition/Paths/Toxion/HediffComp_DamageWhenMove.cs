using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class HediffComp_DamageWhenMove : HediffComp
    {
        private IntVec3 oldPosition;
        private bool initialized = true;
        public int tickCounter = 0;
        BodyPartRecord[] vulnerableParts = new BodyPartRecord[10];
        HediffCompProperties_DamageWhenMove Props => props as HediffCompProperties_DamageWhenMove;
        public override void CompPostTick(ref float severityAdjustment)
        {
            tickCounter++;
            if (initialized)
            {
                Initialize(Pawn);
                oldPosition = Pawn.Position;
                initialized = false;
            }
            if (tickCounter > Props.tickInterval && Pawn.Spawned)
            {
                int rndPart = Rand.RangeInclusive(0, 4);
                int dmg = (int)(((Pawn.Position - oldPosition).LengthHorizontal) * 1.25f);
                Pawn.TakeDamage(new DamageInfo(Props.damageType, dmg,1,-1,null, vulnerableParts[rndPart]));
                VPEBA_MoteMaker.ThrowPoisonMote(Pawn.Position.ToVector3(), Pawn.Map, 1f);
                tickCounter = 0;
            }
        }
        public void Initialize(Pawn victim)
        {
            BodyPartRecord vitalPart = null;
            if (victim != null && !victim.Dead)
            {

                IEnumerable<BodyPartRecord> partSearch = victim.def.race.body.AllParts;
                vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.BloodPumpingSource));
                if (vitalPart != null)
                {
                    this.vulnerableParts[0] = vitalPart;
                }
                vitalPart = null;
                vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.BloodFiltrationKidney));
                if (vitalPart != null)
                {
                    this.vulnerableParts[1] = vitalPart;
                }
                vitalPart = null;
                vitalPart = partSearch.LastOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.BloodFiltrationKidney));
                if (vitalPart != null)
                {
                    this.vulnerableParts[2] = vitalPart;
                }
                vitalPart = null;
                vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.BloodFiltrationLiver));
                if (vitalPart != null)
                {
                    this.vulnerableParts[3] = vitalPart;
                }
                vitalPart = null;
                vitalPart = partSearch.LastOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.BloodFiltrationLiver));
                if (vitalPart != null)
                {
                    this.vulnerableParts[4] = vitalPart;
                }
            }
        }
    }
    class HediffCompProperties_DamageWhenMove : HediffCompProperties
    {
        public int tickInterval = 60;
        public int damageAmount = -1;
        public DamageDef damageType;
        public HediffCompProperties_DamageWhenMove()
        {
            compClass = typeof(HediffComp_DamageWhenMove);
        }
    }
}
