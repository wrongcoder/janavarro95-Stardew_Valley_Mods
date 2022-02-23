using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.HappyBirthday.Framework.Gifts;

namespace Omegasis.HappyBirthday.Framework.API
{
    public class HappyBirthdayAPI
    {

        public event EventHandler OnBirthdayGiftRegistered;
        public event EventHandler PostAllBirthdayGiftsRegistered;
        public event EventHandler<StardewValley.Item> BeforeBirthdayGiftReceived;

        public HappyBirthdayAPI()
        {
        }

        public bool RegisterBirthdayGift(string UniqueGiftId, StardewValley.Item item)
        {
           return GiftIDS.RegisterGift(UniqueGiftId, item);
        }

        public bool IsGiftRegistered(string UniqueGiftId)
        {
            return GiftIDS.IsGiftRegistered(UniqueGiftId);
        }

        public bool UnRegisterGift(string UnqiueGiftId)
        {
            return GiftIDS.RemoveGift(UnqiueGiftId);
        }

        public bool ModifyGift(string UnqiueGiftId, StardewValley.Item ReplacementGift)
        {
            return GiftIDS.ModifyGift(UnqiueGiftId, ReplacementGift);
        }


        public bool AddDefaultBirthdayGift(string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return HappyBirthdayModCore.Instance.giftManager.registerDefaultBirthdayGift(UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount);
        }

        public bool AddNPCBirthdayGift(string NpcName,string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return HappyBirthdayModCore.Instance.giftManager.registerNpcBirthdayGift(NpcName,UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount);
        }

        public bool AddSpouseBirthdayGift(string SpouseName,string UniqueGiftId, int MinHeartsRequiredForGift, int MaxHeartsRequiredForGift, int MinStackAmount, int MaxStackAmount)
        {
            return HappyBirthdayModCore.Instance.giftManager.registerSpouseBirthdayGift(SpouseName, UniqueGiftId, MinHeartsRequiredForGift, MaxHeartsRequiredForGift, MinStackAmount, MaxStackAmount);
        }

        /*
        public bool RemoveDefaultBirthdayGift()
        {

        }

        public bool RemoveNPCBirthdayGift()
        {

        }

        public bool RemoveSpouseBirthdayGift()
        {

        }


        public bool AddEvent()
        {

        }

        public bool ModifyEvent()
        {

        }

        public bool RemoveEvent()
        {

        }
        */

    }
}
