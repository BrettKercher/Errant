using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errant.src.World.Generation.Steps {
    abstract class GenerationStep {

        protected string description = "";  // Displayed in world gen loading screen
        protected string name = "unnamed";  // Used in logging
        protected float weight = 0;         // How much to move the percent by

        public string GetName() {
            return name;
        }

        public abstract void Execute(GenerationData data, Random rng, BackgroundWorker worker = null);

    }
}
