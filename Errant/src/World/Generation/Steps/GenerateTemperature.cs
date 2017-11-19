using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation.Steps {
    class GenerateTemperature : GenerationStep {

        public GenerateTemperature(float inWeight) {
            name = "Generate Temperature";
            description = "It's gettin' hot in here";
            weight = inWeight;
        }

        public override void Execute(GenerationData data, Random rng, BackgroundWorker worker = null) {
            int width = data.width;
            int height = data.height;
            int x, y;
            float dX, dY, t, d;
            float halfHeight = height * 0.5f;

            NoiseWrapper generator = new NoiseWrapper(
                FastNoise.NoiseType.SimplexFractal, FastNoise.FractalType.FBM, 9.5f, 0.9f, 8, rng.Next()
            );

            for (int i = 0; i < data.pointData.Length; i++) {
                x = i % width;
                y = i / width;
                dX = x / (float)width;
                dY = y / (float)height;

                t = generator.GetValue(dX, dY, 0, true);
                data.pointData[i].elevation = t;

                d = 1 - Math.Abs(y / halfHeight - 1);
                t = (3 * d + data.pointData[i].elevation) / 4.0f;
                data.pointData[i].temperature = t;
            }
        }
    }
}
