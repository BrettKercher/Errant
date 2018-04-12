using System.IO;
using Lidgren.Network;

namespace Errant.Networking {
    public class StreamingService {

        private NetConnection connection;
        private FileStream fileStream;
        private int sentOffset;
        private int chunkLen;
        private byte[] tmpBuffer;
        
        public StreamingService(NetConnection _connection) {
            connection = _connection;
            //fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            chunkLen = connection.Peer.Configuration.MaximumTransmissionUnit - 20;
            tmpBuffer = new byte[chunkLen];
            sentOffset = 0;
        }

        public void SendDataChunk() {
            
            if (fileStream == null) {
                return;
            }

            if (connection.CanSendImmediately(NetDeliveryMethod.ReliableOrdered, 1))
            {
                // send another part of the file!
                long remaining = fileStream.Length - sentOffset;
                int bytesToSend = remaining > chunkLen ? chunkLen : (int)remaining;

                fileStream.Read(tmpBuffer, sentOffset, bytesToSend);

                NetOutgoingMessage om;
                if (sentOffset == 0)
                {
                    // first message; send length, chunk length and file name
                    om = connection.Peer.CreateMessage(bytesToSend + 8);
                    om.Write((ulong)fileStream.Length);
                    om.Write(Path.GetFileName(fileStream.Name));
                    connection.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 1);
                }

                om = connection.Peer.CreateMessage(bytesToSend + 8);
                om.Write(tmpBuffer, 0, bytesToSend);

                connection.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 1);
                sentOffset += bytesToSend;

                if (remaining - bytesToSend <= 0)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    fileStream = null;
                }
            }
            
        }
        
    }
}