using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using VFECore.Abilities;
namespace VanillaPsycastsExpanded_BiotechAddition
{
    public class Projectile_ToxExplosive : AbilityProjectile
    {
        private int age = -1;
        private int duration = 360;
        private bool initialized = false;
        private float radius = 4;
        private int strikeDelay = 40;
        private int lastStrike = 0;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.age, "age", 0);
            Scribe_Values.Look<float>(ref this.radius, "radius", 4);
        }
        protected override void DoImpact(Thing hitThing, Map map)
        {
            age++;
            if (age < duration)
            {
                float power = this.ability.GetPowerForPawn();
                Pawn caster = this.launcher as Pawn;
                if (caster != null && !initialized)
                {
                    initialized = true;
                    radius = this.def.projectile.explosionRadius + (.8f * power);
                    duration = 360 + (int)(60 * power);
                    ThingDef fog = VPEBA_DefOf.VPEBA_Fog_Tox;
                    fog.gas.expireSeconds.min = duration / 60;
                    fog.gas.expireSeconds.max = duration / 60;
                    GenExplosion.DoExplosion(base.Position, map, radius, this.def.projectile.damageDef, this, 0, 0, SoundDef.Named("TinyBell"), def, null, null, fog, 1f, 1,GasType.ToxGas, false, null, 0f, 0, 0.0f, false);
                }

                if (this.age >= this.lastStrike + this.strikeDelay)
                {
                    try
                    {
                        List<Pawn> pList = (from pawn in this.Map.mapPawns.AllPawnsSpawned
                                            where (!pawn.Dead && (pawn.Position - base.Position).LengthHorizontal <= radius && pawn.RaceProps != null && pawn.RaceProps.IsFlesh)
                                            select pawn).ToList();

                        for (int i = 0; i < pList.Count(); i++)
                        {
                            Pawn victim = pList[i];
                            List<BodyPartRecord> bprList = new List<BodyPartRecord>();
                            bprList.Clear();
                            BodyPartRecord bpr = null;
                            foreach (BodyPartRecord record in victim.def.race.body.AllParts)
                            {
                                if (record.def.tags.Contains(BodyPartTagDefOf.BreathingSource) || record.def.tags.Contains(BodyPartTagDefOf.BreathingPathway))
                                {
                                    if (victim.health != null && victim.health.hediffSet != null && !victim.health.hediffSet.PartIsMissing(record))
                                    {
                                        bprList.Add(record);
                                    }
                                }
                            }
                            if (bprList != null && bprList.Count > 0 && caster != null)
                            {
                                float amt = Rand.Range(1f, 2f);
                                amt = Rand.Range(amt * .75f, amt * 1.25f);
                                DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, amt, 2f, (float)-1, this.launcher, bprList.RandomElement(), this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown);
                                hitThing.TakeDamage(dinfo);
                            }
                        }
                    }
                    catch
                    {
                        Log.Message("Debug: poison trap failed to process triggered event - terminating poison trap");
                        age = this.duration;
                    }
                    this.lastStrike = this.age;
                }
            }
        }
    }
}
