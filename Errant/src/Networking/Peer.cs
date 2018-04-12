using Lidgren.Network;

namespace Errant.Networking {
    public abstract class Peer {

        protected const int PORT = 7777;
        protected NetPeer peer;
        protected Application application;
        
        public abstract void Start();
        public abstract void ReceiveMessages();

        public void Shutdown() {
            peer?.Shutdown("Application Exit");
        }

        protected Peer(Application _application) {
            application = _application;
        }
    }
}