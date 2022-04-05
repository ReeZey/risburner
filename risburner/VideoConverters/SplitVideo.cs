using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FFMpegCore;
using FFMpegCore.Arguments;
using FFMpegCore.Enums;
using risburner.Interfaces;

namespace risburner.VideoConverters
{
    public class SplitVideo : VideoConvert
    {
        public static void Init(FileInfo inputFileInfo)
        {
            //ffmpeg -i input.mp4 -c copy -map 0 -segment_time 00:20:00 -f segment -reset_timestamps 1 output%03d.mp4

            var fd = new FolderBrowserDialog();
            fd.ShowDialog();
            
            Console.WriteLine(fd.SelectedPath);

            var inputName = inputFileInfo.Name.Split(".")[0];

            Console.Write("how often should an split occur? (format: 00:00:00 -> hours:minutes:seconds): "); 
            var segment = Console.ReadLine();
            if (string.IsNullOrEmpty(segment)) segment = "00:00:00";
            
            var timings = Array.ConvertAll(segment.Split(":"), int.Parse);
            var splitDuration = new TimeSpan(timings[0],timings[1], timings[2]);

            var duration = FFProbe.Analyse(inputFileInfo.FullName).Duration;

            var splitamount = duration.Divide(splitDuration);
            var segmentLength = ((int) Math.Ceiling(splitamount)).ToString().Length + 1;

            FFMpegArguments
                .FromFileInput(inputFileInfo)
                .OutputToFile($"{fd.SelectedPath}\\{inputName}-%0{segmentLength}d{inputFileInfo.Extension}", true, options => options
                    .WithCustomArgument("-c copy")
                    .WithCustomArgument("-map 0")
                    .WithCustomArgument("-segment_time " + segment)
                    .WithCustomArgument("-f segment")
                    .WithCustomArgument("-reset_timestamps 1")
                )
                .NotifyOnProgress(Program.onProgress, splitDuration)
                .ProcessSynchronously();
            
            Console.WriteLine("done");
        }
    }
}