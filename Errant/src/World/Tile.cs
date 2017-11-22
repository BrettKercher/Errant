using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Errant.src.World {

    class Tile {

		private static Dictionary<BIOME, Color> biomeColorMap = new Dictionary<BIOME, Color>() {
			{ BIOME.NONE, Color.Blue },
			{ BIOME.ONE, Color.White },
			{ BIOME.TWO, Color.Gray },
			{ BIOME.THREE, Color.Cyan },
			{ BIOME.FOUR, Color.Green },
			{ BIOME.FIVE, Color.SaddleBrown },
			{ BIOME.SIX, Color.ForestGreen },
			{ BIOME.SEVEN, Color.DarkOliveGreen },
			{ BIOME.EIGHT, Color.SpringGreen },
			{ BIOME.NINE, Color.LawnGreen },
			{ BIOME.TEN, Color.Yellow },
			{ BIOME.ELEVEN, Color.Red },
			{ BIOME.TWELVE, Color.OrangeRed },
		};

		Texture2D texture;
		public Color color;

        public Tile(PointData pointData) {
			color = pointData.land ? Color.White : Color.Black;

			if (pointData.land) {
				color = biomeColorMap[pointData.biome];
			}
			else {
				color = Color.DarkBlue;
			}
		}
	}
}
