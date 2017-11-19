using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World {

    struct TileData {
        ushort groundId;
        ushort objectId;
    }

    class Tile {

        TileData data;
        Texture2D texture;

        public Tile(TileData _data) {
            data = _data;
        }
    }
}
