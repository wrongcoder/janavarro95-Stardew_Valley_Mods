Custom_Shop_Mod_Redux_GUI v 1.2.0

For SDV 1.0.7 and SMAPI v0.40

Posted at 9:15 PM PST 5/8/16

-Updated to be able to be called from other mods for NPC shop binding and soforth.

-Fixed a bug where pressing the U key on the main menu causes the game to crash.

Good evening everyone. At this point I don't even know how I'm creating mods so quickly, but I think I'll strike while the iron is hot. Here is my third release of the day, the Custom Shops Mod Redux!

The custom_shop_mod_redux is a second, much better attempt at adding custom shops into StardewValley!

***Not to be confused with the custom_shop_mod which was it's predecessor and was based around the SMAPI console.

Basically making a custom shop is as simple as following the formatting in the pre-existsing custom_shops in a new text file. (Or by using the super useful Custom Shop Creation Tool)

You can access these custom shops by pressing a key. (The U key by default, which is configurable in the config file)

I recommend that you use the Custom Shop Creation Tool that I created for everyone to create your custom shops. The link is down below. Took me a little bit of time to put together, but it works.

Watch the new video to understand how it works.

Have fun with this one! I hope to see all of the custom shops that you all put out!

====================================================
If you wish to call this mod in your own mod without the menu the code would be

Custom_Shop_Mod_Redux.Class1.external_shop_file_call(path, filename);

Where path is the directory path of the desired text file, and filename is the name of the file inside of the path directory.

*Note that path will not include the file itself. The code will take care of that for you.

This would be useful if you want a custom NPC to have a shop but you don't want to have to code in the shop yourself.
=========================================================
Some goals of mine with this mod:

1. Get as many different types of items available for selling. (finished)

2.Create a nice gui interface in Unity for creating custom shops so that modders don't have to deal with my icky formatting rules. The GUI will take care of that for modders. (finished)

3.Make my code compatible with other mods for modders, so that they can call my shop_command_code and be able to open up a shop from text file with just path information, and file names. (This is next)

4. Document the mod for future user use.
==========================================================
Types of Items this mod supports in shops.
1.Items (like normal inventory items)

2.Furniture- (windows, tables, chairs, etc)

3.swords- Swish, swish.

4.Hats -Got to look cool 

5.Boots - Lace up for adventure.    

6.Wallpapers - Make your house look nice.

7.Carpets/Flooring - Like animal crossing.
 
8.Rings - As long as they aren't evil.
 
9.Lamps - Light up the world.

10.craftables* =*note that there was some....issues adding in craftable objects. They all act like torches when you interact with them. It's kind of hilarious and I don't think I'll change it anytime soon.  You can still have objects like the furnace function like normal, by right clicking it with copper. In order to get the smelted copper bar however, you would have to destroy it, as would go for all machines that behave this way. Sorry. On the plus side your scarecrows can be on fire forever. 


Link:
Get it Here!

Link for the Custom Shops Creation Tool.:

https://myscccd-my.sharepoint.com/personal/0703280_my_scccd_edu/_layouts/15/guestaccess.aspx?guestaccesstoken=ZYxG9Cs8S0q%2bxCVV3fEnc8MI4SfVfe07919rhFUhRiA%3d&docid=0e51dae1da2eb43988f77f5c54ec3ee58

Also here is a video demonstrating how this mod works.

https://youtu.be/bSvNTZmgeZE

