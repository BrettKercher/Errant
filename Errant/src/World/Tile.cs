using Errant.src.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World {

    class Tile {
		
        Texture2D texture;
		public Color color;

        public Tile(PointData pointData) {
			color = pointData.land ? Color.White : Color.Black;
        }
    }
}
