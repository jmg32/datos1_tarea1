using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using whatsapp_like.Code;

namespace whatsapp_like
{
    public partial class Form1 : Form
    {
        private Logica logica;

        public Form1(int port)
        {
            InitializeComponent();
            this.logica = new Logica(port, txtMessages);
        }

        private void btnSend_Click(object sender, EventArgs e)
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

            this.logica.SendMessage(message, port);
            txtMessage.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.logica.Close();
        }
    }
}

