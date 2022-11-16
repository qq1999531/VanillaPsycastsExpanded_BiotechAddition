using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld;
using Verse;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class Ability_ManipulateGene : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            if (pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity > 0)
            {
                foreach (GlobalTargetInfo target in targets)
                {
                    if (target.Thing is Pawn targetPawn)
                    {
                        float chance = Rand.Range(0.0f, 100.0f);
                        float chance2 = Rand.Range(0.0f, 100.0f);
                        if (chance > pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity)
                        {
                            
                            if (chance2 > pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity)
                            {
                                AddRandomGene(VPEBA_DefOf.VPEBA_WorseGeneTemplate, targetPawn, false, pawn);
                                break;
                            }
                                AddRandomGene(VPEBA_DefOf.VPEBA_BadGeneTemplate, targetPawn, false, pawn);
                        }
                        else
                        {
                            if (chance2 > pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity)
                            {
                                AddRandomGene(VPEBA_DefOf.VPEBA_GoodGeneTemplate, targetPawn, false, pawn);
                                break;
                            }
                            AddRandomGene(VPEBA_DefOf.VPEBA_BetterGeneTemplate, targetPawn, false, pawn);
                        }
                    }
                }
            }
            
        }
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            bool check = pawn.health.hediffSet.HasHediff(VPEBA_DefOf.VPEBA_PollutionAccumulation) && pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity > 0;
            return check && base.ValidateTarget(target, showMessages);
        }
        public void AddRandomGene(XenotypeDef template, Pawn target,bool isXenoGene,Pawn caster)
        {
            List<GeneDef> Genelist = template.AllGenes;
            Genelist.Shuffle();
            foreach (GeneDef targetgene in Genelist)
            {
                if (!target.genes.HasGene(targetgene))
                {
                    target.genes.AddGene(targetgene, isXenoGene);
                    caster.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Heal(100);
                    return;
                }
            }
        }
    }
}
