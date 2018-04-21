
using System.Collections.Generic;

namespace Errant.Networking {
    public class SnapshotManager {

        private Dictionary<long, Snapshot> snapshots;
        
        public SnapshotManager() {
            snapshots = new Dictionary<long, Snapshot>();
        }
    }

    public class Snapshot {

        public Snapshot() {
            
        }
    }
}