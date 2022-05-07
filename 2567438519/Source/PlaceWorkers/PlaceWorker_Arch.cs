using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Furnishing.PlaceWorkers {
    public class PlaceWorker_Arch : PlaceWorker {

		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null) {
			IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
			IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec
			}, ghostCol, null);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec2
			}, ghostCol, null);			
		}
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null) {

			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			if (center.Impassable(map) || c.Impassable(map) || c2.Impassable(map) ) {
				return Language.Translate.MustPlaceArchWithFreeSpaces;
			}
			return true;			
		}

	}
}
