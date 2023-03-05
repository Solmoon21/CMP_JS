using UnityEngine;
using System;
using System.Text;

namespace ClawbearGames
{
    public class DailyRewardManager : MonoBehaviour
    {
        [SerializeField] private DailyRewardConfiguration[] dailyRewardConfigurations = null;
        public DailyRewardConfiguration[] DailyRewardConfigurations { get { return dailyRewardConfigurations; } }

        private void Start()
        {
            //Setup PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX, 0);
            }


            //Setup PPK_IS_CLAIMED_LAST_REWARD
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_IS_CLAIMED_LAST_REWARD))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_IS_CLAIMED_LAST_REWARD, 0);
            }
        }



        /// <summary>
        /// Get the amount of time remains till next reward.
        /// </summary>
        /// <returns></returns>
        public int TimeRemainsTillNextReward()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_SAVED_DAY_TIME_OF_LATEST_REWARD))
            {
                return 0;
            }
            else
            {
                //Get the saved date time of latest reward
                string[] dayTimeDatas = PlayerPrefs.GetString(PlayerPrefsKeys.PPK_SAVED_DAY_TIME_OF_LATEST_REWARD).Split(':');
                int year = int.Parse(dayTimeDatas[0]);
                int month = int.Parse(dayTimeDatas[1]);
                int day = int.Parse(dayTimeDatas[2]);
                int hour = int.Parse(dayTimeDatas[3]);
                int minute = int.Parse(dayTimeDatas[4]);
                int second = int.Parse(dayTimeDatas[5]);
                DateTime dateTimeOfLatestReward = new DateTime(year, month, day, hour, minute, second);

                TimeSpan timePassed = DateTime.Now.Subtract(dateTimeOfLatestReward);
                int timeRemains = Mathf.Clamp(86400 - ((int)timePassed.TotalSeconds), 0, 86400);

                //User already claimed the last reward and already passed 24 hours
                if (timeRemains == 0 && PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_IS_CLAIMED_LAST_REWARD) == 1)
                {
                    //Reset PPK_IS_CLAIMED_LAST_REWARD, PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX and reset claimed panel of all daily reward items
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_IS_CLAIMED_LAST_REWARD, 0);
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX, 0);
                    ViewManager.Instance.DailyRewardViewController.UpdateClaimedPanelOfAllItems();
                }
                return timeRemains;
            }
        }


        /// <summary>
        /// Handle actions when user claimed the current daily reward.
        /// </summary>
        public void HandleClaimedCurrentDailyReward()
        {
            //Set day time values
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(DateTime.Now.Year + ":");
            stringBuilder.Append(DateTime.Now.Month + ":");
            stringBuilder.Append(DateTime.Now.Day + ":");
            stringBuilder.Append(DateTime.Now.Hour + ":");
            stringBuilder.Append(DateTime.Now.Minute + ":");
            stringBuilder.Append(DateTime.Now.Second);
            PlayerPrefs.SetString(PlayerPrefsKeys.PPK_SAVED_DAY_TIME_OF_LATEST_REWARD, stringBuilder.ToString().Trim());

            //Update PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX) + 1);
            int currentIndex = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX);
            if (currentIndex == dailyRewardConfigurations.Length)
            {
                //User just claimed the last daily reward item -> update PPK_IS_CLAIMED_LAST_REWARD
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_IS_CLAIMED_LAST_REWARD, 1);
            }
        }



        /// <summary>
        /// Get the coin amount of current daily reward.
        /// </summary>
        /// <returns></returns>
        public int GetCoinAmountOfCurrentDailyReward()
        {
            int currentIndex = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_DAILY_REWARD_CONFIGURATION_INDEX);
            currentIndex = (currentIndex == dailyRewardConfigurations.Length) ? 0 : currentIndex;
            return dailyRewardConfigurations[currentIndex].CoinAmount;
        }
    }
}
