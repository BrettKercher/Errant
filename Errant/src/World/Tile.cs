using Errant.src.Graphics;
using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Errant.src.World {

    class Tile {

		private static Dictionary<BIOME, int> biomeAtlasRowMap = new Dictionary<BIOME, int>() {
			{ BIOME.NONE, 0 },
			{ BIOME.ONE, 1 },
			{ BIOME.TWO, 2 },
			{ BIOME.THREE, 3 },
			{ BIOME.FOUR, 4 },
			{ BIOME.FIVE, 5 },
			{ BIOME.SIX, 6 },
			{ BIOME.SEVEN, 7 },
			{ BIOME.EIGHT, 8 },
			{ BIOME.NINE, 9 },
			{ BIOME.TEN, 10 },
			{ BIOME.ELEVEN, 11 },
			{ BIOME.TWELVE, 12 },
		};

		public int TextureRegionRow { get; private set; }

        public Tile(PointData pointData) {

            TextureRegionRow = biomeAtlasRowMap[pointData.biome];
// 
// 			if (pointData.land) {
//                 TextureRegionIndex = 29;
// 			}
// 			else {
//                 TextureRegionIndex = 14;
//             }
		}
	}
}
