namespace Errant.src.World.Generation {
    class NoiseWrapper {

        FastNoise generator = new FastNoise();

        public NoiseWrapper(FastNoise.NoiseType noiseType, FastNoise.FractalType fractalType, float frequency, float persistence, int octaves, int seed) {
            generator = new FastNoise(seed);
            generator.SetNoiseType(noiseType);
            generator.SetFractalType(fractalType);
            generator.SetFrequency(frequency);
            generator.SetFractalGain(persistence);
            generator.SetFractalOctaves(octaves);
            generator.SetFractalLacunarity(2.0f);
        }

        /// <summary>
        /// Gets a random value between -1 and 1, or 0 and 1 if scaled
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="scaled">Wheter the return value should be scaled to 0 - 1</param>
        public float GetValue(float x, float y, float z, bool scaled = false) {
            float noise = generator.GetValueFractal(x, y, z);
            return scaled ? (noise + 1) * 0.5f : noise;

        }

    }
}
