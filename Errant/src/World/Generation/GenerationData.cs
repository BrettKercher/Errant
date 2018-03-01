using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation {

    public enum BIOME {
        NONE = 0,
        OCEAN,
        PLAINS,
        JUNGLE,
        SWAMP,
        SAVANNA,
        FOREST,
        ICE_PLAINS,
        TAIGA,
        ALPS,
        DESERT,
        MESA,
        VOLCANIC,
    }

    public struct GenerationData {
        public int width;
        public int height;
        public PointData[] pointData;
    }

    public struct PointData {
        public int x;
        public int y;
        public BIOME biome;
        public bool land;
        public bool mainland;
        public float temperature;
        public float moisture;
        public float elevation;

    }
}
