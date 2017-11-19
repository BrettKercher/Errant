using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation.Steps {
    class GenerateMoisture : GenerationStep {

        public GenerateMoisture(float inWeight) {
            name = "Generate Moisture";
            description = "Moist";
            weight = inWeight;
        }

        public override void Execute(GenerationData data, Random rng, BackgroundWorker worker = null) {
            int width = data.width;
            int height = data.height;
            int x, y;
            float dX, dY, m;
            float halfHeight = height * 0.5f;

            NoiseWrapper generator = new NoiseWrapper(
                FastNoise.NoiseType.SimplexFractal, FastNoise.FractalType.FBM, 9.5f, 0.9f, 8, rng.Next()
            );

            for (int i = 0; i < data.pointData.Length; i++) {
                x = i % width;
                y = i / width;
                dX = x / (float)width;
                dY = y / (float)height;

                m = generator.GetValue(dX, dY, 0, true);
                data.pointData[i].moisture = m;
            }
        }
    }
}
