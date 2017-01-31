using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        
        string sourceName;
        string targetName;
        /*
        // 1
        // Initialize process information.
        //
        ProcessStartInfo p = new ProcessStartInfo();
        p.FileName = "7za.exe";

        // 2
        // Use 7-zip
        // specify a=archive and -tgzip=gzip
        // and then target file in quotes followed by source file in quotes
        //
        p.Arguments = "a \"" + targetName + "\" \"" +
            sourceName + "\" ";
        p.WindowStyle = ProcessWindowStyle.Maximized;
        

        // 3.
        // Start process and wait for it to exit
        //
        Process x = Process.Start(p);
        x.WaitForExit();


        ProcessStartInfo P = new ProcessStartInfo();

        P.FileName = "7za.exe";

        P.Arguments = "e Example.7z -oc:\\Users\\owner\\Downloads -y";
        P.WindowStyle = ProcessWindowStyle.Maximized;
        Process X = Process.Start(P);

        X.WaitForExit();
        */

        string[] fileContents = File.ReadAllLines("SearchDirectories.txt");

        List<string> directoryList = new List<string>();
        List<string> allZipFiles = new List<string>();

        foreach(var v in fileContents)
        {
            
            directoryList.Add(v);
        }

        foreach (var v in directoryList)
        {
            Console.WriteLine("Searching " + v + " for .zip files");
            string Path = v;
            string[] zipsInDirectory = Directory.GetFiles(Path, "*.zip", SearchOption.AllDirectories);
            int count2 = zipsInDirectory.Length;
            int count3 = 1;
        foreach(var q in zipsInDirectory)
            {
                Console.WriteLine("Adding in zip number " + count3 + " of " + count2 + " to the archival list.");
                Console.WriteLine("Adding in zip " + q + " to the archival list.");
                allZipFiles.Add(q);
                count3++;
            }

        }

        int maxCount = allZipFiles.Count;
        int count = 1;
        List<string> removalList = new List<string>();

        foreach(var v in allZipFiles)
        {
            Console.WriteLine("Compressing File "+ count +" of "+ maxCount);
            // Console.WriteLine(v.ToString());
            // FileInfo fInfo = new FileInfo(v);
            // Console.WriteLine( fInfo.FullName);
     /*
            string s = v.Remove(v.Length - 4, 4);
            Console.WriteLine("Compressing " +s);
            sourceName = v.ToString();
            targetName = s + ".7z";
             ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = "7za.exe";
            process.Arguments = "a \"" + targetName + "\" \"" +
              sourceName + "\" -y";
            process.WindowStyle = ProcessWindowStyle.Minimized;
             Process processRun = Process.Start(process);
            count++;
             processRun.WaitForExit();
            Console.WriteLine("Sucessfully compressed "+ s +" to .7zip format.");
            
    */
    removalList.Add(v);
        }

        System.Threading.Thread.Sleep(1000);
        foreach (var v in removalList)
        {
            Console.WriteLine("Deleting " + v);
            File.Delete(v);

        }

        System.Threading.Thread.Sleep(1000);
    }
}