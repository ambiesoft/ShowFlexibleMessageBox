using JR.Utils.GUI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Ambiesoft.ShowFlexibleMessageBox
{
    internal static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string message = args.Length == 0 ? "No Message" :
                HttpUtility.UrlDecode(Encoding.UTF8.GetBytes(args[0]), Encoding.UTF8);

            FlexibleMessageBox.Show(message);
        }
    }
}
