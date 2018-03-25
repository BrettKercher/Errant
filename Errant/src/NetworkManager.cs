using System;
using System.Threading;
using Lidgren.Network;

namespace Errant.src {
    public class NetworkManager {
        private Application application;
        private NetPeer peer;
        private Timer updateLoop;

        private const int SnapshotSendRate = 50;
        private const int PORT = 7777;
        
        public NetworkManager(Application _application) {
            application = _application;
        }

        public void Initialize(bool isHost, string address) {
            if (isHost) {
                StartServer();
                updateLoop = new Timer(SendWorldSnapshot, null, 0, SnapshotSendRate);
            }
            else {
                ConnectToServer(address);
            }

            ReceiveMessages();
        }

        private void ConnectToServer(string address) {
            var config = new NetPeerConfiguration("errant");
            peer = new NetClient(config);
            peer.Start();
            peer.Connect(address, 7777);
        }

        private void StartServer() {
            var config = new NetPeerConfiguration("errant") {
                Port = PORT
            };
            
            System.Diagnostics.Debug.WriteLine("Starting server at 127.0.0.1:" + PORT);
            peer = new NetServer(config);
            peer.Start();
        }

        private void SendWorldSnapshot(object obj) {
        }

        private void ReceiveMessages() {
            NetIncomingMessage msg;
            while (true) {
                
                // Block until message received
                peer.MessageReceivedEvent.WaitOne();
                msg = peer.ReadMessage();
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        System.Diagnostics.Debug.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        ProcessStatusChange(msg);
                        break;
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        break;
                    case NetIncomingMessageType.Data:
                        
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                peer.Recycle(msg);
            }
        }

        private void ProcessStatusChange(NetIncomingMessage msg) {
            NetConnectionStatus status = msg.SenderConnection.Status;
            switch (status) {
                case NetConnectionStatus.Connected:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Connected!");
                    // begin sending client map data
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Initiated Connect!");
                    break;
                case NetConnectionStatus.Disconnected:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Disconnected!");
                    break;
                case NetConnectionStatus.None:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: None!");
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Received Invitation!");
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Respond Awaiting Approval!");
                    break;
                case NetConnectionStatus.RespondedConnect:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Respond Connect!");
                    break;
                case NetConnectionStatus.Disconnecting:
                    System.Diagnostics.Debug.WriteLine("[NET] Received Status Change: Disconnecting!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public struct NetworkSettings {
        public bool connect;
        public bool isHost;
        public string address;
    }
}