using System;
using System.IO;
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

            var inputName = inputFileInfo.Name.Split(".")[0];

            var videoDuration = new TimeSpan();
            
            Console.Write("how often should an split occur? format: 00:00:00 -> hours:minutes:seconds: "); 
            var segment = Console.ReadLine();
            if (string.IsNullOrEmpty(segment)) segment = "00:00:00";
            
            FFMpegArguments
                .FromFileInput(inputFileInfo)
                .OutputToFile(inputName + "-%05d" + inputFileInfo.Extension, true, options => options
                    .WithArgument(new CopyArgument())
                    .WithCustomArgument("-c copy")
                    .WithCustomArgument("-map 0")
                    .WithCustomArgument("-segment_time " + segment)
                    .WithCustomArgument("-f segment")
                    .WithCustomArgument("-reset_timestamps 1"))
                .NotifyOnProgress(Program.onProgress, videoDuration)
                .ProcessSynchronously();
            
            Console.WriteLine("done");
        }
    }
}