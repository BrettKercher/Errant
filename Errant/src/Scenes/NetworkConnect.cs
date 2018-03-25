namespace Errant.src.Scenes {
    public class NetworkConnect : Scene {

        public NetworkConnect(Application _application, NetworkSettings netSettings): base(_application) {
            if (netSettings.connect) {
                _application.InitializeNetworkManager(netSettings.isHost, netSettings.address);
            }
        }
    }
}