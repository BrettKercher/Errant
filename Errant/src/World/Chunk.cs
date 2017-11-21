using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World
{
	class Chunk
	{
		Tile[] tiles;

		public Chunk()
		{
			tiles = new Tile[Config.CHUNK_SIZE * Config.CHUNK_SIZE];
		}

		public Tile[] GetTiles()
		{
			return tiles;
		}
	}
}
