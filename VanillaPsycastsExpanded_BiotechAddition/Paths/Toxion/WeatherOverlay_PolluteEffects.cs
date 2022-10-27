﻿using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;
using VFECore;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    internal class WeatherOverlay_PolluteEffects : SkyOverlay
    {
        public int nextDamageTick;
        public override void TickOverlay(Map map)
        {
            base.TickOverlay(map);
            if (VFEGlobal.settings.weatherDamagesOptions.TryGetValue(map.weatherManager.curWeather.defName, out var option) && !option)
            {
                return;
            }
            var options = map.weatherManager.curWeather.GetModExtension<WeatherEffectsExtension>();
            if (options != null)
            {
                if (options.activeOnWeatherPerceived is null || map.weatherManager.CurWeatherPerceived == options.activeOnWeatherPerceived)
                {
                    if (nextDamageTick == 0 || (Find.TickManager.TicksGame - nextDamageTick) > options.ticksInterval.max)
                    {
                        nextDamageTick = NextDamageTick(options);
                    }
                    if (Find.TickManager.TicksGame > nextDamageTick)
                    {
                        DoDamage(options, map);
                        DoPollute(options, map);
                        nextDamageTick = NextDamageTick(options);
                    }
                }
            }
        }

        public int NextDamageTick(WeatherEffectsExtension options)
        {
            return Find.TickManager.TicksGame + Rand.RangeInclusive(options.ticksInterval.min, options.ticksInterval.max);
        }

        public void DoDamage(WeatherEffectsExtension options, Map map)
        {
            for (int i = map.listerThings.AllThings.Count - 1; i >= 0; i--)
            {
                Thing thing = map.listerThings.AllThings[i];
                if (CanDamage(thing, map, options))
                {
                    if (thing is Pawn pawn)
                    {
                        DoPawnDamage(pawn, options);
                    }
                    else
                    {
                        DoThingDamage(thing, options);
                    }
                }
            }
            if (options.damageToApply != null)
            {
                var victimCandidates = map.mapPawns.AllPawns.Where(x => CanDamage(x, map, options));
                var victims = RandomlySelectedItems(victimCandidates, (int)(victimCandidates.Count() * options.percentOfPawnsToDealDamage)).ToList();
                for (int num = victims.Count - 1; num >= 0; num--)
                {
                    var damageInfo = new DamageInfo(options.damageToApply, options.damageRange.RandomInRange);
                    victims[num].TakeDamage(damageInfo);
                }
            }
        }

        public bool CanDamage(Thing thing, Map map, WeatherEffectsExtension options)
        {
            if (thing is Pawn pawn)
            {
                if (!pawn.RaceProps.IsFlesh && !options.worksOnNonFleshPawns)
                {
                    return false;
                }
            }

            if (!thing.Position.InBounds(map) || thing.Position.Roofed(map) && !options.worksIndoors)
            {
                return false;
            }
            return true;
        }
        public void DoPawnDamage(Pawn p, WeatherEffectsExtension options)
        {
            if (options.hediffsToApply != null)
            {
                foreach (var opt in options.hediffsToApply)
                {
                    var hediffDef = HediffDef.Named(opt.hediff);
                    if (hediffDef != null)
                    {
                        var severity = opt.severityOffset;

                        if (opt.effectMultiplyingStat != null) severity *= p.GetStatValue(opt.effectMultiplyingStat);

                        if (severity != 0f)
                        {
                            HealthUtility.AdjustSeverity(p, hediffDef, severity);
                        }
                    }
                }
            }
        }

        public void DoThingDamage(Thing thing, WeatherEffectsExtension options)
        {
            if (options.killsPlants && thing is Plant)
            {
                if (Rand.Value < options.chanceToKillPlants)
                {
                    thing.Kill(null, null);
                }
            }

            else if (thing.def.category == ThingCategory.Item)
            {
                CompRottable compRottable = thing.TryGetComp<CompRottable>();
                if (options.causesRotting && compRottable != null && compRottable.Stage < RotStage.Dessicated)
                {
                    compRottable.RotProgress += options.rotProgressPerDamage;
                }
            }
        }

        public static IEnumerable<Pawn> RandomlySelectedItems(IEnumerable<Pawn> sequence, int count)
        {
            return sequence.InRandomOrder().Take(count);
        }

        public void DoPollute(WeatherEffectsExtension options, Map map)
        {
            int targetCell = Rand.RangeInclusive(0,map.AllCells.Count()-1);
            List<IntVec3> Celllist = (List<IntVec3>)map.AllCells;
            if(Celllist[targetCell].CanPollute(map) && Celllist[targetCell].IsValid && !Celllist[targetCell].IsPolluted(map))
            {
                Celllist[targetCell].Pollute(map);
                map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(Celllist[targetCell], map, Vector3.zero, 1f), Celllist[targetCell], 45);
            }
        }
    }
}