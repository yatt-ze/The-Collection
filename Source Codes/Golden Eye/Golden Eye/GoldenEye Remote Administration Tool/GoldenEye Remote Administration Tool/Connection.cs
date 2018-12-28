using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace GoldenEye_Remote_Administration_Tool
{
    public class Connection
    {
        private TcpClient client; //This will hold the TcpClient.
        private string ip; //This will hold the IP Address.
        private string uniqID;

        //===========================Events=============================
        public delegate void Disconnected(Connection client);

        public event Disconnected DisconnectedEvent;

        public delegate void Received(Connection client, string Message);

        public event Received ReceivedEvent;

        //===============================================================
        public Connection(TcpClient client)
        {
            this.client = client; //Sets the client with the new TcpClient.
            ip = client.Client.RemoteEndPoint.ToString().Remove(client.Client.RemoteEndPoint.ToString().LastIndexOf(':'));
            uniqID = GetRandomString();
            try
            {
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null); //Begins reading...
            }
            catch { }
        }

        private void Read(IAsyncResult ar)
        {
            try
            {
                StreamReader reader = new StreamReader(client.GetStream()); //Sets new stream reader.
                string msg = reader.ReadLine(); //Reads a line and holds it in msg.
                if (msg == "") //If it disconnects then it will show up as nothing.
                {
                    DisconnectedEvent(this); //Raises the disconnected event.
                    return; //This is the same as Exit Sub.
                }
                ReceivedEvent(this, msg);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception eax)
            {
                //  MessageBox.Show(eax.ToString());

                //Main asdf = new Main();
                //if(asdf.disconnectOnClientErrorS.Equals("true")) {
                // this.Send("RECONNECT");
                DisconnectedEvent(this);
                //}
            }
        }

        public void Send(string Message)
        {//You can guess this.
            try
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(Message);
                writer.Flush();
            }
            catch
            {
            }
        }

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        public string IPAddress
        {
            get
            {
                return ip; //Returns the ip.
            }
        }

        public string UniqueID
        {
            get
            {
                return uniqID; //Returns the Unique BotID.
            }
        }
    }
}