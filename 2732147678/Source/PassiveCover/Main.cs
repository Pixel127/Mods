using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using System.Reflection;

namespace PassiveCover
{
    
    public enum CoverCategory
    {
		Security,
		Natural,
		Wall, // ?
		Other
    }

	public static class Main
    {
        public static bool executeThisTime = false; // Hack for harmony patches.
        
        public static CoverCategory GetCoverCategory(Thing thing)
		{
			if (thing.def.category == ThingCategory.Building && thing.def.designationCategory == DesignationCategoryDefOf.Security)
			{
                return CoverCategory.Security;
            } else if (thing.HasThingCategory(ThingCategoryDef.Named("Chunks")) || thing.HasThingCategory(ThingCategoryDef.Named("StoneChunks")) || thing.HasThingCategory(ThingCategoryDef.Named("Plants")))
			{
				return CoverCategory.Natural;
			} else
            {
				return CoverCategory.Other;
            }
        }

		public static List<CoverInfo> GetAdjustedPassiveCover(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
        {
            // Grab mod settings
            bool option_useBestOnly = LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().useBestObstructionOnly;
            float option_negationRange = LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().negatePassiveCoverRange;
            float option_passiveEffectiveness = LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().passiveCoverEffectiveness;

            bool option_categorySecurity = LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().allowObstuctionSecurity;
            bool option_categoryNatural = LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().allowObstructionNatural;
            bool option_categoryOther = LoadedModManager.GetMod<PassiveCover>().GetSettings<PassiveCoverSettings>().allowObstructionOther;

            // The default function will have accounted for all cells adjacent to the target for active cover
            // Therefore, we want to avoid doing anything with them
            List<IntVec3> forbiddenCells = new List<IntVec3>();
            forbiddenCells.Add(target.Cell);
            for (int i = 0; i < 8; i++)
            {
                IntVec3 coverCell = target.Cell + GenAdj.AdjacentCells[i];
                forbiddenCells.Add(coverCell);
            }

            List<CoverInfo> passiveCoverList = new List<CoverInfo>();

            // TODO ?
            // Due to circumstances we have to (and can afford to) be fairly naiive when getting the path the shot will take
            // The game's usual methods for working this out use information like verbs + pawns that we don't have here, so we can't really use them
            // Fortunately, we don't have to be checking if the shot can actually hit because the game is already doing that for us
            // All this ultimately means is we can't account for shooters leaning before shooting, meaning that what's considered in the way of the shot by us will be slightly off in some cases
            ShootLine shotPath = new ShootLine(shooterLoc, target.Cell);

            // Have to use some reflection here because the method the game uses for calculating adjusted cover is private...
            MethodInfo TryFindAdjustedCoverInCell = typeof(CoverUtility).GetMethod("TryFindAdjustedCoverInCell", BindingFlags.NonPublic | BindingFlags.Static);

            foreach (IntVec3 item in shotPath.Points())
            {
                if (!forbiddenCells.Contains(item) && item.InBounds(map) && (shooterLoc - item).LengthHorizontal >= option_negationRange) // (I'm not great at vector stuff but I think that's how you get distance based on how its used in the game's code)
                {
                    // Reflection part 2
                    var parameters = new object[] { shooterLoc, target, item, map, null };
                    bool result = (bool)TryFindAdjustedCoverInCell.Invoke(null, parameters);
                    
                    if (result)
                    {
                        CoverInfo cover = (CoverInfo)parameters[4];
                        if (cover.Thing != null)
                        {
                            // Only consider this cover if its type has been configured to allow it
                            var category = GetCoverCategory(cover.Thing);

                            if (category == CoverCategory.Security)
                            {
                                if (option_categorySecurity == false) continue;
                            }
                            else if (category == CoverCategory.Natural)
                            {
                                if (option_categoryNatural == false) continue;
                            }
                            else
                            {
                                if (option_categoryOther == false) continue;
                            }

                            // Adjust the cover effectiveness to account for it being used passively
                            // (can't acutally modify the block chance of an existing CoverInfo, so we have to replace it with a new one)
                            cover = new CoverInfo(cover.Thing, cover.BlockChance * (option_passiveEffectiveness / 100));

                            if (option_useBestOnly) // Only the best passive cover should be factored in
                            {
                                // Work out if this is the new best passive cover
                                if (!passiveCoverList.Any() || cover.BlockChance > passiveCoverList[0].BlockChance)
                                {
                                    // Remove any previous entry we might have stored before adding this one
                                    passiveCoverList.Clear();

                                    passiveCoverList.Add(cover);

                                }
                            }
                            else // All passive covers should be factored in
                            {
                                passiveCoverList.Add(cover);
                            }
                        }
                    }
                }
            }

            return passiveCoverList;
        }
	}
}
