using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;

namespace PassiveCover
{
    public class PassiveCoverSettings : ModSettings
    {
        public float passiveCoverEffectiveness = 50f;

        public bool allowObstuctionSecurity = true;
        public bool allowObstructionNatural = true;
        public bool allowObstructionOther = true;

        public float negatePassiveCoverRange = 2.0f;
        public bool useBestObstructionOnly = false;
        public bool onlyPassiveIfNoActive = false;

        public bool onlyUseWhenShooting = false;
        public float coverEffectivenessCap = 100f; // Can't be better than this amount

        public override void ExposeData()
        {
            Scribe_Values.Look(ref passiveCoverEffectiveness, "passiveCoverEffectiveness", 50f);

            Scribe_Values.Look(ref allowObstuctionSecurity, "allowObstuctionSecurity", true);
            Scribe_Values.Look(ref allowObstructionNatural, "allowObstructionNatural", true);
            Scribe_Values.Look(ref allowObstructionOther, "allowObstructionOther", true);

            Scribe_Values.Look(ref negatePassiveCoverRange, "negatePassiveCoverRange", 2.0f);
            Scribe_Values.Look(ref useBestObstructionOnly, "useBestObstructionOnly", false);
            Scribe_Values.Look(ref onlyPassiveIfNoActive, "onlyPassiveIfNoActive", false);

            Scribe_Values.Look(ref onlyUseWhenShooting, "onlyUseWhenShooting", false);
            Scribe_Values.Look(ref coverEffectivenessCap, "coverEffectivenessCap", 100f);

            base.ExposeData();
        }
    }

    public class PassiveCover : Mod
    {
        PassiveCoverSettings settings;
        public PassiveCover(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<PassiveCoverSettings>();
        }

        Vector2 scrollPosition = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Please don't take this as an example of how to do scroll bars
            // I just randomly copied + modified bits from other mods until eventually something worked
            Rect outRect = new Rect(inRect.x, inRect.y + 40f, inRect.width, inRect.height - 40f);
            Rect viewRect = new Rect(0, 0, outRect.width - 20f, 1500f);
            
            var listing_options = new Listing_Standard();
            // I still don't understand buffers...
            string bufferA = null;
            string bufferB = null;
            string bufferC = null;

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

            listing_options.Begin(viewRect);

            listing_options.Label("PassiveCover_OptionEffectivenessTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionEffectivenessDesc".Translate(), -1f);
            listing_options.TextFieldNumericLabeled("PassiveCover_OptionEffectiveness".Translate(), ref settings.passiveCoverEffectiveness, ref bufferA, 0f, 10000f);

            listing_options.Label("PassiveCover_OptionUseBestOnlyTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionUseBestOnlyDesc".Translate(), -1f);
            listing_options.CheckboxLabeled("PassiveCover_OptionUseBestOnly".Translate(), ref settings.useBestObstructionOnly);

            listing_options.Label("PassiveCover_OptionNegateIfActiveTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionNegateIfActiveDesc".Translate(), -1f);
            listing_options.CheckboxLabeled("PassiveCover_OptionNegateIfActive".Translate(), ref settings.onlyPassiveIfNoActive);

            listing_options.Label("PassiveCover_OptionNegateRangeTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionNegateRangeDesc".Translate(), -1f);
            listing_options.TextFieldNumericLabeled("PassiveCover_OptionNegateRange".Translate(), ref settings.negatePassiveCoverRange, ref bufferB, 0f, 10000f);

            listing_options.Label("PassiveCover_OptionCategoryAllowedTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionCategoryAllowedDesc".Translate(), -1f);
            listing_options.CheckboxLabeled("PassiveCover_OptionAllowSecurity".Translate(), ref settings.allowObstuctionSecurity);
            listing_options.CheckboxLabeled("PassiveCover_OptionAllowNatural".Translate(), ref settings.allowObstructionNatural);
            listing_options.CheckboxLabeled("PassiveCover_OptionAllowOther".Translate(), ref settings.allowObstructionOther);

            listing_options.Label("PassiveCover_OptionPerformanceModeTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionPerformanceModeDesc".Translate(), -1f);
            listing_options.CheckboxLabeled("PassiveCover_OptionPerformanceMode".Translate(), ref settings.onlyUseWhenShooting);

            listing_options.Label("PassiveCover_OptionCoverCapTitle".Translate(), -1f);
            listing_options.Label("PassiveCover_OptionCoverCapDesc".Translate(), -1f);
            listing_options.TextFieldNumericLabeled("PassiveCover_OptionCoverCap".Translate(), ref settings.coverEffectivenessCap, ref bufferC, 0f, 100f);

            listing_options.End();
            Widgets.EndScrollView();

            viewRect.height = listing_options.CurHeight; // TODO: This isn't limiting the height properly

            base.DoSettingsWindowContents(viewRect);
        }

        public override string SettingsCategory()
        {
            return "PassiveCover_ModName".Translate();
        }
    }
}
