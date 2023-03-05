using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClawbearGames
{
    public class Utilities
    {
        /// <summary>
        /// Is finished tutorial.
        /// </summary>
        /// <returns></returns>
        public static bool IsShowTutorial()
        {
            return PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TUTORIAL, 0) == 1;
        }


        /// <summary>
        /// Covert the given seconds to time format.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string SecondsToTimeFormat(double seconds)
        {
            int hours = (int)seconds / 3600;
            int mins = ((int)seconds % 3600) / 60;
            seconds = Math.Round(seconds % 60, 0);
            return hours + ":" + mins + ":" + seconds;
        }

        /// <summary>
        /// Covert the given seconds to minutes format.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string SecondsToMinutesFormat(double seconds)
        {
            int mins = ((int)seconds % 3600) / 60;
            seconds = Math.Round(seconds % 60, 0);
            return mins + ":" + seconds;
        }
    }
}
