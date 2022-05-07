using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace UtilityPatch
{
    [StaticConstructorOnStartup]
    public static class RimmuNationPatcher
    {
        static readonly List<string> defsToPatchString = new List<string>()
        {
            "RHApparel_M56Webbing_Vietnam",
            "RHApparel_ALICEWebbing_ColdWar",
            "RHApparel_RRV_ChestRig",
            "RHApparel_USMC_ChestRig",
            "RHApparel_CondorRecon_ChestRig",
            "RHApparel_CryeAVSTan_ChestRig",
            "RHApparel_MilTec_ChestRig",
            "RHApparel_WarriorFalcon_ChestRig",
            "RHApparel_CondorBattleBelt_Tan",
            "RHApparel_VTACBattleBelt_Tan",
            "RHApparel_SuperBelt_Tan",
            "RHApparel_Type56Webbing_ColdWar",
            "RHApparel_Type63Webbing_ColdWar",
            "RHApparel_M23_ChestRig",
            "RHApparel_KulaMongoose_ChestRig",
            "RHApparel_Arktis_TacticalVest",
            "RHApparel_TexShoemakerPoliceBelt_Python",
            "RHApparel_SafarilandPoliceBelt_Beretta",
            "RHApparel_SafarilandPoliceBelt_Glock",
        };

        static readonly List<string> defsToPatchEftArmor = new List<string>()
        {
            "JDSTarkov_6B5-16_Zh-86_rig",
            "JDSTarkov_6B3TM-01M",
            "JDSTarkov_6B5-15_Zh-86_rig",
            "JDSTarkov_ANA_Tactical_M2_rig",
            "JDSTarkov_ANA_Tactical_M1_rig",
            "JDSTarkov_Crye_Precision_AVS",
            "JDSTarkov_Ars_Arma_A18_Skanda",
            "JDSTarkov_Wartech_TV-110_rig",
            "JDSTarkov_5_11_Tactec_plate_carrier",
            "JDSTarkov_Ars_Arma_CPC_MOD_2_rig",
        };

        static readonly List<string> defsToPatchEft = new List<string>()
        {
            "JDSTarkov_Scav_Vest",
            "JDSTarkov_Spiritus_Systems",
            "JDSTarkov_SOE_Micro",
            "JDSTarkov_Wartech_TV-109_Rig",
            "JDSTarkov_UMTBS_6sh112_Rig",
            "JDSTarkov_Splav_Tarzan_M22_Rig",
            "JDSTarkov_Haley_Strategic_D3CRX",
            "JDSTarkov_Triton_M43-A",
            "JDSTarkov_Commando_Chest_Harness",
            "JDSTarkov_Direct_Action_Compact",
            "JDSTarkov_LBT-1961A_Load_Bearing",
            "DSTarkov_BlackRock",
            "JDSTarkov_Wartech_MK3_Rig",
            "JDSTarkov_ANA_Tactical",
            "JDSTarkov_Velocity_Systems",
            "JDSTarkov_Belt-A",
        };

        static readonly List<string> beltsToPatch = new List<string>()
        {
            "RE_AmmoCryoBelt",
            "RE_AmmoDisruptorBelt",
            "RE_AmmoExplosiveBelt",
            "RE_AmmoIncendiaryRounds",
            "RE_AmmoPiercingRounds",
            "RE_AmmoToxicBelt",
        };

        static readonly List<string> beltsToPatchAccessories = new List<string>()
        {
            "VAEA_Apparel_Quiver",
            "VAEA_Apparel_AmmoPack",
        };

        static RimmuNationPatcher()
        {
            if (ModLister.HasActiveModWithName("[RH2] Rimmu-Nation² - Clothing"))
            {
                if (LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().useTacticalLayer)
                {
                    foreach (string s in defsToPatchString)
                    {
                        ThingDef d = DefDatabase<ThingDef>.GetNamedSilentFail(s);
                        if (d != null)
                            d.apparel.layers = new List<ApparelLayerDef>() { UtilityDefOf.PacksAreNotBelts_Tactical };
                    }
                }
            }
            if (ModLister.HasActiveModWithName("[JDS] EFT Apparel"))
            {
                if (LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().useTacticalLayerEft)
                {
                    foreach (string s in defsToPatchEft)
                    {
                        ThingDef d = DefDatabase<ThingDef>.GetNamedSilentFail(s);
                        if (d != null)
                            d.apparel.layers = new List<ApparelLayerDef>() { UtilityDefOf.PacksAreNotBelts_Tactical };
                    }
                }
                if (LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().useTacticalLayerEftArmor)
                {
                    foreach (string s in defsToPatchEftArmor)
                    {
                        ThingDef d = DefDatabase<ThingDef>.GetNamedSilentFail(s);
                        if (d != null)
                            d.apparel.layers = new List<ApparelLayerDef>() { UtilityDefOf.PacksAreNotBelts_Tactical };
                    }
                }
            }
            if (ModLister.HasActiveModWithName("Rim-Effect: Core"))
            {
                if (LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().useAmmoLayer)
                {
                    foreach (string s in beltsToPatch)
                    {
                        ThingDef d = DefDatabase<ThingDef>.GetNamedSilentFail(s);
                        if (d != null)
                            d.apparel.layers = new List<ApparelLayerDef>() { UtilityDefOf.PacksAreNotBelts_Ammo };
                    }
                }
            }
            if (ModLister.HasActiveModWithName("Vanilla Apparel Expanded — Accessories"))
            {
                if (LoadedModManager.GetMod<UtilityPatchMod>().GetSettings<UtilityPatchSettings>().useAmmoLayerAccessories)
                {
                    foreach (string s in beltsToPatchAccessories)
                    {
                        ThingDef d = DefDatabase<ThingDef>.GetNamedSilentFail(s);
                        if (d != null)
                            d.apparel.layers = new List<ApparelLayerDef>() { UtilityDefOf.PacksAreNotBelts_Ammo };
                    }
                }
            }
        }
    }
}
