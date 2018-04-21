using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World
{
	public class Chunk
	{
		private PersistentTile[] tileData;
		private ActiveTile[] tiles;

		public Chunk()
		{
			tiles = new ActiveTile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];
		}

		public ActiveTile[] GetTiles()
		{
			return tiles;
		}
	}
}
