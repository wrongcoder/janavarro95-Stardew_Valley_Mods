Stardew Valley Save_Anywhere Mod v 1.0.5

For SDV[1.07] and SMAPI [0.40]

Updated at 9:22 P.M on 6/8/16

-Fixed it where time simulation wouldn't happen at all. Now it propperly works again.

-Known Issues.

         -Can't save in the Community Center or Sewer due to some serializable issues outside of my control. Sorry. =(

Good afternoon everyone, and today I'd like to announce my biggest project yet, the Save_Anywhere mod! 

Now before you get super excited let me go over the basics of what this mod does.

Normally when you want to save you have to go to bed and go to the next day, which is typically ok. However, sometimes you want to save where you are and come back later to resume playing. What this mod does (at the time of writing) can be summarized like this.

1.Save the player's save file like going to bed would.
         -Things such as tiles watered, stamina, health, etc are preserved. Note that not everything that is 
normally saved is preserved.

2. Warp the player upon loading the game to the position they were when they saved. *1

3. Calculate shipping for the player upon saving (because items shipped aren't saved to a save file)

4.Recalculate NPC's path finding to be as as possible. *2.

5. Simulate game time up until when the character loaded. *3

6. Generated "save_anywhere saves" are deleted upon going to bed to prevent the character from being warped unnecessarily. One is always generated/updated when you save.
Now the asterisks are for the following.

*1. There is an issue where if you happen to resize/move/alter the game's window while the game is starting up/loading, then the player will not properly warp back to their original location. If this happens, reload the game and don't alter the game window/screen.

*2. I discovered when you save the game and reload it, that NPCs get randomly warped around the world which caused a massive problem. When the mod loads it will calculate the NPC's original positions and put them back into their staring positions so it might take just a few seconds before the game loads all the way.

*3. I chose to rapidly simulate in game time so that NPCs would be able to properly path find back to their save-game positions. For every 2 real life seconds, 10 in game minutes will pass and NPCs will move considerably faster to make sure that they move back to their save-game positions. This will last until the game-time is updated to the time when the game was saved.

Now in order to save the mod, you just need to press a key while playing the game. "k" is the default key, but you can change this in the config file.

That pretty much sums it up to this point. 

Thanks and have fun. =)

-Alpha_Omegasis

Min Requirements:
SMAPI 0.40 
Stardew Valley 1.07
