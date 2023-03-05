using System.Collections;
using UnityEngine;

namespace ClawbearGames
{
    public class CoinManager : MonoBehaviour
    {
        [Header("Coin Manager Configuration")]
        [SerializeField] private int initialCoins = 0;
        [SerializeField] private int minRewardedCoins = 100;
        [SerializeField] private int maxRewardedCoins = 150;

        public int CollectedCoins { private set; get; }


        public int TotalCoins
        {
            private set { PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, value); }
            get { return PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, initialCoins); }
        }


        /// <summary>
        /// Set the TotalCoins by the given amount.
        /// </summary>
        /// <param name="amount"></param>
        public void SetTotalCoins(int amount)
        {
            TotalCoins = amount;
        }

        /// <summary>
        /// Add collected coins by given amount
        /// </summary>
        /// <param name="amount"></param>
        public void AddCollectedCoins(int amount)
        {
            CollectedCoins += amount;
        }


        /// <summary>
        /// Set the CollectedCoins by the given amount.
        /// </summary>
        /// <param name="amount"></param>
        public void SetCollectedCoins(int amount)
        {
            CollectedCoins = amount;
        }


        /// <summary>
        /// Get an amount of coins to reward to user.
        /// </summary>
        /// <returns></returns>
        public int GetRewardedCoins()
        {
            return Random.Range(minRewardedCoins, maxRewardedCoins) / 5 * 5;
        }




        /// <summary>
        /// Increase an amount of coins for TotalCoins with given delay time.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        public void IncreaseTotalCoins(int amount, float delay)
        {
            StartCoroutine(CRIncreaseTotalCoins(amount, delay));
        }


        /// <summary>
        /// Coroutine increase an amount of coins for TotalCoins with given delay time.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRIncreaseTotalCoins(int amount, float delay)
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Rewarded);
            yield return new WaitForSeconds(delay);
            float t = 0;
            float runTime = 0.25f;
            int startTotalCoins = TotalCoins;
            int endTotalCoins = startTotalCoins + amount;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                int newTotalCoins = Mathf.RoundToInt(Mathf.Lerp(startTotalCoins, endTotalCoins, factor));
                SetTotalCoins(newTotalCoins);
                yield return null;
            }
        }



        /// <summary>
        /// Increase an amount of coins for CollectedCoins with given delay time.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        public void IncreaseCollectedCoins(int amount, float delay)
        {
            StartCoroutine(CRIncreaseCollectedCoins(amount, delay));
        }


        /// <summary>
        /// Coroutine increase an amount of coins for CollectedCoins with given delay time.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRIncreaseCollectedCoins(int amount, float delay)
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Rewarded);
            yield return new WaitForSeconds(delay);
            float t = 0;
            float runTime = 0.25f;
            int startCollectedCoins = CollectedCoins;
            int endCollectedCoins = startCollectedCoins + amount;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                int newCollectedCoins = Mathf.RoundToInt(Mathf.Lerp(startCollectedCoins, endCollectedCoins, factor));
                SetCollectedCoins(newCollectedCoins);
                yield return null;
            }
        }




        /// <summary>
        /// Remove an amount of total coins.
        /// </summary>
        /// <param name="delay"></param>
        public void RemoveTotalCoins(int amount, float delay)
        {
            StartCoroutine(CRRemoveTotalCoins(amount, delay));
        }


        /// <summary>
        /// Coroutine remove an amount of total coins.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRRemoveTotalCoins(int amount, float delay)
        {
            yield return new WaitForSeconds(delay);
            float t = 0;
            float runTime = 0.15f;
            int startTotalCoins = TotalCoins;
            int endTotalCoins = startTotalCoins - amount;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                int newTotalCoins = Mathf.RoundToInt(Mathf.Lerp(startTotalCoins, endTotalCoins, factor));
                SetTotalCoins(newTotalCoins);
                yield return null;
            }
        }
    }
}
