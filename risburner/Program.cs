using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using risburner.Interfaces;
using risburner.VideoConverters;

namespace risburner
{
    
    class Program
    {
        private static string inputFile;
        
        private static List<Converter> converters = new List<Converter>();
        
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

            var inputFileInfo = new FileInfo(inputFile);
            if (!inputFileInfo.Exists)
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            
            int page = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"---- page {page + 1} ----");

                var pageoffset = page * 7;
                
                Dictionary<int, Converter> tempArray = new Dictionary<int, Converter>();
                
                for (int i = pageoffset; i < pageoffset + 7; i++)
                {
                    int index = i - pageoffset;

                    string temp;
                    
                    try
                    {
                        temp = converters[i].ToString()?.Split(".")[2];
                        tempArray.Add(index + 1, converters[i]);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    Console.WriteLine($"{index + 1}: " + temp);
                }
                
                if(page != 0) Console.WriteLine("8: previous page");
                Console.WriteLine("9: next page");
                
                switch (Console.Read())
                {
                    case '1':
                        if(tempArray.ContainsKey(1)) tempArray[1].Init(inputFileInfo);
                        break;
                    case '2':
                        if(tempArray.ContainsKey(2)) tempArray[2].Init(inputFileInfo);
                        break;
                    case '3':
                        if(tempArray.ContainsKey(3)) tempArray[3].Init(inputFileInfo);
                        break;
                    case '4':
                        if(tempArray.ContainsKey(4)) tempArray[4].Init(inputFileInfo);
                        break;
                    case '5':
                        if(tempArray.ContainsKey(5)) tempArray[5].Init(inputFileInfo);
                        break;
                    case '6':
                        if(tempArray.ContainsKey(6)) tempArray[6].Init(inputFileInfo);
                        break;
                    case '7':
                        if(tempArray.ContainsKey(7)) tempArray[7].Init(inputFileInfo);
                        break;
                    case '8':
                        if(--page < 0) page = 0;
                        break;
                    case '9':
                        page++;
                        break;
                }
            }

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