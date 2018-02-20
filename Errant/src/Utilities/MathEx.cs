using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.Utilities {
    static class MathEx {

        //Distance from center of map
        public static float EuclideanDistanceCenter(int x, int y, int width, int height) {
            float nX, nY;
            nX = (x / (float)width) - 0.5f;
            nY = (y / (float)height) - 0.5f;

            return 2 * (float)Math.Sqrt(nX * nX + nY * nY);
        }

    }
}
