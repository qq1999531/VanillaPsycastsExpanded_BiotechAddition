using System.Collections.Generic;
using Verse;
using RimWorld.Planet;
using RimWorld;
using UnityEngine;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_PsychicGreyGoo : Ability
    {
        private float radius;
        private float secondradius;
        private float adjustSeverity = 0;
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            if (pawn.Spawned && def.HasModExtension<AbilityExtension_Radius>())
            {
                radius = def.GetModExtension<AbilityExtension_Radius>().radius;
                secondradius = def.GetModExtension<AbilityExtension_Radius>().secondradius;
                foreach (GlobalTargetInfo target in targets)
                {
                    if (target.Cell.IsValid)
                    {
                        IntVec3 curtarget;
                        IEnumerable<IntVec3> applyCell = GenRadial.RadialCellsAround(target.Cell, radius, true);
                        foreach(IntVec3 curCell in applyCell)
                        {
                            foreach (Thing affected in curCell.GetThingList(target.Map))
                            {
                                if (affected is Pawn victim)
                                {
                                    if (victim != null && !victim.Dead && victim != pawn)
                                    {
                                        {
                                            List<BodyPartRecord> partSearch = victim.def.race.body.AllParts;
                                            int rndPart = Rand.Range(0, partSearch.Count);
                                            victim.TakeDamage(new DamageInfo(DamageDefOf.Vaporize, 100, 1, -1, pawn, partSearch[rndPart]));
                                            adjustSeverity += 1f;
                                            curtarget = victim.Position;
                                            IEnumerable<IntVec3> applyPolluteCell = GenRadial.RadialCellsAround(curtarget, secondradius, true);
                                            foreach(IntVec3 polluteCell in applyPolluteCell)
                                            {
                                                if (polluteCell.IsValid)
                                                {
                                                    if (!polluteCell.IsPolluted(victim.Map) && polluteCell.CanPollute(victim.Map))
                                                    {
                                                        polluteCell.Pollute(victim.Map, false);
                                                        victim.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(polluteCell, victim.Map, Vector3.zero, 1f), polluteCell, 45);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int percentageDamage = affected.HitPoints / 4;
                                    affected.TakeDamage(new DamageInfo(DamageDefOf.Vaporize, percentageDamage, 1, -1, pawn));
                                    adjustSeverity += 1f;
                                    curtarget = affected.Position;
                                    IEnumerable<IntVec3> applyPolluteCell = GenRadial.RadialCellsAround(curtarget, secondradius, true);
                                    foreach (IntVec3 polluteCell in applyPolluteCell)
                                    {
                                        if (polluteCell.IsValid)
                                        {
                                            if (!polluteCell.IsPolluted(affected.Map) && polluteCell.CanPollute(affected.Map))
                                            {
                                                polluteCell.Pollute(affected.Map, false);
                                                affected.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(polluteCell, affected.Map, Vector3.zero, 1f), polluteCell, 45);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_GreyGoo))
                {
                    Hediff hediff = HediffMaker.MakeHediff(VPEBA_DefOf.VPEBA_GreyGoo, this.pawn, null);
                    hediff.Severity = adjustSeverity;
                    pawn.health.AddHediff(hediff, pawn.health.hediffSet.GetBrain());
                }
                if (pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_GreyGoo))
                {
                    HealthUtility.AdjustSeverity(pawn, VPEBA_DefOf.VPEBA_GreyGoo, adjustSeverity);
                }
            }
        }
    }
}
