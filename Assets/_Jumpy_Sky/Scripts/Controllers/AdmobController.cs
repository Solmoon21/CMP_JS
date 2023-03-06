using System;
using System.Collections;
using UnityEngine;

#if CB_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif

namespace ClawbearGames
{
    public class AdmobController : MonoBehaviour
    {

#if CB_ADMOB


        [Header("Banner ID")]
#if UNITY_ANDROID
        [SerializeField] private string androidBannerID = "ca-app-pub-1064078647772222/9329609006";
#elif UNITY_IOS
        [SerializeField] private string iOSBannerID = "ca-app-pub-1064078647772222/9329609006";
#endif
        [SerializeField] private AdPosition bannerPosition = AdPosition.Bottom;


        [Header("Interstitial Ad ID")]
#if UNITY_ANDROID
        [SerializeField] private string androidInterstitialAdID = "ca-app-pub-1064078647772222/2139808686";
#elif UNITY_IOS
        [SerializeField] private string iOSInterstitialAdID = "ca-app-pub-1064078647772222/2139808686";
#endif

        [Header("Rewarded Ad ID")]
#if UNITY_ANDROID
        [SerializeField] private string androidRewardedAdID = "ca-app-pub-1064078647772222/9919321234";

#elif UNITY_IOS
        [SerializeField] private string iOSRewardedAdID = "ca-app-pub-1064078647772222/9919321234";
#endif

        private BannerView bannerView = null;
        private InterstitialAd interstitialAd = null;
        private RewardedAd rewardedAd = null;
        private bool isInitializeCompleted = false;
        private bool isCompletedRewardedAd = false;

#endif


        private AdRequest CreateAdRequest(string keyword)
        {
            return new AdRequest.Builder()
                .AddKeyword(keyword)
                .Build();
        }



        private void Awake()
        {
#if CB_ADMOB
            MobileAds.SetiOSAppPauseOnBackground(true);
            MobileAds.Initialize((initStatus) => 
            {
                // Callbacks from GoogleMobileAds are not guaranteed to be called on
                // main thread.
                // In this example we use MobileAdsEventExecutor to schedule these calls on
                // the next Update() loop.
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    isInitializeCompleted = true;
                    LoadInterstitialAd();
                    LoadRewardedAd();
                });
            });
#endif
        }


        /// <summary>
        /// Load and show a banner ad.
        /// </summary>
        public void ShowBannerAd(float delay)
        {
            StartCoroutine(CRLoadAndShowBanner(delay));
        }


        /// <summary>
        /// Coroutine load and show banner.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRLoadAndShowBanner(float delay)
        {
            yield return new WaitForSeconds(delay);
#if CB_ADMOB
            while (!isInitializeCompleted)
            {
                yield return null;
            }

            // Clean up banner ad before creating a new one.
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            // Create a 320x50 banner at the top of the screen.
#if UNITY_ANDROID
            bannerView = new BannerView(androidBannerID, AdSize.Banner, bannerPosition);
#elif UNITY_IOS
            bannerView = new BannerView(iOSBannerID, AdSize.SmartBanner, bannerPosition);
#endif
            // Load banner ad.
            bannerView.LoadAd(new AdRequest.Builder().Build());
#endif
        }

        /// <summary>
        /// Hide the current banner ad.
        /// </summary>
        public void HideBanner()
        {
#if CB_ADMOB
            if (bannerView != null)
            {
                bannerView.Hide();
            }
#endif
        }


        /// <summary>
        /// Determine whether the interstitial ad is ready.
        /// </summary>
        /// <returns></returns>
        public bool IsInterstitialAdReady()
        {
#if CB_ADMOB
            if (!isInitializeCompleted)
                return false;

            if (interstitialAd.CanShowAd())
            {
                return true;
            }
            else
            {
                LoadInterstitialAd();
                return false;
            }
#else
            return false;
#endif
        }


        /// <summary>
        /// Show interstitial ad with given delay time.
        /// </summary>
        /// <param name="delay"></param>
        public void ShowInterstitialAd(float delay)
        {
            StartCoroutine(CRShowInterstitialAd(delay));
        }


        /// <summary>
        /// Coroutine show interstitial ad.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRShowInterstitialAd(float delay)
        {
            yield return new WaitForSeconds(delay);
#if CB_ADMOB
            if (interstitialAd.CanShowAd())
            {
                interstitialAd.Show();
            }
            else
            {

                if (!isInitializeCompleted)
                    yield return null;
                InterstitialAd.Load(androidInterstitialAdID, CreateAdRequest("Interstitial"), (InterstitialAd ad, LoadAdError loadAdError)=>{
                    if (loadAdError != null)
                    {
                        return;
                    }
                    else
                    if (ad == null)
                    {
                        return;
                    }

                    interstitialAd = ad;
                });
                // if (isInitializeCompleted)
                //     interstitialAd.LoadAd(new AdRequest.Builder().Build());
            }
#endif
        }



