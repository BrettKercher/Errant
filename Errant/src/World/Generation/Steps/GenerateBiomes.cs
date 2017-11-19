using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation.Steps {
    class GenerateBiomes : GenerationStep {

        private readonly int BIOME_DENSITY = 16;
        private List<BiomeGrower> fractals;

        public GenerateBiomes(float inWeight) {
            name = "Generate Biomes";
            description = "Makin' some Biomes";
            weight = inWeight;
        }

        public override void Execute(GenerationData data, Random rng, BackgroundWorker worker = null) {
            InitializeCentralPoints(data, rng, worker);
        }

        private void InitializeCentralPoints(GenerationData data, Random rng, BackgroundWorker worker = null) {
            
            int x, y, biome;
            int xOffset, yOffset;
            int randomIndex;
            BiomeGrower grower;

            int width = data.width;
            int height = data.height;
            int xStep = width / BIOME_DENSITY;
            int yStep = height / BIOME_DENSITY;

            int minXOffset = (int)Math.Ceiling(-xStep * 0.4f);
            int maxXOffset = (int)Math.Floor(xStep * 0.4f);
            int minYOffset = (int)Math.Ceiling(-yStep * 0.4f);
            int maxYOffset = (int)Math.Floor(xStep * 0.4f);

            fractals = new List<BiomeGrower>((BIOME_DENSITY - 1) * (BIOME_DENSITY - 1));

            for (int i = 1; i < BIOME_DENSITY; i++) {
                y = (i * yStep);
                for (int j = 1; j < BIOME_DENSITY; j++) {
                    x = (j * xStep);
                    xOffset = rng.Next(minXOffset, maxXOffset);
                    yOffset = rng.Next(minYOffset, maxYOffset);

                    biome = rng.Next(1, 6);
                    grower = new BiomeGrower(((y + yOffset) * width) + (x + xOffset), (BIOME)(biome));
                    grower.Grow(data, rng);
                    fractals.Add(grower);
                }
            }
            
            while(IsUnfinishedFractal()) {
                randomIndex = rng.Next(0, fractals.Count);
                fractals[randomIndex].Grow(data, rng);
            }
        }


        private bool IsUnfinishedFractal() {
            foreach(BiomeGrower grower in fractals) {
                if (!grower.isEmpty()) {
                    return true;
                }
            }
            return false;
        }
    }
}