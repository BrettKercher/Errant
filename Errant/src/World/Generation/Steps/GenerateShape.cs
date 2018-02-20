using Errant.src.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Errant.src.World.Generation.Steps {
    class GenerateShape : GenerationStep {
        readonly float sA = 0.83f;  //Pushes everything up 83
        readonly float sB = 1.75f;  //Pushes edges down
        readonly float sC = 0.90f;  //Controls drop off

        readonly float oceanThreshold = 0.075f; //Elevation Threshold for ocean

        public GenerateShape(float inWeight) {
            name = "Generate Shape";
            description = "Shapin' Things";
            weight = inWeight;
        }

        public override void Execute(GenerationData data, Random rng, BackgroundWorker worker = null) {
            int x, y;
            float dX, dY;
            float ddX, ddY;
            float e1, e2, d;
            float halfHeight = data.height >> 1;

            NoiseWrapper shapeGenerator1 = new NoiseWrapper(
                FastNoise.NoiseType.SimplexFractal, FastNoise.FractalType.Billow, 12f, 0.525f, 8, rng.Next() // 22.5f, 0.575f, 8
            );
            NoiseWrapper shapeGenerator2 = new NoiseWrapper(
                FastNoise.NoiseType.SimplexFractal, FastNoise.FractalType.FBM, 20.0f, .45f, 8, rng.Next() // 25.0f, 0.4f, 8, 
            );

            for (int i = 0; i < data.pointData.Length; i++) {
                x = i % data.width;
                y = i / data.width;

                dX = x / (float)data.width;
                dY = y / (float)data.height;

                data.pointData[i].x = x;
                data.pointData[i].y = y;

                // Calculate Shape
                e1 = 1 - Math.Abs(shapeGenerator1.GetValue(dX, dY, 0, true));

                e1 *= 0.1f;
                e1 = (e1 * 2) - 1;

                ddX = dX + e1;
                ddY = dY + e1;
                
                e1 = shapeGenerator2.GetValue(ddX, ddY, 0, true);

                d = MathEx.EuclideanDistanceCenter(x, y, data.width, data.height);
                e2 = e1 + sA - sB * (float)Math.Pow(d, sC);

                data.pointData[i].land = e2 > oceanThreshold;
                data.pointData[i].elevation = e2;

                //Report Progress back to Main thread
                if (worker != null) {
                    worker.ReportProgress((int)((i / (float)data.pointData.Length) * 100));
                }
            }
            FillIslands(data);
            FillLakes(data);
        }

        /// <summary>
        /// Use BFS flood fill to identify the main island, and fill in any inaccessible land
        /// </summary>
        /// <param name="data">An object wrapper for information about each tile</param>
        void FillIslands(GenerationData data) {
            PointData[] climateData = data.pointData;
            int width = data.width;
            int height = data.height;
            int halfIndex = (height / 2 * width) + (width / 2);
            int nIndex;

            PointData c;
            if (climateData[halfIndex].land) {
                Queue<PointData> q = new Queue<PointData>(climateData.Length);
                climateData[halfIndex].mainland = true;
                q.Enqueue(climateData[halfIndex]);
                while (q.Count > 0) {
                    c = q.Dequeue();

                    nIndex = ((c.y + 0) * width) + (c.x + 1);
                    if (nIndex >= 0 && nIndex < climateData.Length && climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }

                    nIndex = ((c.y + 0) * width) + (c.x - 1);
                    if (nIndex >= 0 && nIndex < climateData.Length && climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }

                    nIndex = ((c.y + 1) * width) + (c.x + 0);
                    if (nIndex >= 0 && nIndex < climateData.Length && climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }

                    nIndex = ((c.y - 1) * width) + (c.x + 0);
                    if (nIndex >= 0 && nIndex < climateData.Length && climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }
                }

                for (int i = 0; i < climateData.Length; i++) {
                    climateData[i].land = climateData[i].mainland;
                    climateData[i].mainland = false;
                }
            }
            else {
                Debug.WriteLine("Assumed the center would be land, and it wasn't.");
            }
        }

        /// <summary>
        /// Use BFS flood fill to identify the main ocean, and fill in any inaccessible water
        /// </summary>
        /// <param name="data">An object wrapper for information about each tile</param>
        void FillLakes(GenerationData data) {
            PointData[] climateData = data.pointData;
            int width = data.width;
            int height = data.height;
            int nIndex;

            PointData c;
            if (!climateData[0].land) {
                Queue<PointData> q = new Queue<PointData>(climateData.Length);
                climateData[0].mainland = true;
                q.Enqueue(climateData[0]);
                while (q.Count > 0) {
                    c = q.Dequeue();

                    nIndex = ((c.y + 0) * width) + (c.x + 1);
                    if (nIndex >= 0 && nIndex < climateData.Length && !climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }

                    nIndex = ((c.y + 0) * width) + (c.x - 1);
                    if (nIndex >= 0 && nIndex < climateData.Length && !climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }

                    nIndex = ((c.y + 1) * width) + (c.x + 0);
                    if (nIndex >= 0 && nIndex < climateData.Length && !climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }

                    nIndex = ((c.y - 1) * width) + (c.x + 0);
                    if (nIndex >= 0 && nIndex < climateData.Length && !climateData[nIndex].land && !climateData[nIndex].mainland) {
                        climateData[nIndex].mainland = true;
                        q.Enqueue(climateData[nIndex]);
                    }
                }

                for (int i = 0; i < climateData.Length; i++) {
                    climateData[i].land = !climateData[i].mainland;
                    climateData[i].mainland = false;
                }
            }
        }

    }
}
