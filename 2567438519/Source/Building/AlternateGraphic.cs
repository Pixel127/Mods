using RimWorld;
using Verse;

namespace Furnishing.Building {
    public class AlternateGraphic : Verse.Building {
        private Graphic cachedGraphicFull;

		public override Graphic Graphic {
			get {
				CompRefuelable compRefuelable = this.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && !compRefuelable.HasFuel) {
					return base.Graphic;
				}
				if (!FlickUtility.WantsToBeOn(this)) {
					return base.Graphic;
				}
				CompPowerTrader compPowerTrader = this.TryGetComp<CompPowerTrader>();
				if (compPowerTrader != null && !compPowerTrader.PowerOn) {
					return base.Graphic;
				}
				if (def.building.fullGraveGraphicData == null) {
					return base.Graphic;
				}
				if (cachedGraphicFull == null) {
					cachedGraphicFull = def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return cachedGraphicFull;
			}
		}
	}
}
