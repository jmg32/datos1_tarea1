using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace whatsapp_like
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Please provide a port number as an argument.");
                return;
            }

            if (!int.TryParse(args[0], out int port))
            {
                MessageBox.Show("Invalid port number.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(port));
        }
    }

}
