using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ClawbearGames
{
    public class IngameManager : MonoBehaviour
    {
        public static IngameManager Instance { private set; get; }
        public static event System.Action<IngameState> IngameStateChanged = delegate { };


        [Header("Enter a number of level to test. Set back to 0 to disable this feature.")]
        [SerializeField] private int testingLevel = 0;



        [Header("Ingame Configuration")]
        [SerializeField] private float reviveWaitTime = 5f;
        [SerializeField] private int initialPlatformAmount = 10;
        [SerializeField] private int jumpAmountToChangeColors = 7;
        [SerializeField] private PlatformColorsConfiguration[] platformColorsConfiguration = null;

        [Header("Level Configuration")]
        [SerializeField] private List<LevelConfiguration> listLevelConfiguration = new List<LevelConfiguration>();


        [Header("Ingame References")]
        [SerializeField] private Material backgroundMaterial = null;
        [SerializeField] private ParticleSystem[] confettiEffects = null;

        public IngameState IngameState
        {
            get { return ingameState; }
            private set
            {
                if (value != ingameState)
                {
                    ingameState = value;
                    IngameStateChanged(ingameState);
                }
            }
        }
        public Vector3 PlatformSize { private set; get; }
        public float ReviveWaitTime { get { return reviveWaitTime; } }
        public int CurrentLevel { private set; get; }
        public bool IsRevived { private set; get; }



        private IngameState ingameState = IngameState.Ingame_GameOver;
        private List<PlatformParams> listPlatformParams = new List<PlatformParams>();
        private LevelConfiguration currentLevelConfigs = null;
        private AudioClip backgroundMusic = null;
        private Vector3 nextPlatformPos = Vector3.zero;
        private int platformParamsIndex = 0;
        private int platformColorsIndex = 0;
        private int jumpCount = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            ServicesManager.Instance.CoinManager.SetCollectedCoins(0);
            StartCoroutine(CRShowViewWithDelay(ViewType.INGAME_VIEW, 0f));

            //Setup variables
            IsRevived = false;
            platformColorsIndex = Random.Range(0, platformColorsConfiguration.Length);
            nextPlatformPos = PlayerController.Instance.transform.position;
            confettiEffects[0].transform.root.gameObject.SetActive(false);

            //Load level parameters
            CurrentLevel = (testingLevel != 0) ? testingLevel : PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1);
            foreach(LevelConfiguration levelConfigs in listLevelConfiguration)
            {
                if (CurrentLevel >= levelConfigs.MinLevel && CurrentLevel < levelConfigs.MaxLevel)
                {
                    //Setup background and others parameters
                    currentLevelConfigs = levelConfigs;
                    backgroundMusic = levelConfigs.BackgroundMusicClip;
                    backgroundMaterial.SetColor("_TopColor", levelConfigs.BackgroundTopColor);
                    backgroundMaterial.SetColor("_BottomColor", levelConfigs.BackgroundBottomColor);
                    PlayerController.Instance.SetMovementSpeed(Random.Range(levelConfigs.MinPlayerMovementSpeed, levelConfigs.MaxPlayerMovementSpeed));

                    int platformAmount = Random.Range(levelConfigs.MinPlatformAmount, levelConfigs.MaxPlatformAmount);
                    for (int a = 0; a < platformAmount; a++)
                    {
                        PlatformParams platformParams = new PlatformParams();
                        platformParams.SetPlatformType(GetPlatformType());
                        platformParams.SetPlatformSize(GetPlatformSize());
                        platformParams.SetPlatformDistance(Random.Range(levelConfigs.MinPlatformDistance, levelConfigs.MaxPlatformDistance));
                        platformParams.SetHorizontalDeviation(Random.Range(levelConfigs.MinHorizontalDevitation, levelConfigs.MaxHorizontalDevitation));
                        platformParams.SetMovableFrequency(levelConfigs.MovableFrequency);
                        platformParams.SetMovementSpeed(Random.Range(levelConfigs.MinMovementSpeed, levelConfigs.MaxMovementSpeed));
                        platformParams.SetLerpType(levelConfigs.LerpType);
                        platformParams.SetCoinItemFrequency(levelConfigs.CoinItemFrequency);
                        platformParams.SetCoinItemAmount(Random.Range(levelConfigs.MinCoinItemAmount, levelConfigs.MaxCoinItemAmount));
                        platformParams.SetMagnetItemFrequency(levelConfigs.MagnetItemFrequency);
                        platformParams.SetShieldItemFrequency(levelConfigs.ShieldItemFrequency);
                        platformParams.SetObstacleFrequency(levelConfigs.ObstacleFrequency);
                        platformParams.SetEmptyPlatform((a <= 2 || a == platformAmount - 1) ? true : false);
                        listPlatformParams.Add(platformParams);
                    }

                    break;
                }
            }


            for(int i = 0; i < initialPlatformAmount; i++)
            {
                CreateNextPlatform();
            }

            if (Utilities.IsShowTutorial())
                Invoke(nameof(PlayingGame), 0.15f);
        }

        /// <summary>
        /// Call IngameState.Ingame_Playing event and handle other actions.
        /// Actual start the game.
        /// </summary>
        public void PlayingGame()
        {
            //Fire event
            IngameState = IngameState.Ingame_Playing;
            ingameState = IngameState.Ingame_Playing;

            //Other actions
            if (IsRevived)
            {
                StartCoroutine(CRShowViewWithDelay(ViewType.INGAME_VIEW, 0f));
                ServicesManager.Instance.SoundManager.ResumeMusic(0.5f);
            }
            else
            {
                ServicesManager.Instance.SoundManager.PlayMusic(backgroundMusic, 0.5f);
            }
        }


        /// <summary>
        /// Call IngameState.Ingame_Revive event and handle other actions.
        /// </summary>
        public void Revive()
        {
            //Fire event
            IngameState = IngameState.Ingame_Revive;
            ingameState = IngameState.Ingame_Revive;

            //Add another actions here
            StartCoroutine(CRShowViewWithDelay(ViewType.REVIVE_VIEW, 1f));
            ServicesManager.Instance.SoundManager.PauseMusic(0.5f);
        }


        /// <summary>
        /// Call IngameState.Ingame_GameOver event and handle other actions.
        /// </summary>
        public void GameOver()
        {
            //Fire event
            IngameState = IngameState.Ingame_GameOver;
            ingameState = IngameState.Ingame_GameOver;

            //Add another actions here
            StartCoroutine(CRShowViewWithDelay(ViewType.ENDGAME_VIEW, 0.25f));
            ServicesManager.Instance.SoundManager.StopMusic(0.5f);
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.LevelFailed);
        }


        /// <summary>
        /// Call IngameState.Ingame_CompleteLevel event and handle other actions.
        /// </summary>
        public void CompletedLevel()
        {
            //Fire event
            IngameState = IngameState.Ingame_CompleteLevel;
            ingameState = IngameState.Ingame_CompleteLevel;

            //Other actions
            StartCoroutine(CRShowViewWithDelay(ViewType.ENDGAME_VIEW, 1f));
            ServicesManager.Instance.SoundManager.StopMusic(0.5f);
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.LevelCompleted);
            foreach (ParticleSystem o in confettiEffects)
            {
                o.Play();
            }

            //Save level
            if (testingLevel == 0)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL) + 1);

                //Report level to leaderboard
                string username = PlayerPrefs.GetString(PlayerPrefsKeys.PPK_SAVED_USER_NAME);
                if (!string.IsNullOrEmpty(username))
                {
                    ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();
                }
            }
        }


        /// <summary>
        /// Get the PlatformType based on level configs.
        /// </summary>
        /// <returns></returns>
        private PlatformType GetPlatformType()
        {
            //Calculate the total frequency
            float totalFreq = 0;
            foreach (PlatformTypeConfiguration configuration in currentLevelConfigs.ListPlatformTypeConfiguration)
            {
                totalFreq += configuration.Frequency;
            }

            float randomFreq = Random.Range(0, totalFreq);
            for (int i = 0; i < currentLevelConfigs.ListPlatformTypeConfiguration.Count; i++)
            {
                if (randomFreq < currentLevelConfigs.ListPlatformTypeConfiguration[i].Frequency)
                {
                    return currentLevelConfigs.ListPlatformTypeConfiguration[i].PlatformType;
                }
                else
                {
                    randomFreq -= currentLevelConfigs.ListPlatformTypeConfiguration[i].Frequency;
                }
            }

            return currentLevelConfigs.ListPlatformTypeConfiguration[0].PlatformType;
        }



        /// <summary>
        /// Get the PlatformSize based on level configs.
        /// </summary>
        /// <returns></returns>
        private PlatformSize GetPlatformSize()
        {
            //Calculate the total frequency
            float totalFreq = 0;
            foreach (PlatformSizeConfiguration configuration in currentLevelConfigs.ListPlatformSizeConfiguration)
            {
                totalFreq += configuration.Frequency;
            }

            float randomFreq = Random.Range(0, totalFreq);
            for (int i = 0; i < currentLevelConfigs.ListPlatformSizeConfiguration.Count; i++)
            {
                if (randomFreq < currentLevelConfigs.ListPlatformSizeConfiguration[i].Frequency)
                {
                    return currentLevelConfigs.ListPlatformSizeConfiguration[i].PlatformSize;
                }
                else
                {
                    randomFreq -= currentLevelConfigs.ListPlatformSizeConfiguration[i].Frequency;
                }
            }

            return currentLevelConfigs.ListPlatformSizeConfiguration[0].PlatformSize;
        }


        /// <summary>
        /// Coroutine show the view with given viewType and delay time.
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRShowViewWithDelay(ViewType viewType, float delay)
        {
            yield return new WaitForSeconds(delay);
            ViewManager.Instance.OnShowView(viewType);
        }



        //////////////////////////////////////Publish functions



        /// <summary>
        /// Continue the game
        /// </summary>
        public void SetContinueGame()
        {
            IsRevived = true;
            Invoke(nameof(PlayingGame), 0.05f);
        }



        /// <summary>
        /// Handle actions when player died.
        /// </summary>
        public void HandlePlayerDied()
        {
            if (IsRevived || !ServicesManager.Instance.AdManager.IsRewardedVideoAdReady())
            {
                //Fire event
                IngameState = IngameState.Ingame_GameOver;
                ingameState = IngameState.Ingame_GameOver;

                //Add another actions here
                StartCoroutine(CRShowViewWithDelay(ViewType.ENDGAME_VIEW, 1f));
                ServicesManager.Instance.SoundManager.StopMusic(0.5f);
                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.LevelFailed);
            }
            else
            {
                Revive();
            }
        }



        /// <summary>
        /// Get a platform's position that higher than the player.
        /// </summary>
        /// <returns></returns>
        public float GetHigherPlatformPoint()
        {
            float minDis = 10000;
            Vector3 resultPoint = PlayerController.Instance.transform.position;
            List<PlatformController> listActivePlatform = PoolManager.Instance.GetActivePlatforms();
            foreach(PlatformController platform in listActivePlatform) 
            {            
                if (platform.transform.position.y > PlayerController.Instance.transform.position.y + 1f)
                {
                    float currentDis = platform.transform.position.y - PlayerController.Instance.transform.position.y;
                    if (currentDis < minDis)
                    {
                        minDis = currentDis;
                        resultPoint = platform.transform.position;
                    }
                }
            }

            return resultPoint.y;
        }



        /// <summary>
        /// Determine the given y axis is the highest y axis of all platforms.
        /// </summary>
        /// <param name="platformYAxis"></param>
        /// <returns></returns>
        public bool IsHighestYAxis(float platformYAxis)
        {
            List<PlatformController> listActivePlatform = PoolManager.Instance.GetActivePlatforms();
            foreach (PlatformController platform in listActivePlatform)
            {
                if (platform.transform.position.y > platformYAxis + 1f)
                {
                    return false;
                }
            }

            return true;
        }



        /// <summary>
        /// Increase jump count to change platform's colors.
        /// </summary>
        public void IncreaseJumpCount()
        {
            jumpCount++;
            if (jumpCount == jumpAmountToChangeColors)
            {
                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.ChangedColors);

                jumpCount = 0;
                platformColorsIndex = (platformColorsIndex + 1 == platformColorsConfiguration.Length) ? 0 : platformColorsIndex + 1;
                List<PlatformController> listActivePlatform = PoolManager.Instance.GetActivePlatforms();
                List<PlatformController> listSorted = new List<PlatformController>();

                while (listSorted.Count < listActivePlatform.Count)
                {
                    float minDis = 1000;
                    PlatformController lowestPlatform = null;
                    foreach(PlatformController platform in listActivePlatform) 
                    {
                        float platformY = platform.transform.position.y;
                        if(!listSorted.Contains(platform) && platform.transform.position.y - PlayerController.Instance.TargetY < minDis)
                        {
                            minDis = platform.transform.position.y - PlayerController.Instance.TargetY;
                            lowestPlatform = platform;
                        }
                    }
                    listSorted.Add(lowestPlatform);
                }

                for (int i = 0; i < listSorted.Count; i++)
                {
                    if (listSorted[i].gameObject.activeSelf)
                    {
                        listSorted[i].ChangeColor(platformColorsConfiguration[platformColorsIndex], i * 0.1f);
                    }
                }
            }
        }




        /// <summary>
        /// Create next platform.
        /// </summary>
        public void CreateNextPlatform()
        {
            if (platformParamsIndex == listPlatformParams.Count && !confettiEffects[0].transform.root.gameObject.activeSelf)
            {
                //Enable finish platform
                confettiEffects[0].transform.root.position = nextPlatformPos + Vector3.down * listPlatformParams[listPlatformParams.Count - 1].PlatformDistance;
                confettiEffects[0].transform.root.gameObject.SetActive(true);
            }
            else if (platformParamsIndex < listPlatformParams.Count)
            {
                //Create the platform
                PlatformParams platformParams = listPlatformParams[platformParamsIndex];
                PlatformController currentPlatform = PoolManager.Instance.GetPlatformController(platformParams.PlatformType, platformParams.PlatformSize);
                currentPlatform.transform.position = nextPlatformPos;
                currentPlatform.gameObject.SetActive(true);
                currentPlatform.SetColors(platformColorsConfiguration[platformColorsIndex]);
                currentPlatform.OnSetup(platformParams);
                nextPlatformPos = currentPlatform.transform.position + Vector3.up * platformParams.PlatformDistance;

                //Update paramaters
                platformParamsIndex++;
            }
        }
    }
}
