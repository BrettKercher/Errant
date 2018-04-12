using System;
using Errant.Networking;
using Errant.src.World;
using Lidgren.Network;

namespace Errant.src.Networking {
    public class Client : Peer {

        public delegate void TestHandler(WorldHeader header);
        public static event TestHandler WorldDetailsReceived = delegate { };
        
        private string address;

        public Client(Application _application, string _address): base(_application) {
            address = _address;
        }
        
        public override void Start() {
            
            var config = new NetPeerConfiguration("errant");
        
            peer = new NetClient(config);
            peer.Start();
            System.Diagnostics.Debug.WriteLine("Attempting to connect to: " + address);
            peer.Connect(address, PORT);
        }
        
        public override void ReceiveMessages() {
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
                        ProcessDataMessage(msg);
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
        
        private void ProcessDataMessage(NetIncomingMessage msg) {
            byte msgType = msg.ReadByte();

            switch (msgType) {
                case 0:
                    // World Details
                    // string = world name
                    // string = version number
                    // int = width
                    // int = height

                    WorldHeader header;
                    header.name = msg.ReadString();
                    header.versionNumber = msg.ReadString();
                    header.width = msg.ReadInt32();
                    header.height = msg.ReadInt32();

                    WorldDetailsReceived(header);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ProcessStatusChange(NetIncomingMessage msg) {
            NetConnectionStatus status = msg.SenderConnection.Status;
            switch (status) {
                case NetConnectionStatus.Connected:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Connected!");
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Initiated Connect!");
                    break;
                case NetConnectionStatus.Disconnected:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Disconnected!");
                    break;
                case NetConnectionStatus.None:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: None!");
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Received Invitation!");
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Respond Awaiting Approval!");
                    break;
                case NetConnectionStatus.RespondedConnect:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Respond Connect!");
                    break;
                case NetConnectionStatus.Disconnecting:
                    System.Diagnostics.Debug.WriteLine("[CLIENT] Received Status Change: Disconnecting!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}