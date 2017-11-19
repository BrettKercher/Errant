using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation {
    class BiomeGrower {

        List<int> frontier;
        BIOME biomeType;

        public BiomeGrower(int _centerIndex, BIOME _biomeType) {
            biomeType = _biomeType;
            frontier = new List<int>();
            frontier.Add(_centerIndex);
        }

        public bool isEmpty() {
            return frontier.Count == 0;
        }

        public void Grow(GenerationData data, Random rng) {
            if(isEmpty()) {
                return;
            }

            int width = data.width;
            int height = data.height;
            int nIndex;

            nIndex = rng.Next(0, frontier.Count);
            int nextIndex = frontier[nIndex];
            int endIndex = frontier[frontier.Count - 1];
            frontier[nIndex] = endIndex;
            frontier[frontier.Count - 1] = nextIndex;
            frontier.RemoveAt(frontier.Count - 1);

            int x = nextIndex % width;
            int y = nextIndex / width;

            nIndex = ((y + 0) * width) + (x + 1);
            if (nIndex >= 0 && nIndex < data.pointData.Length && data.pointData[nIndex].biome == BIOME.NONE) {
                data.pointData[nIndex].biome = biomeType;
                frontier.Add(nIndex);
            }

            nIndex = ((y + 0) * width) + (x - 1);
            if (nIndex >= 0 && nIndex < data.pointData.Length && data.pointData[nIndex].biome == BIOME.NONE) {
                data.pointData[nIndex].biome = biomeType;
                frontier.Add(nIndex);
            }

            nIndex = ((y + 1) * width) + (x + 0);
            if (nIndex >= 0 && nIndex < data.pointData.Length && data.pointData[nIndex].biome == BIOME.NONE) {
                data.pointData[nIndex].biome = biomeType;
                frontier.Add(nIndex);
            }

            nIndex = ((y - 1) * width) + (x + 0);
            if (nIndex >= 0 && nIndex < data.pointData.Length && data.pointData[nIndex].biome == BIOME.NONE) {
                data.pointData[nIndex].biome = biomeType;
                frontier.Add(nIndex);
            }
        }
    }
}
