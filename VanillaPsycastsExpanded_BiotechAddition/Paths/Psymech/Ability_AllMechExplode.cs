using RimWorld;
using Verse;
using RimWorld.Planet;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_AllMechExplode : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            foreach (GlobalTargetInfo target in targets)
            {
                if (target.Thing is Pawn pawn)
                {
                    foreach(Pawn victim in pawn.mechanitor.ControlledPawns)
                    {
                        victim.TakeDamage(new DamageInfo(DamageDefOf.Bomb, victim.MaxHitPoints, 1, -1, victim));
                        GenExplosion.DoExplosion(victim.Position, victim.Map, 3, DamageDefOf.Bomb, victim, victim.MaxHitPoints / 2, 0);
                    }
                }
            }
        }
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (target.HasThing && target.Thing is Pawn pawn)
            {
                if (!pawn.mechanitor.ControlledPawns.NullOrEmpty())
                {
                    return base.ValidateTarget(target, showMessages);
                }
            }
            return false;
        }
    }
}
