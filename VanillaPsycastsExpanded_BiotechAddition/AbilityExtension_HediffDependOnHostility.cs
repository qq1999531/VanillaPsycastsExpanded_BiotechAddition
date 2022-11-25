using RimWorld;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VanillaPsycastsExpanded_BiotechAddition
{
	public class AbilityExtension_HediffDependOnHostility : AbilityExtension_AbilityMod
	{
		public override bool ValidateTarget(LocalTargetInfo target, Ability ability, bool throwMessages = false)
		{
			if (target.Thing != null && target.Thing is Pawn targetPawn)
			{
				if (applyToMech == true && !targetPawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false) || targetPawn.GetOverseer() == null)
				{
					if (throwMessages)
					{
						//Messages.Message("VFEA.TargetMustBeMechRelated".Translate(), target.Thing, MessageTypeDefOf.CautionInput, null, true);
					}
					return false;
				}
			}
				return base.ValidateTarget(target, ability, throwMessages);
		}
		public HediffDef goodhediff;
		public HediffDef badhediff;
		public BodyPartDef bodyPartToApply;
		public float severity = -1f;
		public StatDef durationMultiplier;
		public bool durationMultiplierFromCaster;
		public bool applyToCaster;
		public bool applyToMech;
	}
}
