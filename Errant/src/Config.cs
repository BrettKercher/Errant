using Errant.src.World;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

//This file should eventually be read from json/xml
namespace Errant.src {
	static class Config {

		//TODO: Read in from json/xml

		public static readonly Dictionary<WorldSize, Vector2> sizeMap = new Dictionary<WorldSize, Vector2>() {
			{   WorldSize.TINY, new Vector2(12, 8)      },
			{   WorldSize.SMALL, new Vector2(72, 48)    },
			{   WorldSize.MEDIUM, new Vector2(96, 64)   },
			{   WorldSize.LARGE, new Vector2(144, 96)   }
		};

		public static readonly int CHUNK_SIZE = 32;      // Size in tiles of a chunk
		public static readonly int TILE_SIZE = 32;       // Size in pixels of a tile

        public static readonly int GROUND_LAYER = 1;
        public static readonly int GROUND_TRANSITION_LAYER = 2;
        public static readonly int PLAYER_LAYER = 3;
	}
}
