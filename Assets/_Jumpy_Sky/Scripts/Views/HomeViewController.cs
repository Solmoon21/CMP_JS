﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ClawbearGames
{
    public class HomeViewController : BaseViewController
    {
        [SerializeField] private RectTransform topPanelTrans = null;
        [SerializeField] private RectTransform bottomPanelTrans = null;
        [SerializeField] private RectTransform gameNameTrans = null;
        [SerializeField] private RectTransform playButtonTrans = null;
        [SerializeField] private RectTransform shareButtonTrans = null;
        [SerializeField] private RectTransform soundButtonsTrans = null;
        [SerializeField] private RectTransform musicButtonsTrans = null;
        [SerializeField] private RectTransform leaderboardButtonTrans = null;
        [SerializeField] private RectTransform rateAppButtonTrans = null;
        [SerializeField] private RectTransform upgradeButtonTrans = null;
        [SerializeField] private RectTransform soundOnButtonTrans = null;
        [SerializeField] private RectTransform soundOffButtonTrans = null;
        [SerializeField] private RectTransform musicOnButtonTrans = null;
        [SerializeField] private RectTransform musicOffButtonTrans = null;
        [SerializeField] private RectTransform warningSignTrans = null;
        [SerializeField] private Text currentLevelText = null;
        [SerializeField] private Text totalCoinsText = null;
        private int settingButtonTurn = 1;

        private void Update()
        {
            totalCoinsText.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
        }


        /// <summary>
        /// Coroutine on click setting button.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CROnClickSettingButton()
        {
            if (settingButtonTurn == -1)
            {
                MoveRectTransform(shareButtonTrans, shareButtonTrans.anchoredPosition, new Vector2(0, shareButtonTrans.anchoredPosition.y), 0.5f);
                MoveRectTransform(leaderboardButtonTrans, leaderboardButtonTrans.anchoredPosition, new Vector2(0, leaderboardButtonTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                MoveRectTransform(soundButtonsTrans, soundButtonsTrans.anchoredPosition, new Vector2(0, soundButtonsTrans.anchoredPosition.y), 0.5f);
                MoveRectTransform(rateAppButtonTrans, rateAppButtonTrans.anchoredPosition, new Vector2(0, rateAppButtonTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                MoveRectTransform(musicButtonsTrans, musicButtonsTrans.anchoredPosition, new Vector2(0, musicButtonsTrans.anchoredPosition.y), 0.5f);
                MoveRectTransform(upgradeButtonTrans, upgradeButtonTrans.anchoredPosition, new Vector2(0, upgradeButtonTrans.anchoredPosition.y), 0.5f);
            }
            else
            {
                MoveRectTransform(shareButtonTrans, shareButtonTrans.anchoredPosition, new Vector2(-200, shareButtonTrans.anchoredPosition.y), 0.5f);
                MoveRectTransform(leaderboardButtonTrans, leaderboardButtonTrans.anchoredPosition, new Vector2(200, leaderboardButtonTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                MoveRectTransform(soundButtonsTrans, soundButtonsTrans.anchoredPosition, new Vector2(-200, soundButtonsTrans.anchoredPosition.y), 0.5f);
                MoveRectTransform(rateAppButtonTrans, rateAppButtonTrans.anchoredPosition, new Vector2(200, rateAppButtonTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                MoveRectTransform(musicButtonsTrans, musicButtonsTrans.anchoredPosition, new Vector2(-200, musicButtonsTrans.anchoredPosition.y), 0.5f);
                MoveRectTransform(upgradeButtonTrans, upgradeButtonTrans.anchoredPosition, new Vector2(200, upgradeButtonTrans.anchoredPosition.y), 0.5f);
            }
        }



        /// <summary>
        ////////////////////////////////////////////////// Public Functions
        /// </summary>


        public override void OnShow()
        {
            MoveRectTransform(topPanelTrans, topPanelTrans.anchoredPosition, new Vector2(topPanelTrans.anchoredPosition.x, 0f), 0.5f);
            MoveRectTransform(bottomPanelTrans, bottomPanelTrans.anchoredPosition, new Vector2(bottomPanelTrans.anchoredPosition.x, 0f), 0.75f);
            ScaleRectTransform(gameNameTrans, Vector2.zero, Vector2.one, 1f);
            ScaleRectTransform(playButtonTrans, Vector2.zero, Vector2.one, 0.5f);

            settingButtonTurn = 1;
            currentLevelText.text = "LEVEL: " + PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1).ToString();

            //Update sound buttons
            if (ServicesManager.Instance.SoundManager.IsSoundOff())
            {
                soundOnButtonTrans.gameObject.SetActive(false);
                soundOffButtonTrans.gameObject.SetActive(true);
            }
            else
            {
                soundOnButtonTrans.gameObject.SetActive(true);
                soundOffButtonTrans.gameObject.SetActive(false);
            }

            //Update music buttons
            if (ServicesManager.Instance.SoundManager.IsMusicOff())
            {
                musicOffButtonTrans.gameObject.SetActive(true);
                musicOnButtonTrans.gameObject.SetActive(false);
            }
            else
            {
                musicOffButtonTrans.gameObject.SetActive(false);
                musicOnButtonTrans.gameObject.SetActive(true);
            }


            //Handle warning sign
            if (ServicesManager.Instance.AdManager.IsRewardedVideoAdReady())
            {
                warningSignTrans.gameObject.SetActive(true);
            }
            else
            {
                int timeRemains = ServicesManager.Instance.DailyRewardManager.TimeRemainsTillNextReward();
                warningSignTrans.gameObject.SetActive((timeRemains == 0) ? true : false);
            }
        }


        public override void OnClose()
        {
            topPanelTrans.anchoredPosition = new Vector2(topPanelTrans.anchoredPosition.x, 200f);
            bottomPanelTrans.anchoredPosition = new Vector2(bottomPanelTrans.anchoredPosition.x, -500f);
            gameNameTrans.localScale = Vector2.zero;
            playButtonTrans.localScale = Vector3.zero;

            shareButtonTrans.anchoredPosition = new Vector2(-200, shareButtonTrans.anchoredPosition.y);
            soundButtonsTrans.anchoredPosition = new Vector2(-200, soundButtonsTrans.anchoredPosition.y);
            musicButtonsTrans.anchoredPosition = new Vector2(-200, musicButtonsTrans.anchoredPosition.y);
            leaderboardButtonTrans.anchoredPosition = new Vector2(200, leaderboardButtonTrans.anchoredPosition.y);
            rateAppButtonTrans.anchoredPosition = new Vector2(200, rateAppButtonTrans.anchoredPosition.y);
            upgradeButtonTrans.anchoredPosition = new Vector2(200, upgradeButtonTrans.anchoredPosition.y);
            gameObject.SetActive(false);
        }


        /// <summary>
        ////////////////////////////////////////////////// UI Buttons
        /// </summary>


        public void OnClickPlayButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            LoadScene("Ingame", 0.25f);
        }


        public void OnClickRewardButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            ViewManager.Instance.OnShowView(ViewType.DAILY_REWARD_VIEW);
        }


        public void OnClickCharacterButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            LoadScene("Character", 0.25f);
        }

        public void OnClickSettingButton()
        {
            settingButtonTurn *= -1;
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            StartCoroutine(CROnClickSettingButton());
        }


        public void OnClickShareButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            ServicesManager.Instance.ShareManager.NativeShare();
        }

        public void OnClickSoundButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            ServicesManager.Instance.SoundManager.ToggleSound();
            if (ServicesManager.Instance.SoundManager.IsSoundOff())
            {
                soundOnButtonTrans.gameObject.SetActive(false);
                soundOffButtonTrans.gameObject.SetActive(true);
            }
            else
            {
                soundOnButtonTrans.gameObject.SetActive(true);
                soundOffButtonTrans.gameObject.SetActive(false);
            }
        }

        public void OnClickMusicButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            ServicesManager.Instance.SoundManager.ToggleMusic();
            if (ServicesManager.Instance.SoundManager.IsMusicOff())
            {
                musicOffButtonTrans.gameObject.SetActive(true);
                musicOnButtonTrans.gameObject.SetActive(false);
            }
            else
            {
                musicOffButtonTrans.gameObject.SetActive(false);
                musicOnButtonTrans.gameObject.SetActive(true);
            }
        }



        public void OnClickLeaderboardButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            ViewManager.Instance.OnShowView(ViewType.LEADERBOARD_VIEW);
        }

        public void OnClickRateAppButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            Application.OpenURL(ServicesManager.Instance.ShareManager.AppUrl);
        }
        public void OnClickRemoveAdsButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
        }
    }
}
