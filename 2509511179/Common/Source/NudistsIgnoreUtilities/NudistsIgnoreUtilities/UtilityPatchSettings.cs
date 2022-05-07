using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UtilityPatch
{
    public class UtilityPatchSettings : ModSettings
    {
        public bool nudistsIgnoreUtility;
        public bool useTacticalLayer = false;
        public bool useTacticalLayerEft = false;
        public bool useTacticalLayerEftArmor = false;
        public bool useAmmoLayer = false;
        public bool useAmmoLayerAccessories = false;
        public bool utilityIgnoresDamage = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref nudistsIgnoreUtility, "nudistsIgnoreUtility");
            Scribe_Values.Look(ref useTacticalLayer, "useTacticalLayer");
            Scribe_Values.Look(ref useTacticalLayerEft, "useTacticalLayerEft");
            Scribe_Values.Look(ref useTacticalLayerEftArmor, "useTacticalLayerEftArmor");
            Scribe_Values.Look(ref useAmmoLayer, "useAmmoLayer");
            Scribe_Values.Look(ref useAmmoLayerAccessories, "useAmmoLayerAccessories");
            Scribe_Values.Look(ref utilityIgnoresDamage, "utilityIgnoresDamage");
            base.ExposeData();
        }
    }
}
