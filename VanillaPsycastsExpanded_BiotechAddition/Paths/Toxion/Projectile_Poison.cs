using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using VFECore.Abilities;


namespace VanillaPsycastsExpanded_BiotechAddition
{
    public class Projectile_Poison : AbilityProjectile
    {
        BodyPartRecord[] vulnerableParts = new BodyPartRecord[10];
        Pawn hitPawn = null;
        Pawn caster = null;
        protected override void DoImpact(Thing hitThing, Map map)
        {
            caster = launcher as Pawn;
            float power = 3 * this.ability.GetPowerForPawn();
                hitPawn = hitThing as Pawn;
                    Initialize(hitPawn);
                    damageEntities(hitPawn, null, 4, this.def.projectile.damageDef);
            if (!hitPawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_Poisoned))
            {
                int rndPart = Rand.RangeInclusive(0, 4);
                Hediff hediff = HediffMaker.MakeHediff(VPEBA_DefOf.VPEBA_Poisoned, hitPawn, vulnerableParts[rndPart]);
                hitPawn.health.AddHediff(hediff);
                VPEBA_MoteMaker.ThrowPoisonMote(hitPawn.Position.ToVector3(), map, 2.2f);
            }
            else
            {
                HealthUtility.AdjustSeverity(hitPawn, VPEBA_DefOf.VPEBA_Poisoned, Rand.Range(1f + power, 4f + (2f * power)));
                VPEBA_MoteMaker.ThrowPoisonMote(hitPawn.Position.ToVector3(), map, 2.2f);
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
        public void damageEntities(Pawn victim, BodyPartRecord hitPart, int amt, DamageDef type)
        {
            DamageInfo dinfo;
            amt = Mathf.RoundToInt(amt * Rand.Range(.5f, 1.2f) * 3 * this.ability.GetPowerForPawn());
            if (this.caster != null && victim != null && !victim.Dead && !victim.Downed && hitPart != null)
            {
                dinfo = new DamageInfo(type, amt, 0, (float)-1, this.caster, hitPart, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown);
                victim.TakeDamage(dinfo);
            }
        }
    }
}