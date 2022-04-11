using CommandLine;
using JR.Utils.GUI.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
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

            [Option('m', "map", Required = false, HelpText = "Mapfile name for message")]
            public string Map { get; set; }

            [CommandLine.Value(0)]
            public string MainText { get; set; }
        }

        public static Byte[] ReadMMFAllBytes(string fileName)
        {
            using (var mmf = MemoryMappedFile.OpenExisting(fileName))
            {
                using (var stream = mmf.CreateViewStream())
                {
                    using (BinaryReader binReader = new BinaryReader(stream))
                    {
                        return binReader.ReadBytes((int)stream.Length);
                    }
                }
            }
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
                 string message;
                 if (!string.IsNullOrEmpty(o.Map))
                 {
                     message = o.Map;
                     MemoryMappedFile share_mem = MemoryMappedFile.OpenExisting(o.Map);
                     MemoryMappedViewAccessor accessor = share_mem.CreateViewAccessor();

                     // Write data to shared memory
                     int size = accessor.ReadInt32(0);

                     // sizeof Int
                     int pos = 4;

                     List<byte> data = new List<byte>();
                     for (int i = 0; i < size; i++)
                         data.Add(accessor.ReadByte(pos + i));

                     message = HttpUtility.UrlDecode(
                         Encoding.UTF8.GetString(data.ToArray()), Encoding.UTF8);

                     // Dispose resource
                     accessor.Dispose();
                     share_mem.Dispose();
                 }
                 else
                 {
                     message = string.IsNullOrEmpty(o.MainText) ? "No Message" :
                        HttpUtility.UrlDecode(Encoding.UTF8.GetBytes(o.MainText), Encoding.UTF8);
                 }
                 string caption = string.IsNullOrEmpty(o.Title) ? "" :
                    HttpUtility.UrlDecode(Encoding.UTF8.GetBytes(o.Title), Encoding.UTF8);

                 FlexibleMessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
             });
        }
    }
}
