
namespace Errant.src.World {

    public enum WorldSize {
        TINY = 0,
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3
    }

    class GenerationSettings {
        public WorldSize size;
        public int seed;
    }
}
