using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace whatsapp_like
{
    public partial class Form1 : Form
    {
        private Socket _socket;
        private Thread _receiveThread;
        private int _port;

        public Form1(int port)
        {
            InitializeComponent();
            _port = port;
            InitializeSocket();
        }

        private void InitializeSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _receiveThread = new Thread(ReceiveMessages);
            _receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    int received = _socket.ReceiveFrom(buffer, ref remoteEndPoint);
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
            if (txtMessages.InvokeRequired)
            {
                txtMessages.Invoke(new Action<string>(AppendMessage), message);
            }
            else
            {
                txtMessages.AppendText(message + Environment.NewLine);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMessage.Text;

                if (string.IsNullOrEmpty(txtPort.Text))
                {
                    MessageBox.Show("Please enter a destination port.");
                    return;
                }

                if (!int.TryParse(txtPort.Text, out int port))
                {
                    MessageBox.Show("Invalid port number.");
                    return;
                }

                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Loopback, port);
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                _socket.SendTo(buffer, remoteEndPoint);
                AppendMessage($"Sent to {remoteEndPoint}: {message}");
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _receiveThread?.Abort();
            _socket?.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

