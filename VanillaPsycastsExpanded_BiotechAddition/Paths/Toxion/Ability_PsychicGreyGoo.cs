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
                            for(int i =0;i< curCell.GetThingList(target.Map).Count; i++)
                            {
                                Thing affected = curCell.GetThingList(target.Map)[i];
                                if (affected is Pawn victim)
                                {
                                    if (victim != null && !victim.Dead && victim != pawn)
                                    {
                                        {
                                            List<BodyPartRecord> partSearch = victim.def.race.body.AllParts;
                                            int rndPart = Rand.Range(0, partSearch.Count);
                                            adjustSeverity += 1f;
                                            curtarget = victim.Position;
                                            foreach (IntVec3 polluteCell in GenRadial.RadialCellsAround(curtarget, secondradius, true))
                                            {
                                                if (affected.Map?.pollutionGrid.EverPollutable(polluteCell) ?? false)
                                                {
                                                    if (!polluteCell.IsPolluted(victim.Map) && polluteCell.CanPollute(victim.Map))
                                                    {
                                                        polluteCell.Pollute(victim.Map, false);
                                                        victim.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(polluteCell, victim.Map, Vector3.zero, 1f), polluteCell, 45);
                                                    }
                                                }
                                            }
                                            victim.TakeDamage(new DamageInfo(DamageDefOf.Vaporize, 100, 1, -1, pawn, partSearch[rndPart]));
                                        }
                                    }
                                }
                                else
                                {
                                    if (affected != null)
                                    {
                                        curtarget = affected.Position;
                                        int percentageDamage = affected.MaxHitPoints / 4;
                                        adjustSeverity += 1f;
                                        foreach (IntVec3 polluteCell in GenRadial.RadialCellsAround(curtarget, secondradius, true))
                                        {
                                            if (affected.Map?.pollutionGrid.EverPollutable(polluteCell) ?? false)
                                            {
                                                if (!polluteCell.IsPolluted(affected.Map) && polluteCell.CanPollute(affected.Map))
                                                {
                                                    polluteCell.Pollute(affected.Map, false);
                                                    affected.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(polluteCell, affected.Map, Vector3.zero, 1f), polluteCell, 45);
                                                }
                                            }
                                        }
                                        affected.TakeDamage(new DamageInfo(DamageDefOf.Vaporize, percentageDamage, 1, -1, pawn));
                                    }
                                }
                            }
                        }
                    }
                }
                if (!pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_GreyGoo))
                {
                    Hediff hediff = HediffMaker.MakeHediff(VPEBA_DefOf.VPEBA_GreyGoo, pawn, null);
                    hediff.Severity = adjustSeverity;
                    pawn.health.AddHediff(hediff, pawn.health.hediffSet.GetBrain());
                }
                if (pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_GreyGoo))
                {
                    HealthUtility.AdjustSeverity(pawn, VPEBA_DefOf.VPEBA_GreyGoo, adjustSeverity);
                }
            }
        }
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            bool check = target.Cell.IsValid;
            return check && base.ValidateTarget(target, showMessages);
        }
    }
}
