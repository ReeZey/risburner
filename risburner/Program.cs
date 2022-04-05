using System;
using System.IO;
using System.Windows.Forms;
using risburner.VideoConverters;

namespace risburner
{
    
    class Program
    {
        private static string inputFile;
        
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                inputFile = args[0];
            }
            else
            {
                var fd = new OpenFileDialog();
                fd.ShowDialog();
                inputFile = fd.FileName;
            }

            var inputFileInfo = new FileInfo(inputFile);
            if (!inputFileInfo.Exists)
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            
            SplitVideo.Init(inputFileInfo);

            Console.WriteLine("Convertion done, press any key to quit...");
            Console.ReadKey();
        }
        
        public static Action<double> onProgress = p =>
        {
            if (double.IsNaN(p)) p = 0;
            Console.WriteLine($"Progress: {p}%");
        };
    }
}