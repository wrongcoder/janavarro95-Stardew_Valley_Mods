using System;


namespace Omegasis.HappyBirthday.Framework.Compatibility
{
    /// <summary>Stardew-Access screenreader API interface for accessibility.</summary>
    public interface IStardewAccessApi
    {
        public void SayWithMenuChecker(string text, bool interrupt);
    }
}
