using HarmonyLib;
using Verse;
namespace VanillaPsycastsExpanded_BiotechAddition
{
    public class PsycastsBiotechAddMod : Mod
    {
        public static Harmony Harm;
        public PsycastsBiotechAddMod(ModContentPack content) : base(content)
        {
            Harm = new Harmony("SCXR.VanillaPsycastsExpanded_BiotechAddition");
            Harm.PatchAll();
        }
    }
}
