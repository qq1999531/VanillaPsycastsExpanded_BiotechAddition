using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    public static class VPEBA_MoteMaker
    {
        public static void ThrowPoisonMote(Vector3 loc, Map map, float scale)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(VPEBA_DefOf.VPEBA_Mote_Poison, null);
            moteThrown.Scale = 1.9f * scale;
            moteThrown.rotationRate = (float)Rand.Range(-60, 60);
            moteThrown.exactPosition = loc;
            moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.4f, 0.5f));
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
        }
    }
}