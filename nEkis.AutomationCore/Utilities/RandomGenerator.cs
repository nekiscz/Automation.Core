using System;
using System.Security.Cryptography;

namespace nEkis.Automation.Core.Utilities
{
    /// <summary>
    /// Overrides .Next() methods of System.Random class
    /// </summary>
    public class RandomGenerator : Random
    {
        readonly RNGCryptoServiceProvider csp;
        /// <summary>
        /// Populates RNGCryptoServiceProvider
        /// </summary>
        public RandomGenerator()
        {
            csp = new RNGCryptoServiceProvider();
        }
        /// <summary>
        /// Generates random number from zero to int.MaxValue
        /// </summary>
        /// <returns>Nonnegative random number</returns>
        new public int Next()
        {
            return Next(0, int.MaxValue);
        }
        /// <summary>
        /// Generates random number from zero to upper range
        /// </summary>
        /// <param name="maxValue">Exclusive upper limit of range</param>
        /// <returns>Nonnegative random number less than the specified maximum</returns>
        new public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
        /// <summary>
        /// Generates random number in given range
        /// </summary>
        /// <param name="minValue">Inclusive lower limit</param>
        /// <param name="maxValue">Exclusive upper limit</param>
        /// <returns>Random number within a specified range</returns>
        new public int Next(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

            long diff = (long)maxValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        private uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            csp.GetBytes(buffer);
            return buffer;
        }

    }
}
