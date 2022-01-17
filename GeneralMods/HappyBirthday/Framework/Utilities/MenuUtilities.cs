using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Omegasis.HappyBirthday.Framework.Utilities
{
    public static class MenuUtilities
    {
        /// <summary>Checks if the current billboard is the daily quest screen or not.</summary>
        public static bool IsDailyQuestBoard;

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        public static void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            switch (e.NewMenu)
            {
                case null:
                    OnActiveMenuChangedToNull();

                    return;

                case Billboard billboard:
                    {
                        OnMenuChangedToBillboard(billboard);
                        break;
                    }
                case DialogueBox dBox:
                    {
                        break;
                    }
            }

        }

        /// <summary>
        /// Occurs when 
        /// </summary>
        public static void OnActiveMenuChangedToNull()
        {
            IsDailyQuestBoard = false;
            //Validate the gift and give it to the player.
            if (HappyBirthday.Instance.lastSpeaker != null)
            {
                if (HappyBirthday.Instance.giftManager.BirthdayGiftToReceive != null && HappyBirthday.Instance.VillagerQueue[HappyBirthday.Instance.lastSpeaker.Name].hasGivenBirthdayGift == false)
                {
                    while (HappyBirthday.Instance.giftManager.BirthdayGiftToReceive.Name == "Error Item" || HappyBirthday.Instance.giftManager.BirthdayGiftToReceive.Name == "Rock" || HappyBirthday.Instance.giftManager.BirthdayGiftToReceive.Name == "???")
                        HappyBirthday.Instance.giftManager.setNextBirthdayGift(HappyBirthday.Instance.lastSpeaker.Name);
                    Game1.player.addItemByMenuIfNecessaryElseHoldUp(HappyBirthday.Instance.giftManager.BirthdayGiftToReceive);
                    HappyBirthday.Instance.giftManager.BirthdayGiftToReceive = null;
                    HappyBirthday.Instance.VillagerQueue[HappyBirthday.Instance.lastSpeaker.Name].hasGivenBirthdayGift = true;
                    HappyBirthday.Instance.lastSpeaker = null;
                }
            }
        }

        public static void OnMenuChangedToDialogueBox()
        {
            if (Game1.eventUp) return;
            //Hijack the dialogue box and ensure that birthday dialogue gets spoken.
            if (Game1.currentSpeaker != null)
            {
                HappyBirthday.Instance.lastSpeaker = Game1.currentSpeaker;
                if (Game1.activeClickableMenu != null && HappyBirthday.Instance.birthdayManager.isBirthday() && HappyBirthday.Instance.VillagerQueue.ContainsKey(Game1.currentSpeaker.Name))
                {
                    if (NPCUtilities.ShouldWishPlayerHappyBirthday(Game1.currentSpeaker.Name) == false) return;
                    if (Game1.activeClickableMenu is DialogueBox && NPCUtilities.ShouldWishPlayerHappyBirthday(Game1.currentSpeaker.Name))
                    {
                        Game1.currentSpeaker.resetCurrentDialogue();
                        Game1.currentSpeaker.resetSeasonalDialogue();
                        HappyBirthday.Instance.Helper.Reflection.GetMethod(Game1.currentSpeaker, "loadCurrentDialogue", true).Invoke();
                        Game1.npcDialogues[Game1.currentSpeaker.Name] = Game1.currentSpeaker.CurrentDialogue;
                        if (HappyBirthday.Instance.birthdayManager.isBirthday() && HappyBirthday.Instance.VillagerQueue[Game1.currentSpeaker.Name].hasGivenBirthdayGift == false)
                        {
                            try
                            {
                                HappyBirthday.Instance.giftManager.setNextBirthdayGift(Game1.currentSpeaker.Name);
                                HappyBirthday.Instance.Monitor.Log("Setting next birthday gift.");
                            }
                            catch (Exception ex)
                            {
                                HappyBirthday.Instance.Monitor.Log(ex.ToString(), LogLevel.Error);
                            }
                        }

                        Game1.activeClickableMenu = new DialogueBox(new Dialogue(HappyBirthday.Instance.birthdayMessages.getBirthdayMessage(Game1.currentSpeaker.Name), Game1.currentSpeaker));
                        HappyBirthday.Instance.VillagerQueue[Game1.currentSpeaker.Name].hasGivenBirthdayWish = true;

                        // Set birthday gift for the player to recieve from the npc they are currently talking with.

                    }

                }
            }
        }

        public static void OnMenuChangedToBillboard(Billboard billboard)
        {
            IsDailyQuestBoard = HappyBirthday.ModHelper.Reflection.GetField<bool>((Game1.activeClickableMenu as Billboard), "dailyQuestBoard", true).GetValue();
            if (IsDailyQuestBoard)
                return;

            Texture2D text = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            Color[] col = new Color[1];
            col[0] = new Color(0, 0, 0, 1);
            text.SetData<Color>(col);
            //players birthday position rect=new ....

            if (!string.IsNullOrEmpty(HappyBirthday.Instance.birthdayManager.playerBirthdayData.BirthdaySeason))
            {
                if (HappyBirthday.Instance.birthdayManager.playerBirthdayData.BirthdaySeason.ToLower() == Game1.currentSeason.ToLower())
                {
                    int index = HappyBirthday.Instance.birthdayManager.playerBirthdayData.BirthdayDay;

                    string bdayDisplay = Game1.content.LoadString("Strings\\UI:Billboard_Birthday");
                    Rectangle birthdayRect = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 200 + (index - 1) / 7 * 32 * 4, 124, 124);
                    billboard.calendarDays.Add(new ClickableTextureComponent("", birthdayRect, "", string.Format(bdayDisplay, Game1.player.Name), text, new Rectangle(0, 0, 124, 124), 1f, false));
                    //billboard.calendarDays.Add(new ClickableTextureComponent("", birthdayRect, "", $"{Game1.player.Name}'s Birthday", text, new Rectangle(0, 0, 124, 124), 1f, false));
                }
            }

            foreach (var pair in HappyBirthday.Instance.birthdayManager.othersBirthdays)
            {
                if (pair.Value.BirthdaySeason != Game1.currentSeason.ToLower()) continue;
                int index = pair.Value.BirthdayDay;

                string bdayDisplay = Game1.content.LoadString("Strings\\UI:Billboard_Birthday");
                Rectangle otherBirthdayRect = new Rectangle(Game1.activeClickableMenu.xPositionOnScreen + 152 + (index - 1) % 7 * 32 * 4, Game1.activeClickableMenu.yPositionOnScreen + 200 + (index - 1) / 7 * 32 * 4, 124, 124);
                billboard.calendarDays.Add(new ClickableTextureComponent("", otherBirthdayRect, "", string.Format(bdayDisplay, Game1.getFarmer(pair.Key).Name), text, new Rectangle(0, 0, 124, 124), 1f, false));
            }
        }
    }
}
