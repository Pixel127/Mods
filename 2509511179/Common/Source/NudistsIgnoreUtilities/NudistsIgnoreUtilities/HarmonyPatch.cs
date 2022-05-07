using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace UtilityPatch
{
    [StaticConstructorOnStartup]
    public class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("com.turnovus.utilitypatch");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(ThoughtWorker_NudistNude))]
    [HarmonyPatch("CurrentStateInternal")]
    class NudistNudePatch
    {

        static void Postfix(Pawn p, ref ThoughtState __result)
        {
            bool nudistsIgnoreUtility = LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().nudistsIgnoreUtility;
            if (__result.Active || !nudistsIgnoreUtility)
                return;
            List<Apparel> wornApparel = p.apparel.WornApparel;
            bool noUtilities = true;
            foreach (Apparel apparel in wornApparel)
            {
                foreach (BodyPartGroupDef bodyPart in apparel.def.apparel.bodyPartGroups)
                {
                    if (bodyPart == BodyPartGroupDefOf.Torso || bodyPart == BodyPartGroupDefOf.Legs)
                    {
                        if (!apparel.def.apparel.layers.Contains(ApparelLayerDefOf.Belt) &
                            !apparel.def.apparel.layers.Contains(UtilityDefOf.PacksAreNotBelts_Tactical))
                            noUtilities = false;
                    }
                }
            }
            if (noUtilities)
                __result = true;
        }

    }

    [HarmonyPatch(typeof(DamageWorker))]
    [HarmonyPatch("Apply")]
    class DamagePatch
    {
        public static bool IsUtility(Apparel a)
        {
            return a.def.apparel.layers.Where(l => l.IsUtilityLayer).Any();
        }

        static bool Prefix(ref DamageWorker.DamageResult __result, Thing victim)
        {
            bool utilityIgnoresDamage = LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().utilityIgnoresDamage;
            if (utilityIgnoresDamage && victim is Apparel a && a.Wearer != null && IsUtility(a))
            {
                DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
                damageResult.totalDamageDealt = 0.0f;
                __result = damageResult;
                return false;
            }
            return true;
        }
    }

}
