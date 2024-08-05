using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace whatsapp_like
{
    static class Program
    {
        static void Main(string[] port)
        {

            if (port.Length == 0 || !int.TryParse(port[0], out int portc))
            {
                MessageBox.Show("Invalid port number.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(portc));
        }
    }

}
