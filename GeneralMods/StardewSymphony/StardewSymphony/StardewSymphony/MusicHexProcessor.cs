using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stardew_Music_Expansion_API
{
    class MusicHexProcessor
    {
        public static List<string> allsoundBanks;
        public static List<string> allHexDumps;
        public static List<string> allWaveBanks;

        public static void processHex()
        {
            int counter = 0;
            string HexDumpContents = "";
            string rawName = "";
            string rawHexName = "";
            string cueName = "";
            List<string> cleanCueNames = new List<string>();
            foreach (var v in allsoundBanks)
            {
                cleanCueNames = new List<string>();
                byte[] array = System.IO.File.ReadAllBytes(v);
               // Log.AsyncC(HexDump(array));
                rawName=v.Substring(0,v.Length-4);
                cueName = (rawName + "CueList.txt");
                if (File.Exists(cueName)) continue;

                HexDumpContents = HexDump(array);

                rawHexName = rawName + "HexDump.txt";
                File.WriteAllText(rawHexName, HexDumpContents);
                // string fileName = (v.Remove(v.Length - 5, v.Length-1));
                //Log.AsyncM(fileName);
                allHexDumps.Add(rawHexName);
            
                string[] readText = File.ReadAllLines(rawHexName);
                string largeString="";
                foreach (var line in readText)
                {
                   // Log.AsyncY(line);
                    try
                    {
                        string newString = "";
                        for (int i = 62; i <= 77; i++)
                        {
                            newString += line[i];
                        }
                        // Log.AsyncG(newString);
                        largeString += newString;
                        // Log.AsyncC(largeString);
                        //Log.AsyncG(line.Substring(63, 78));
                    }
                    catch (Exception e)
                    {
                        // Log.AsyncY("WTF");
                        // Log.AsyncO(line.Length);
                    }
                }
                    string[] splits = largeString.Split('ÿ');
                    string fix = "";
                    foreach (string s in splits)
                    {
                        if (s == "") continue;
                        fix += s;
                    }
                    splits = fix.Split('.');

                    foreach (var split in splits)
                    {
                        if (split == "") continue;
                       // Log.AsyncM(split);

                        try
                        {



                        // Game1.playSound(split);
                        Game1.waveBank = Class1.master_list[counter].newwave;
                        Game1.soundBank = Class1.master_list[counter].new_sound_bank;
                        
                       
                        if (Game1.soundBank.GetCue(split) != null)
                        {
                            //Game1.playSound(split);
                            cleanCueNames.Add(split);
                           // Log.AsyncG("Sucessfully added " + split + " to the list of successful songs to play.");
                        }

                        Class1.reset();

                        }
                        catch(Exception e)
                        {
                         //   Log.AsyncR(e);
                        }

                    }

                cueName = (rawName + "CueList.txt");
               // Log.AsyncM(cueName);
                cleanCueNames.Sort();
                File.WriteAllLines(cueName, cleanCueNames);
                counter++;
            }
             
            }

           




        

        public static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = asciiSymbol(b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            return result.ToString();
        }
        static char asciiSymbol(byte val)
        {
            if (val < 32) return '.';  // Non-printable ASCII
            if (val < 127) return (char)val;   // Normal ASCII
            // Handle the hole in Latin-1
            if (val == 127) return '.';
            if (val < 0x90) return "€.‚ƒ„…†‡ˆ‰Š‹Œ.Ž."[val & 0xF];
            if (val < 0xA0) return ".‘’“”•–—˜™š›œ.žŸ"[val & 0xF];
            if (val == 0xAD) return '.';   // Soft hyphen: this symbol is zero-width even in monospace fonts
            return (char)val;   // Normal Latin-1
        }
    



}
}
