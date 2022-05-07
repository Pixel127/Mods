using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;
using HeavyWeapons;
using HarmonyLib;

namespace HeavyWeaponsAcceptAllUtilities
{
    [StaticConstructorOnStartup]
    public class HarmonyPatches
    {
        static HarmonyPatches()
        {
            //Log.Message("Packs Are Not Belts - Harmony Patches Started.");
            var harmony = new Harmony("com.turnovus.utilitypatch");
            harmony.PatchAll();
            Log.Message("Packs Are Not Belts - Harmony Patches Loaded.");
        }
    }

    //[HarmonyPatch(typeof(Patch_FloatMenuMakerMap.AddHumanlikeOrders_Fix))]
    //[HarmonyPatch("CanEquip")]
    [HarmonyPatch]
    class HeavyWeaponsPatch
    {

        static bool Prepare()
        {
            foreach (ModContentPack mod in Verse.LoadedModManager.RunningModsListForReading)
            {
                if (mod.Name == "Vanilla Expanded Framework")
                {
                    Log.Message("Packs Are Not Belts - Found Vanilla Expanded Framework - Patching HeavyWeapons.dll");
                    return true;
                }
            }
            return false;
        }

        static MethodBase TargetMethod()
        {
            return typeof(Patch_FloatMenuMakerMap.AddHumanlikeOrders_Fix).GetMethod("CanEquip");
        }


        static void Postfix(Pawn pawn, DefModExtension options, ref bool __result)
        {
            /**
            if (options.GetType().GetField("supportedArmors") == null)
                return;
            **/
            if (pawn.apparel.WornApparel != null)
            {
                foreach (Apparel apparel in pawn.apparel.WornApparel)
                {
                    if (apparel.def.apparel.layers.Contains(ApparelLayerDefOf.Belt) || apparel.def.apparel.layers.Contains(MyDefOf.PacksAreNotBelts_Exoskeleton))
                    {
                        FieldInfo supportedArmorField = options.GetType().GetField("supportedArmors");
                        List<String> supportedArmors = supportedArmorField.GetValue(options) as List<String>;
                        if (supportedArmors != null && supportedArmors.Contains(apparel.def.defName))
                        {
                            __result = true;
                        }
                    }
                }
            }
        }

    }

    [HarmonyPatch(typeof(ApparelLayerDef))]
    [HarmonyPatch("IsUtilityLayer", MethodType.Getter)]
    class UtilityLayerPatch
    {
        static void Postfix(ApparelLayerDef __instance, ref bool __result)
        {
            if (__instance.HasModExtension<DefExtension_IsUtilityLayer>())
            {
                __result = true;
            }
        }
    }
}
