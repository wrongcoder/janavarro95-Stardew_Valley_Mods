using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using System.IO;
using System.Net.Mime;
using StardewModdingAPI.Inheritance;
using StardewValley;
using StardewValley.Tools;
using Microsoft.Xna.Framework;
using StardewValley.Objects;

using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace ClassLibrary1
{

    public class AutoSpeed : Mod
    {

        string speedstring = "player_setspeed 5";
        bool auto_update = false;
        string mypath = "";
        public override string Name
        {
            get { return "AutoSpeed"; }
        }

        public override string Authour
        {
            get { return "Alpha_Omegasis"; }
        }

        public override string Version
        {
            get { return "0.0.3a"; }
        }

        public override string Description
        {
            get { return "Makes the player move faster upon gameloading and new days."; }
        }

        public override void Entry(params object[] objects)
        {
            //NOTE ALL COMMANDS MUST BE WRITTEN IN FUNCTIONS NOW LIKE HOW THIS PROGRAM DOCUMENTS IT. OTHERWISE NOTHING WILL WORK
            set_up();
           // StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += Events_UpdateTick;
            finish();
            StardewModdingAPI.Events.GameEvents.UpdateTick += Events_UpdateTick;
            // StardewModdingAPI.Events.GameEvents.UpdateTick+=Events_UpdateTick;
        }


        public void finish()
        {
            Console.WriteLine("FINISHED LOADING");
        }

        public void set_up()
        {
            mypath = Path.GetFullPath("Mods/AutoSpeed.txt");
            //mypath = Path.GetFullPath("Mods/SpeedMod.txt");
            Console.WriteLine("Found SpeedFile.txt at " + mypath);
            // Open the text file using a stream reader.
            using (StreamReader sr = new StreamReader(mypath))
            {
                // Read the stream to a string
                String funstring = sr.ReadToEnd();
                Console.WriteLine(funstring);
                speedstring = funstring;
            }
            Console.WriteLine("AutoSpeed mod has started.");
            Program.LogColour(ConsoleColor.Magenta, "SpeedMod makes the player move faster upon loading and new days.");
            Program.LogColour(ConsoleColor.Cyan, "This has been update for SMAPI 0.0.7");
            
            //GameEvents.UpdateTick += Events_UpdateTick;

        }

        public void Events_UpdateTick(object sender, EventArgs e)
        {

            //Console.WriteLine("In DA LOOP");

            if (StardewModdingAPI.Inheritance.SGame.newDay)
            {
                auto_update = false;
                //New_day_Update();
            }
           if (StardewModdingAPI.Inheritance.SGame.hasLoadedGame) //makes sure a game file is loaded up
                    {
               
                       // Console.WriteLine("LOADED");
           if (StardewModdingAPI.Inheritance.SGame.player.isMoving()) //waits for the character to move to update speed
              {
                  //Console.WriteLine("MOVING");
                  if (auto_update == false)
                  {
                      New_day_Update();
                  }
               }
             }
           //Console.WriteLine("Leaving da loop");
            }

        void New_day_Update() //updates all info whenever I call this.
        {
            StardewModdingAPI.Command.CallCommand(speedstring);
            Console.WriteLine("AutoSpeed has updated player's speed"); //super hacky way of doing this;
            auto_update = true;
        }

      //  public EventArgs e { get; set; }
    }
}