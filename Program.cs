using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace replicator
{
    class Program
    {
        static void Main(string[] args)
        {
            var secondMultiplier = 1_000;
            var source = args[0];
            var destination = args[1];
            var delaySeconds = int.Parse(args[2]) * secondMultiplier;
            var startTime = DateTime.Parse(args[3]);
            var endTime = DateTime.Parse(args[4]);

            if (!Directory.Exists(source)){
                Console.WriteLine($"Folder {source} does not exist");
                return;
            }

            if (!Directory.Exists(source)){
                Console.WriteLine($"Folder {source} does not exist");
                return;
            }


            while (IsAvailableTime(startTime, endTime)){
                var sourceFiles = GetFilesFromFolder(source);
                var destinationFiles = GetFilesFromFolder(destination);

                sourceFiles.ExceptWith(destinationFiles);

                Parallel.ForEach(sourceFiles, file => 
                    File.Copy(Path.Combine(source, file), Path.Combine(destination, file), true));

                Thread.Sleep(delaySeconds);
            }
        }

        static HashSet<string> GetFilesFromFolder(string path){
            var set = new HashSet<string>();

            foreach(var name in Directory.GetFiles(path)){
                set.Add(Path.GetFileName(name));
            }

            return set;
        }

        static bool IsAvailableTime(DateTime startTime, DateTime endTime){
            var currentTime = DateTime.Now;

            return startTime.TimeOfDay <= currentTime.TimeOfDay && currentTime.TimeOfDay <= endTime.TimeOfDay;
        }
    }
}
