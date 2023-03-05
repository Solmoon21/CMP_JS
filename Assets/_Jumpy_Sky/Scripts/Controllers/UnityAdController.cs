using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace ClawbearGames
{
    // public class UnityAdController : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    // {
    //     [SerializeField] private string unityAdGameID = "1611450";

    //     [Space(10)]
    //     [SerializeField] private string bannerAdID = "Android_Banner";
    //     [SerializeField] private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;


    //     [Space(10)]
    //     [SerializeField] private string interstitialAdID = "Android_Interstitial";

    //     [Space(10)]
    //     [SerializeField] private string rewardedVideoAdID = "Android_Rewarded";

    //     [Space(10)]
    //     [SerializeField] private bool enableTestMode = false;


    //     private bool isInterstitialAdReady = false;
    //     private bool isRewardedVideoAdReady = false;

    //     private void Start()
    //     {
    //         isInterstitialAdReady = false;
    //         isRewardedVideoAdReady = false;
    //         Advertisement.Initialize(unityAdGameID, enableTestMode, this);
    //     }


    //     /// <summary>
    //     /// Show the banner ad with given delay time
    //     /// </summary>
    //     public void ShowBanner(float delay)
    //     {
    //         StartCoroutine(CRShowBanner(delay));
    //     }



    //     /// <summary>
    //     /// Hide the current banner
    //     /// </summary>
    //     public void HideBanner()
    //     {
    //         Advertisement.Banner.Hide();
    //     }



    //     /// <summary>
    //     /// Coroutine wait for banner ready then show.
    //     /// </summary>
    //     /// <param name="delay"></param>
    //     /// <returns></returns>
    //     private IEnumerator CRShowBanner(float delay)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         float timer = 0;
    //         while (!Advertisement.Banner.isLoaded)
    //         {
    //             yield return null;
    //             timer += Time.deltaTime;
    //             if (timer >= 5f)
    //             {
    //                 timer = 0;
    //                 Advertisement.Banner.Load(bannerAdID);
    //             }
    //         }

    //         Advertisement.Banner.SetPosition(bannerPosition);
    //         Advertisement.Banner.Show(bannerAdID);
    //     }








    //     /// <summary>
    //     /// Determine whether the interstitial ad is ready
    //     /// </summary>
    //     /// <returns></returns>
    //     public bool IsInterstitialAdReady()
    //     {
    //         return isInterstitialAdReady;
    //     }


    //     /// <summary>
    //     /// Show interstitial ad given given delay time
    //     /// </summary>
    //     /// <param name="delay"></param>
    //     public void ShowInterstitialAd(float delay)
    //     {
    //         StartCoroutine(CRShowInterstitial(delay));
    //     }


    //     /// <summary>
    //     /// Coroutine show interstitial ad.
    //     /// </summary>
    //     /// <param name="delay"></param>
    //     /// <returns></returns>
    //     private IEnumerator CRShowInterstitial(float delay)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         Advertisement.Show(interstitialAdID, this);
    //     }








    //     /// <summary>
    //     /// Determine whether the rewarded video ad is ready
    //     /// </summary>
    //     /// <returns></returns>
    //     public bool IsRewardedAdReady()
    //     {
    //         return isRewardedVideoAdReady;
    //     }

    //     /// <summary>
    //     /// Show rewarded video with given delay time
    //     /// </summary>
    //     /// <param name="delay"></param>
    //     public void ShowRewardedAd(float delay)
    //     {
    //         StartCoroutine(CRShowRewardedAd(delay));
    //     }


    //     /// <summary>
    //     /// Coroutine show the rewarded ad with delay.
    //     /// </summary>
    //     /// <param name="delay"></param>
    //     /// <returns></returns>
    //     private IEnumerator CRShowRewardedAd(float delay)
    //     {
    //         yield return new WaitForSeconds(delay);
    //         Advertisement.Show(rewardedVideoAdID, this);
    //     }

    //     ////////////////////////////////////////////////// Callbacks

    //     public void OnInitializationComplete() 
    //     {
    //         Advertisement.Load(interstitialAdID, this);
    //         Advertisement.Load(rewardedVideoAdID, this);
    //     }

    //     public void OnInitializationFailed(UnityAdsInitializationError error, string message) 
    //     {
    //         Advertisement.Initialize(unityAdGameID, enableTestMode, this);
    //     }

    //     public void OnUnityAdsAdLoaded(string placementId)
    //     {
    //         if (placementId.Equals(interstitialAdID))
    //         {
    //             isInterstitialAdReady = true;
    //         }
    //         else
    //         {
    //             isRewardedVideoAdReady = true;
    //         }
    //     }

    //     public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    //     {
    //         if (placementId.Equals(interstitialAdID))
    //         {
    //             isInterstitialAdReady = false;
    //         }
    //         else
    //         {
    //             isRewardedVideoAdReady = false;
    //         }

    //         Advertisement.Load(interstitialAdID, this);
    //         Advertisement.Load(rewardedVideoAdID, this);
    //     }






    //     ////////////////////////////////////////////////// Show Callback

    //     public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    //     {
    //         isInterstitialAdReady = false;
    //         isRewardedVideoAdReady = false;
    //         if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
    //         {
    //             if (placementId.Equals(rewardedVideoAdID))
    //             {
    //                 ServicesManager.Instance.AdManager.OnRewardedVideoClosed(true);
    //                 Advertisement.Load(rewardedVideoAdID, this);
    //             }
    //             else
    //             {
    //                 Advertisement.Load(interstitialAdID, this);
    //             }
    //         }
    //         else
    //         {
    //             if (placementId.Equals(rewardedVideoAdID))
    //             {
    //                 ServicesManager.Instance.AdManager.OnRewardedVideoClosed(false);
    //                 Advertisement.Load(rewardedVideoAdID, this);
    //             }
    //             else
    //             {
    //                 Advertisement.Load(interstitialAdID, this);
    //             }
    //         }
    //     }
    //     public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    //     {
    //         isInterstitialAdReady = false;
    //         isRewardedVideoAdReady = false;
    //         if (placementId.Equals(rewardedVideoAdID))
    //         {
    //             Advertisement.Load(rewardedVideoAdID, this);
    //         }
    //         else
    //         {
    //             Advertisement.Load(interstitialAdID, this);
    //         }
    //     }

    //     public void OnUnityAdsShowStart(string placementId) { }

    //     public void OnUnityAdsShowClick(string placementId) { }
    // }
}