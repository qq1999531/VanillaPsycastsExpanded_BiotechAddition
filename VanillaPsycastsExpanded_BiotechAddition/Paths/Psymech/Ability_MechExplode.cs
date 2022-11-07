using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld.Planet;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_MechExplode : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            foreach (GlobalTargetInfo target in targets)
            {
                if (target.Thing is Pawn pawn && !pawn.mechanitor.ControlledPawns.NullOrEmpty())
                {
                    int rndPart = Rand.Range(0, pawn.mechanitor.ControlledPawns.Count());

                }
            }
        }
    }
}
