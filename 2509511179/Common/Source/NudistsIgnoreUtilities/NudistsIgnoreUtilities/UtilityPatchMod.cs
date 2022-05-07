using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace UtilityPatch
{
    public class UtilityPatchMod : Mod
    {
        public UtilityPatchSettings settings;

        public UtilityPatchMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<UtilityPatchSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Nudists ignore utilities", ref settings.nudistsIgnoreUtility, "By default, anything worn on the legs or torso will upset nudists. This setting allows them to wear all utilities without consequence.");
            listingStandard.CheckboxLabeled("Use tactical layer for Rimmu-Nation 2", ref settings.useTacticalLayer, "(REQUIRES RESTART) If enabled, all webbings and belts from Rimmu-nation 2 will be moved to a special \"tactical\" layer. This will make them wearable with other types of equipment.");
            listingStandard.CheckboxLabeled("Use tactical layer for [JDS] EFT Apparel (non-armor)", ref settings.useTacticalLayerEft, "(REQUIRES RESTART) If enabled, all non-armor chest-rigs from [JDS] EFT Apparel will be moved to a special \"tactical\" layer. This will make them wearable with other types of equipment.");
            listingStandard.CheckboxLabeled("Use tactical layer for [JDS] EFT Apparel (armor)", ref settings.useTacticalLayerEftArmor, "(REQUIRES RESTART) If enabled, all armored chest-rigs from [JDS] EFT Apparel will be moved to a special \"tactical\" layer. This will make them wearable with other types of equipment.");
            listingStandard.CheckboxLabeled("Use ammunition layer for Rim-Effect ammo belts", ref settings.useAmmoLayer, "(REQUIRES RESTART) If enabled, all ammo belts from Rim-Effect: Core will be moved to their own layer. This will make them wearable with all other items.");
            listingStandard.CheckboxLabeled("Use ammunition layer for VAE Accessories", ref settings.useAmmoLayerAccessories, "(REQUIRES RESTART) If enabled, the ammo pack and quiver will be moved to the \"Ammuntion\" layer."); ;
            listingStandard.CheckboxLabeled("Equipment ignores damage", ref settings.utilityIgnoresDamage, "If enabled, apparel on the equipment layers will not take damage in combat.");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Packs Are Not Belts";
        }
    }
}
