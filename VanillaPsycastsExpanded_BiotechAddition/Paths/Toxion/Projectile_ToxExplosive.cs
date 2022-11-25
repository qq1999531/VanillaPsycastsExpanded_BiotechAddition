using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using VFECore.Abilities;
namespace VanillaPsycastsExpanded_BiotechAddition
{
    public class Projectile_ToxExplosive : AbilityProjectile
    {
        private int duration = 360;
        private bool initialized = false;
        private float radius = 4;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.radius, "radius", 4);
        }
        protected override void DoImpact(Thing hitThing, Map map)
        {
                float power = ability.GetPowerForPawn();
                Pawn caster = launcher as Pawn;
                if (caster != null && !initialized)
                {
                    initialized = true;
                    radius = def.projectile.explosionRadius + (.8f * power);
                    duration = 360 + (int)(60 * power);
                    ThingDef fog = VPEBA_DefOf.VPEBA_Fog_Tox;
                    fog.gas.expireSeconds.min = duration / 60;
                    fog.gas.expireSeconds.max = duration / 60;
                    GenExplosion.DoExplosion(base.Position, map, radius, def.projectile.damageDef, this, 0, 0, SoundDef.Named("TinyBell"), def, null, null, fog, 1f, 1,GasType.ToxGas, false, null, 0f, 0, 0.0f, false);
                }
        }
    }
}
