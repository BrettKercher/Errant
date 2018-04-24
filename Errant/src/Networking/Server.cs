using System;
using System.Collections.Generic;
using Errant.Networking;
using Lidgren.Network;
using System.Threading;
using Errant.src.World;

namespace Errant.src.Networking {
    public class Server : Peer {
        
        private Timer updateLoop;
        private const int SnapshotSendRate = 50;
        private ulong snapshotCount = 0;

        private Dictionary<NetConnection, ulong> baselineSnapshotMap;

        public Server(Application _application) : base(_application) {
            baselineSnapshotMap = new Dictionary<NetConnection, ulong>();
        }

        public override void Start() {
            
            var config = new NetPeerConfiguration("errant") {
                Port = PORT
            };
            
            System.Diagnostics.Debug.WriteLine("Starting server at 127.0.0.1:" + PORT);
            peer = new NetServer(config);
            peer.Start();
            
            updateLoop = new Timer(SendWorldSnapshot, null, 0, SnapshotSendRate);
        }
        
        private void SendWorldSnapshot(object obj) {
            // generate snapshot of everything that has changed from the previous frame
            //send relevant sections of the snapshot to each connection
            Snapshot snap = application.TakeSnapshot(snapshotCount);
            snapshotCount++;

            NetOutgoingMessage msg = CreateMessageFromSnapshot(snap);
            
            foreach (NetConnection conn in peer.Connections) {

                peer.SendMessage(msg, conn, NetDeliveryMethod.ReliableOrdered);
                
                ulong lastSnapshot;
                if (baselineSnapshotMap.TryGetValue(conn, out lastSnapshot)) {
                    if (lastSnapshot == 0) {
                        //do initial snapshot
                    }
                    else {
                        // do delta snapshot
                    }
                }
            }
        }

        private NetOutgoingMessage CreateMessageFromSnapshot(Snapshot snapshot) {
            return null;
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
                        ProcessStatusChangeMessage(msg);
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
                    // Snapshot Ack
                    // 1 long = most recent received snapshot
                    
                    if (baselineSnapshotMap.ContainsKey(msg.SenderConnection)) {
                        baselineSnapshotMap[msg.SenderConnection] = msg.ReadUInt64();
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessStatusChangeMessage(NetIncomingMessage msg) {
            NetConnectionStatus status = msg.SenderConnection.Status;
            switch (status) {
                case NetConnectionStatus.Connected:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Connected!");
                    HandleClientConnected(msg);
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Initiated Connect!");
                    break;
                case NetConnectionStatus.Disconnected:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Disconnected!");
                    baselineSnapshotMap.Remove(msg.SenderConnection);
                    break;
                case NetConnectionStatus.None:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: None!");
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Received Invitation!");
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Respond Awaiting Approval!");
                    break;
                case NetConnectionStatus.RespondedConnect:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Respond Connect!");
                    break;
                case NetConnectionStatus.Disconnecting:
                    System.Diagnostics.Debug.WriteLine("[SERVER] Received Status Change: Disconnecting!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleClientConnected(NetIncomingMessage msg) {
            baselineSnapshotMap.Add(msg.SenderConnection, 0);
                    
            // Send the world header to the newly connected client
            NetOutgoingMessage sendMsg = peer.CreateMessage();
            WorldManager worldManager = application.GetCurrentWorldManager();
            if (worldManager != null) {
                WorldHeader header = worldManager.GetWorldHeader();
                sendMsg.Write((byte)0);    //message type
                sendMsg.Write(header.name);
                sendMsg.Write(header.width);
                sendMsg.Write(header.height);
                sendMsg.Write(header.spawnArea.X);
                sendMsg.Write(header.spawnArea.Y);

                peer.SendMessage(sendMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
            }
            else {
                msg.SenderConnection.Disconnect("The host world is not yet loaded!");
            }
        }

    }
}