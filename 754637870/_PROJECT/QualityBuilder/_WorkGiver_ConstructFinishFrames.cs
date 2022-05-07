using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace QualityBuilder
{
    [HarmonyPatch(typeof(WorkGiver_ConstructFinishFrames), "JobOnThing")]
    public class _WorkGiver_ConstructFinishFrames
	{
        public static void Postfix(ref Job __result, Pawn pawn, Thing t, bool forced = false)
		{
            if (!forced && QualityBuilder.hasDesignation(t))
            {
                if (!isPawnGoodEnoughToBuild(pawn))
                    __result = null;
            }
		}

        private static bool pawnCapable(Pawn p)
        {
            Pawn_HealthTracker healthTracker = p.health;
            PawnCapacitiesHandler capacitiesHandler = healthTracker.capacities;
            return capacitiesHandler.CapableOf(PawnCapacityDefOf.Manipulation) && capacitiesHandler.CapableOf(PawnCapacityDefOf.Moving);
        }

        public static bool isPawnGoodEnoughToBuild(Pawn pawn)
        {
            if (pawn == null || pawn.Faction != Faction.OfPlayer || pawn.skills == null)
                return false;
            Map curMap = pawn.Map;
            Pawn overridePawn = QualityBuilderModSettings.getBestConstructorOverride(curMap);
            if (overridePawn != null)
                return pawn == overridePawn;
            int curPawnLevel = pawn.skills.GetSkill(SkillDefOf.Construction).Level;
            if (QualityBuilderModSettings.getIgnoreQualityBuilderAtSkill(curMap) > curPawnLevel)
            {
                Pawn bestConstructor = curMap.mapPawns.FreeColonists.Where(p => (p.RaceProps.Humanlike && p.skills != null && p.workSettings.WorkIsActive(WorkTypeDefOf.Construction) && pawnCapable(p))).OrderByDescending<Pawn, int>(p => p.skills.GetSkill(SkillDefOf.Construction).Level).FirstOrDefault<Pawn>();
                if (bestConstructor == null)
                    return true;
                int bestConstructorLevel = bestConstructor.skills.GetSkill(SkillDefOf.Construction).Level;
                int targetLevel = bestConstructorLevel - QualityBuilderModSettings.getSkillDifferenceFromBestBuilder(curMap);
                if (targetLevel < 0)
                    targetLevel = 0;
                if (pawn.skills.GetSkill(SkillDefOf.Construction).Level < targetLevel)
                    return false;
            }
            return true;
        }
    }
}
