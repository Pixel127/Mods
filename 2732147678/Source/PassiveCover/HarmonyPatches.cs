using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using Verse;

namespace PassiveCover
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.atkana.passivecover");

            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CoverUtility), nameof(CoverUtility.CalculateCoverGiverSet))]
    static class CalculateCoverGiverSetPatch
    {
        static void Postfix(ref List<CoverInfo> __result, LocalTargetInfo target, IntVec3 shooterLoc, Map map)
        {
            // Performance hack omitted here as the main performance hit is from CalcualteOverallBlockChance
            
            // If the setting is enabled, don't go on to add passive cover sources if the defender already has active cover protection
            if (LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().onlyPassiveIfNoActive == true && __result.Any())
            {
                return;
            }
            
            __result.AddRange(Main.GetAdjustedPassiveCover(target, shooterLoc, map));
        }
    }

    [HarmonyPatch(typeof(CoverUtility), nameof(CoverUtility.CalculateOverallBlockChance))]
    static class CalculateOverallBlockChancePatch
    {
        static void Postfix(ref float __result, LocalTargetInfo target, IntVec3 shooterLoc, Map map)
        {
            // Performance hack - Game can execute this function loads outside of just calculating aim.
            // Only run if this call is for shooting (flag is set elsewhere)
            if (!Main.executeThisTime && LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().onlyUseWhenShooting)
            {
                return;
            }

            Main.executeThisTime = false;
            
            // If the setting is enabled, don't go on to add passive cover protection if the defender already has active cover protection
            if (LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().onlyPassiveIfNoActive == true && __result > 0f)
            {
                return;
            }

            List<CoverInfo> passiveCovers = Main.GetAdjustedPassiveCover(target, shooterLoc, map);
            bool addedPassiveCover = false;

            foreach (var cover in passiveCovers)
            {
                if (__result >= 1f) continue;
                
                __result += (1f - __result) * cover.BlockChance;
                __result = Math.Min(__result, 1f); // Cap to prevent overflow with high numbers
                addedPassiveCover = true;
            }

            if (addedPassiveCover)
            {
                __result = Math.Min(__result, (LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().coverEffectivenessCap / 100));
            }
        }
    }

    /* Performance Hack Patches:
     * If the performance mode is enabled, we only ever want to do the block chance calculations when actually firing.
     * To do so, we set a flag whenever an appropriate use should be happening.
     * For best compatibility, we should probably do this in Verse.ShotReport.HitReportFor, as the majority of base game things that utilise that are for shooting
     * Unfortunately, one of them isn't - the ai check for avoiding friendly fire.
     * We could instead set the flag in the 2 shooting functions that use HitReportFor, but one of those is protected and I don't know how to do reflection alongside Harmony patches...
     */

    [HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitReportFor))]
    static class HitReportForPatch
    {
        static bool Prefix()
        {
            Main.executeThisTime = true;
            return true; // Don't want to actually prevent execution
        }
    }
}
