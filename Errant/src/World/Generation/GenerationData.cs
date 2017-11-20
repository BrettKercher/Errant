using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation {

    public enum BIOME {
        NONE = 0,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        ELEVEN,
        TWELVE,
    }

    struct GenerationData {
        public int width;
        public int height;
        public PointData[] pointData;
    }

    struct PointData {
        public int x;
        public int y;
        public BIOME biome;
        public bool land;
        public bool mainland;
        public float temperature;
        public float moisture;
        public float elevation;

        public float debugValue1;
        public float debugValue2;

    }
}
