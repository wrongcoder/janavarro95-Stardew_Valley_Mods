using System;


namespace Omegasis.HappyBirthday
{
    /// <summary>Stardew-Access screenreader API interface for accessibility.</summary>
    public interface IStardewAccessApi
    {
        public void SayWithMenuChecker(String text, Boolean interrupt);
    }
}
