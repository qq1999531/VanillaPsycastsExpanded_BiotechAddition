﻿using RimWorld.Planet;
using RimWorld;
using Verse;
using VanillaPsycastsExpanded;
using VanillaPsycastsExpanded.Staticlord;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class PollutionRainMaker : Thing
    {
        private GameCondition caused;
        public Pawn Pawn;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (respawningAfterLoad) return;
            this.caused = GameConditionMaker.MakeConditionPermanent(VPEBA_DefOf.VPEBA_PollutionRain_Condition);
            this.caused.conditionCauser = this;
            map.GameConditionManager.RegisterCondition(this.caused);
            this.Map.weatherManager.TransitionTo(VPEBA_DefOf.VPEBA_PollutionRain_Weather);
            this.Map.weatherDecider.StartNextWeather();
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            this.caused.End();
            this.Map.weatherDecider.StartNextWeather();
            base.Destroy(mode);
        }

        public override void Tick()
        {
            if (!this.Pawn.psychicEntropy.TryAddEntropy(1f, this) || this.Pawn.Downed) this.Destroy();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref this.caused, "caused");
            Scribe_References.Look(ref this.Pawn, "pawn");

            if (Scribe.mode == LoadSaveMode.PostLoadInit) this.caused.conditionCauser = this;
        }
    }
    public class Ability_PollutionRain : Ability, IAbilityToggle, IChannelledPsycast
    {
        private PollutionRainMaker maker;

        public bool Toggle
        {
            get => this.maker is { Spawned: true };
            set
            {
                if (value)
                    this.DoAction();
                else
                    this.maker?.Destroy();
            }
        }

        public string OffLabel => "VPEBA.StopPollutionRain".Translate();
        public bool IsActive => this.maker is { Spawned: true };

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            this.maker = (PollutionRainMaker)ThingMaker.MakeThing(VPEBA_DefOf.VPEBA_PollutionRainMaker);
            this.maker.Pawn = this.pawn;
            GenSpawn.Spawn(this.maker, this.pawn.Position, this.pawn.Map);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref this.maker, nameof(this.maker));
        }

        public override Gizmo GetGizmo() => new Command_AbilityToggle(this.pawn, this);
    }
}
