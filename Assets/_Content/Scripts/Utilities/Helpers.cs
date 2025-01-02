using System.Collections.Generic;
using UnityEngine;

namespace _Content.Scripts.Utilities
{
    public static class Helpers
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitForSeconds = new();

        /// <summary>
        /// Returns a WaitForSeconds object for the specified duration. 
        /// </summary>
        /// <param name="seconds"> The duration in seconds to wait. </param>
        /// <returns> WaitForSeconds object. </returns>
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (WaitForSeconds.TryGetValue(seconds, out var forSeconds)) return forSeconds;
            
            var waitForSeconds = new WaitForSeconds(seconds);
            WaitForSeconds.Add(seconds, waitForSeconds);
            return WaitForSeconds[seconds];
        }
    }
}