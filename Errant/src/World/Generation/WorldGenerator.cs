using Errant.src.World.Generation.Steps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Errant.src.World.Generation {

	class WorldGenerator {

		List<GenerationStep> steps = new List<GenerationStep>();

		public WorldGenerator() {
			steps.Add(new GenerateShape(0.0f));
			steps.Add(new GenerateTemperature(0.0f));
			steps.Add(new GenerateMoisture(0.0f));
			steps.Add(new GenerateBiomes(0.0f));
		}

		public GenerationData Generate(GenerationSettings settings, BackgroundWorker worker = null) {
			Random rng = new Random(); //settings.seed

			GenerationData data = new GenerationData();
			data.width = (int)Config.sizeMap[settings.size].X * Config.CHUNK_SIZE;
			data.height = (int)Config.sizeMap[settings.size].Y * Config.CHUNK_SIZE;
			data.pointData = new PointData[data.width * data.height];

			for (int i = 0; i < data.pointData.Length; i++) {
				data.pointData[i] = new PointData();
			}

			Stopwatch timer = new Stopwatch();
			foreach (GenerationStep step in steps) {
				timer.Start();
				step.Execute(data, rng, worker);
				Debug.WriteLine("[GENERATION] " + step.GetName() + " Ran In: " + FormatTime(timer.Elapsed));
				timer.Reset();
			}
			
			//determine spawn location
			int equator = data.height / 2;
			int startingY = equator * data.width;
			int firstLand = 0;
			while (!data.pointData[startingY + firstLand].land) {
				firstLand++;
			}
			
			data.spawnArea = new Rectangle(firstLand + 10, equator - 5, Config.SPAWN_AREA_SIZE, Config.SPAWN_AREA_SIZE);

			return data;
		}

		private string FormatTime(TimeSpan time) {
			return String.Format("{0:00}hr {1:00}min {2:00}.{3:00}sec",
				time.Hours, time.Minutes, time.Seconds,
				time.Milliseconds / 10);
		}

	}
}
