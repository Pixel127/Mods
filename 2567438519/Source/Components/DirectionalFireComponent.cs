using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Furnishing.Components {
    [StaticConstructorOnStartup]
    public class DirectionalFireComponent : CompFireOverlay {
        public static Graphic overlayGraphic;
        public override void PostDraw() {
            if (((CompProps_DirectionalFire)Props).rotationsAccepted.Contains(parent.Rotation))
                base.PostDraw();
        }
        public void DrawCall(bool invert) {
            Vector3 drawPos = parent.DrawPos;
            drawPos.y += 0.046875f;
            if(((CompProps_DirectionalFire)Props).directionalOffset != 0)
            switch (parent.Rotation.AsInt) {
                case 0:
                    drawPos.y += ((CompProps_DirectionalFire)Props).directionalOffset;
                    break;
                case 1:
                    drawPos.x += ((CompProps_DirectionalFire)Props).directionalOffset;
                    break;
                case 2:
                    drawPos.y -= ((CompProps_DirectionalFire)Props).directionalOffset;
                    break;
                case 3:
                    drawPos.x -= ((CompProps_DirectionalFire)Props).directionalOffset;
                    break;
                default:
                    break;
            }
            FireGraphic.Draw(drawPos, Rot4.North, parent, 0f);
        }
    }
    public class CompProps_DirectionalFire : CompProperties_FireOverlay {
        public List<Rot4> rotationsAccepted = new List<Rot4>();
        public string overlay;
        public float directionalOffset = 0;
        public CompProps_DirectionalFire() {
            compClass = typeof(DirectionalFireComponent);
        }
        public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null) {
            return;
        }
    }
}