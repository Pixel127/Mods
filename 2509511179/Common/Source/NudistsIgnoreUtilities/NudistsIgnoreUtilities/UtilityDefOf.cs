using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace UtilityPatch
{
    [DefOf]
    public static class UtilityDefOf
    {
        public static ApparelLayerDef PacksAreNotBelts_Tactical;
        public static ApparelLayerDef PacksAreNotBelts_Ammo;

        static UtilityDefOf() =>
            DefOfHelper.EnsureInitializedInCtor(typeof(UtilityDefOf));
    }
}
