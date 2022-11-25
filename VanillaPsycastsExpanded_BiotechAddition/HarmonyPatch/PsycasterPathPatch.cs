using Verse;
using HarmonyLib;
using VanillaPsycastsExpanded;

namespace VanillaPsycastsExpanded_BiotechAddition.HarmonyPatches
{
    [HarmonyPatch(typeof(PsycasterPathDef), nameof(PsycasterPathDef.CanPawnUnlock))]
    public static class PsycasterPathDef_CanPawnUnlock_Patch
    {
        public static void Postfix(Pawn pawn, ref bool __result, PsycasterPathDef __instance)
        {
            if (__instance.HasModExtension<PsycasterPathExtension_BiotechRequirement>())
            {
                __result = __result && __instance.GetModExtension<PsycasterPathExtension_BiotechRequirement>().PawnHasGenes(pawn) && __instance.GetModExtension<PsycasterPathExtension_BiotechRequirement>().PawnIsXeno(pawn);
            }
        }
    }
}