#if CB_ADMOB
        /// <summary>
        /// Load the interstitial ad.
        /// </summary>
        private void LoadInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
            }
#if UNITY_ANDROID
            //interstitialAd = new InterstitialAd(androidInterstitialAdID);
            if (!isInitializeCompleted)
                return;
            InterstitialAd.Load(androidInterstitialAdID, CreateAdRequest("Interstitial"), (InterstitialAd ad, LoadAdError loadAdError)=>{
                if (loadAdError != null)
                {
                    return;
                }
                else
                if (ad == null)
                {
                    return;
                }
                interstitialAd = ad;
            });
#elif UNITY_IOS
                interstitialAd = new InterstitialAd(iOSInterstitialAdID);
#endif
            interstitialAd.OnAdFullScreenContentClosed += HandleOnAdFullScreenContentClosed;
    }

    public void HandleOnAdFullScreenContentClosed()
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            LoadInterstitialAd();
        });
        if (!isInitializeCompleted)
            return;
        InterstitialAd.Load(androidInterstitialAdID, CreateAdRequest("Interstitial"), (InterstitialAd ad, LoadAdError loadAdError)=>{
            if (loadAdError != null)
            {
                return;
            }
            else
            if (ad == null)
            {
                return;
            }
            interstitialAd = ad;
        });
    }
#endif












        /// <summary>
        /// Determine whether the rewarded ad is ready.
        /// </summary>
        /// <returns></returns>
        public bool IsRewardedAdReady()
        {
#if CB_ADMOB
            if (!isInitializeCompleted)
                return false;

            if (rewardedAd.CanShowAd())
            {
                return true;
            }
            else
            {
                LoadRewardedAd();
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// Show rewarded ad with given delay time 
        /// </summary>
        /// <param name="delay"></param>
        public void ShowRewardedAd(float delay)
        {
            StartCoroutine(CRShowRewardedAd(delay));
        }


        /// <summary>
        /// Coroutine show rewarded ad.
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator CRShowRewardedAd(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
#if CB_ADMOB
            if (rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward)=>{
                    Debug.Log("Rewared ads showing");
                });
            }
            else
            {
                LoadRewardedAd();
            }
#endif
        }


#if CB_ADMOB
        /// <summary>
        /// Load the rewarded ad.
        /// </summary>
        private void LoadRewardedAd()
        {
            //Create the singleton rewardedAd.
#if UNITY_ANDROID
            //rewardedAd = new RewardedAd(androidRewardedAdID);
            if (!isInitializeCompleted)
                return;
            RewardedAd.Load(androidRewardedAdID, CreateAdRequest("Reward"), (RewardedAd ad,LoadAdError loadAdError)=>{
                if (loadAdError != null)
                {
                    return;
                }
                else
                if (ad == null)
                {
                    return;
                }
                rewardedAd = ad;
            });
#elif UNITY_IOS
            rewardedAd = new RewardedAd(iOSRewardedAdID);
#endif
            //Register events for rewardedAd
            


            rewardedAd.OnAdFullScreenContentClosed += ()=>{
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    //User closed the video
                    ServicesManager.Instance.AdManager.OnRewardedVideoClosed(isCompletedRewardedAd);
                    isCompletedRewardedAd = false;
                    LoadRewardedAd();
                });
            };
            // rewardedAd.OnAdClosed += (sender, args) =>
            // {
            //     MobileAdsEventExecutor.ExecuteInUpdate(() =>
            //     {
            //         //User closed the video
            //         ServicesManager.Instance.AdManager.OnRewardedVideoClosed(isCompletedRewardedAd);
            //         isCompletedRewardedAd = false;
            //         LoadRewardedAd();
            //     });
            // };

            rewardedAd.OnAdPaid += (AdValue adValue)=>{
                isCompletedRewardedAd = true;
            };

            //rewardedAd.OnUserEarnedReward += (sender, args) =>
            //{
            //    //User watched the whole video
            //    isCompletedRewardedAd = true;
            //};

            if (!isInitializeCompleted)
                return;
            RewardedAd.Load(androidRewardedAdID, CreateAdRequest("Reward"), (RewardedAd ad,LoadAdError loadAdError)=>{
                if (loadAdError != null)
                {
                    return;
                }
                else
                if (ad == null)
                {
                    return;
                }
                rewardedAd = ad;
            });
    }
#endif
    }
}

