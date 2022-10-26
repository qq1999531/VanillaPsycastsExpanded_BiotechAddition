using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace VanillaPsycastsExpanded_BiotechAddition
{
    class PsycasterPathExtension_BiotechRequirement : DefModExtension
    {
        public List<GeneDef> requiredGenes = new();
        public XenotypeDef requiredXenotype;

        public bool PawnHasGenes(Pawn pawn)
        {
            if (requiredGenes.NullOrEmpty()) return true;
            foreach (var requirement in requiredGenes)
                if ((pawn.genes?.HasXenogene(requirement) ?? false) || (pawn.genes?.HasEndogene(requirement) ?? false))
                    return true;
            return false;
        }
        public bool PawnIsXeno(Pawn pawn) => pawn.genes?.Xenotype == requiredXenotype;
    }
}
