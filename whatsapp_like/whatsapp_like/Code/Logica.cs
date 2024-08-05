using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace whatsapp_like.Code
{
    public class Logica
    {
        private Socket socket;
        private Thread thread;
        private TextBox mensaje;
        private int port;

        public Logica(int port, TextBox txtMessages)
        {
            this.port = port;
            this.mensaje = txtMessages;
            InitializeSocket();
        }

        private void InitializeSocket()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.socket.Bind(new IPEndPoint(IPAddress.Any, this.port));
            this.thread = new Thread(ReceiveMessages);
            this.thread.Start();
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    int received = this.socket.ReceiveFrom(buffer, ref remoteEndPoint);
                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    AppendMessage($"Received from {remoteEndPoint}: {message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AppendMessage(string message)
        {
            if (this.mensaje.InvokeRequired)
            {
                this.mensaje.Invoke(new Action<string>(AppendMessage), message);
            }
            else
            {
                this.mensaje.AppendText(message + Environment.NewLine);
            }
        }

        public void SendMessage(string message, int port)
        {
            try
            {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Loopback, port);
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                this.socket.SendTo(buffer, remoteEndPoint);
                AppendMessage($"Sent to {remoteEndPoint}: {message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Close()
        {
            this.thread.Abort();
            this.socket.Close();
        }
    }
}
