using CommandLine;
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
        public class Options
        {
            [Option('t', "title", Required = false, HelpText = "Title of message box")]
            public string Title { get; set; }

            [CommandLine.Value(0)]
            public string MainText { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Parser.Default.ParseArguments<Options>(args)
             .WithParsed<Options>(o =>
             {
                 string message = string.IsNullOrEmpty(o.MainText) ? "No Message" :
                     HttpUtility.UrlDecode(Encoding.UTF8.GetBytes(o.MainText), Encoding.UTF8);

                 FlexibleMessageBox.Show(message, o.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
             });
        }
    }
}
