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
                        XenotypeDef template;
                        float chance = Rand.Range(0.0f, 100.0f);
                        if (chance > pawn.health.hediffSet.GetFirstHediffOfDef(VPEBA_DefOf.VPEBA_PollutionAccumulation).Severity)
                        {
                                template = VPEBA_DefOf.VPEBA_BadGeneTemplate;
                                List<GeneDef> Genelist = template.AllGenes;
                                AddRandomGene(Genelist, targetPawn, false, pawn);
                        }
                        else
                        {
                            template = VPEBA_DefOf.VPEBA_GoodGeneTemplate;
                            List<GeneDef> Genelist = template.AllGenes;
                            AddRandomGene(Genelist, targetPawn, false, pawn);
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
        public void AddRandomGene(List<GeneDef> Genelist,Pawn target,bool isXenoGene,Pawn caster)
        {
            foreach(GeneDef targetgene in Genelist)
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
