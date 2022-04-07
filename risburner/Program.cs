using System;
using System.Collections.Generic;
using System.Windows.Forms;
using risburner.Interfaces;
using risburner.VideoConverters;

namespace risburner
{
    
    class Program
    {
        public static string inputFile;

        public static List<Converter> converters = new List<Converter>();
        
        [STAThread]
        private static void Main(string[] args)
        {
            converters.AddRange(new Converter[]
            {
                new SplitVideo()
            });
            
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
            
            var convertUtility = new ConvertUtility();
            convertUtility.Run();
        }
    }
}