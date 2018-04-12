using System;
using Errant.Networking;
using Errant.src.Networking;
using Lidgren.Network;

namespace Errant.src {
    public class NetworkManager {
        private Application application;
        private bool isHost;
        
        private Peer peer;
        
        public NetworkManager(Application _application, bool _isHost) {
            application = _application;
            isHost = _isHost;
        }

        public void Initialize(string address) {
            if (isHost) {
                peer = new Server(application);
            }
            else {
                peer = new Client(application, address);
            }
            
            peer.Start();
            peer.ReceiveMessages();
        }

        public void Shutdown() {
            peer?.Shutdown();
        }
    }

    public struct NetworkSettings {
        public bool isHost;
        public string address;
    }
}