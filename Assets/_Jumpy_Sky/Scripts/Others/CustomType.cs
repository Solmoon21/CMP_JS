using System.Collections.Generic;
using UnityEngine;

namespace ClawbearGames
{

    #region --------------------Ingame enums
    public enum IngameState
    {
        Ingame_Playing,
        Ingame_Revive,
        Ingame_GameOver,
        Ingame_CompleteLevel,
    }

    public enum PlayerState
    {
        Player_Prepare,
        Player_Living,
        Player_Died,
        Player_CompletedLevel,
    }


    public enum ItemType
    {
        COIN = 0,
        MAGNET = 1,
        SHIELD = 2,
    }

    public enum PlatformType
    {
        SQUARE = 0,
        HEXAGON = 1,
        HEPTAGON = 2,
        OCTAGON = 3,
        CIRCLE = 4,
    }

    public enum PlatformSize
    {
        SMALL = 0,
        NORMAL = 1,
        BIG = 2,
    }


    public enum DayType
    {
        DAY_1 = 0,
        DAY_2 = 1,
        DAY_3 = 2,
        DAY_4 = 3,
        DAY_5 = 4,
        DAY_6 = 5,
        DAY_7 = 6,
        DAY_8 = 7,
        DAY_9 = 8,
    }

    #endregion



    #region --------------------Ads enums
    public enum BannerAdType
    {
        NONE,
        ADMOB,
        UNITY,
    }

    public enum InterstitialAdType
    {
        UNITY,
        ADMOB,
    }


    public enum RewardedAdType
    {
        UNITY,
        ADMOB,
    }
    #endregion



    #region --------------------View Enums
    public enum ViewType
    {
        HOME_VIEW = 0,
        LEADERBOARD_VIEW = 1,
        DAILY_REWARD_VIEW = 2,
        LOADING_VIEW = 3,
        INGAME_VIEW = 4,
        REVIVE_VIEW = 5,
        ENDGAME_VIEW = 6,
        CHARACTER_VIEW = 7,
        UPGRADE_VIEW = 8,
    }

    #endregion



    #region --------------------Classes

    [System.Serializable]
    public class LevelConfiguration
    {
        [Header("Level Number Configuration")]
        [SerializeField] private int minLevel = 1;
        public int MinLevel => minLevel;
        [SerializeField] private int maxLevel = 1;
        public int MaxLevel => maxLevel;



        [Header("Background Configuration")]
        [SerializeField] private Color backgroundTopColor = Color.white;
        public Color BackgroundTopColor => backgroundTopColor;
        [SerializeField] private Color backgroundBottomColor = Color.white;
        public Color BackgroundBottomColor => backgroundBottomColor;
        [SerializeField] private AudioClip backgroundMusicClip = null;
        public AudioClip BackgroundMusicClip => backgroundMusicClip;


        [Header("Player Parameters Configuration")]
        [SerializeField][Range(1, 200)] private int minPlayerMovementSpeed = 15;
        public int MinPlayerMovementSpeed { get { return minPlayerMovementSpeed; } }
        [SerializeField][Range(1, 200)] private int maxPlayerMovementSpeed = 35;
        public int MaxPlayerMovementSpeed { get { return maxPlayerMovementSpeed; } }


        [Header("Items Configuration")]
        [SerializeField][Range(0f, 1f)] private float coinItemFrequency = 0.1f;
        public float CoinItemFrequency => coinItemFrequency;
        [SerializeField][Range(1, 20)] private int minCoinItemAmount = 1;
        public int MinCoinItemAmount => minCoinItemAmount;
        [SerializeField][Range(1, 20)] private int maxCoinItemAmount = 5;
        public int MaxCoinItemAmount => maxCoinItemAmount;
        [SerializeField][Range(0f, 1f)] private float magnetItemFrequency = 0.1f;
        public float MagnetItemFrequency => magnetItemFrequency;
        [SerializeField][Range(0f, 1f)] private float shieldItemFrequency = 0.1f;
        public float ShieldItemFrequency => shieldItemFrequency;

        [Header("Platforms Configuration")]
        [SerializeField] private int minPlatformAmount = 30;
        public int MinPlatformAmount => minPlatformAmount;
        [SerializeField] private int maxPlatformAmount = 60;
        public int MaxPlatformAmount => maxPlatformAmount;
        [SerializeField] private int minPlatformDistance = 4;
        public int MinPlatformDistance => minPlatformDistance;
        [SerializeField] private int maxPlatformDistance = 7;
        public int MaxPlatformDistance => maxPlatformDistance;
        [SerializeField] private float minHorizontalDevitation = -2f;
        public float MinHorizontalDevitation => minHorizontalDevitation;
        [SerializeField] private float maxHorizontalDevitation = 2f;
        public float MaxHorizontalDevitation => maxHorizontalDevitation;


        [SerializeField][Range(0f, 1f)] private float movableFrequency = 0.1f;
        public float MovableFrequency => movableFrequency;
        [SerializeField] private float minMovementSpeed = 1f;
        public float MinMovementSpeed => minMovementSpeed;
        [SerializeField] private float maxMovementSpeed = 3f;
        public float MaxMovementSpeed => maxMovementSpeed;
        [SerializeField] private LerpType lerpType = LerpType.Liner;
        public LerpType LerpType => lerpType;

        [SerializeField][Range(0f, 1f)] private float obstacleFrequency = 0.1f;
        public float ObstacleFrequency => obstacleFrequency;


        [SerializeField] private List<PlatformTypeConfiguration> listPlatformTypeConfiguration = new List<PlatformTypeConfiguration>();
        public List<PlatformTypeConfiguration> ListPlatformTypeConfiguration => listPlatformTypeConfiguration;

        [SerializeField] private List<PlatformSizeConfiguration> listPlatformSizeConfiguration = new List<PlatformSizeConfiguration>();
        public List<PlatformSizeConfiguration> ListPlatformSizeConfiguration => listPlatformSizeConfiguration;
    }


    [System.Serializable]
    public class PlatformTypeConfiguration
    {
        [SerializeField] private PlatformType platformType = PlatformType.SQUARE;
        public PlatformType PlatformType => platformType;

        [SerializeField][Range(0f, 1f)] private float frequency = 0.1f;
        public float Frequency => frequency;  
    }

    [System.Serializable]
    public class PlatformSizeConfiguration
    {
        [SerializeField] private PlatformSize platformSize = PlatformSize.SMALL;
        public PlatformSize PlatformSize => platformSize;

        [SerializeField][Range(0f, 1f)] private float frequency = 0.1f;
        public float Frequency => frequency;      
    }


    [System.Serializable]
    public class PlatformColorsConfiguration
    {
        [SerializeField] private Color platformColor = Color.white;
        public Color PlatformColor => platformColor;
        [SerializeField] private Color platformInnerColor = Color.white;
        public Color PlatformInnerColor => platformInnerColor;
    }


    [System.Serializable]
    public class PlatformPrefabConfiguration
    {
        [SerializeField] private PlatformType platformType = PlatformType.SQUARE;
        public PlatformType PlatformType => platformType;
        [SerializeField] private PlatformController[] platformControllerPrefabs = null;
        public PlatformController[] PlatformControllerPrefabs => platformControllerPrefabs;
    }



    [System.Serializable]
    public class DailyRewardConfiguration
    {
        [SerializeField] private DayType dayType = DayType.DAY_1;

        /// <summary>
        /// the day type of this DailyRewardItem.
        /// </summary>
        public DayType DayType { get { return dayType; } }


        [SerializeField] private int coinAmount = 0;


        /// <summary>
        /// The amount of coins reward to player.
        /// </summary>
        public int CoinAmount { get { return coinAmount; } }
    }


    [System.Serializable]
    public class InterstitialAdConfiguration
    {
        [SerializeField] private IngameState ingameStateWhenShowingAd = IngameState.Ingame_CompleteLevel;
        public IngameState IngameStateWhenShowingAd { get { return ingameStateWhenShowingAd; } }

        [SerializeField] private int ingameStateAmountWhenShowingAd = 3;
        public int IngameStateAmountWhenShowingAd { get { return ingameStateAmountWhenShowingAd; } }


        [SerializeField] private float delayTimeWhenShowingAd = 2f;
        public float DelayTimeWhenShowingAd { get { return delayTimeWhenShowingAd; } }

        [SerializeField] private List<InterstitialAdType> listInterstitialAdType = new List<InterstitialAdType>();
        public List<InterstitialAdType> ListInterstitialAdType { get { return listInterstitialAdType; } }
    }


    public class PlatformParams 
    {
        public bool IsEmptyPlatform { private set; get; }
        public void SetEmptyPlatform(bool isEmptyPlatform)
        {
            IsEmptyPlatform = isEmptyPlatform;
        }


        public PlatformType PlatformType { private set; get; }
        public void SetPlatformType(PlatformType platformType)
        {
            PlatformType = platformType;
        }

        public PlatformSize PlatformSize { private set; get; }
        public void SetPlatformSize(PlatformSize platformSize)
        {
            PlatformSize = platformSize;
        }

        public int PlatformDistance { private set; get; }
        public void SetPlatformDistance(int platformDistance)
        {
            PlatformDistance = platformDistance;
        }


        public float HorizontalDeviation { private set; get; }
        public void SetHorizontalDeviation(float horizontalDeviation)
        {
            HorizontalDeviation = horizontalDeviation;
        }

        public float CoinItemFrequency { private set; get; }
        public void SetCoinItemFrequency(float frequency)
        {
            CoinItemFrequency = frequency;
        }


        public int CoinItemAmount { private set; get; }
        public void SetCoinItemAmount(int amount)
        {
            CoinItemAmount = amount;
        }


        public float MagnetItemFrequency { private set; get; }
        public void SetMagnetItemFrequency(float frequency)
        {
            MagnetItemFrequency = frequency;
        }

        public float ShieldItemFrequency { private set; get; }
        public void SetShieldItemFrequency(float frequency)
        {
            ShieldItemFrequency = frequency;
        }

        public float ObstacleFrequency { private set; get; }
        public void SetObstacleFrequency(float frequency)
        {
            ObstacleFrequency = frequency;
        }


        public float MovebleFrequency { private set; get; }
        public void SetMovableFrequency(float frequency)
        {
            MovebleFrequency = frequency;
        }

        public float MovementSpeed { private set; get; }
        public void SetMovementSpeed(float speed)
        {
            MovementSpeed = speed;
        }


        public LerpType LerpType { private set; get; }
        public void SetLerpType(LerpType lerpType)
        {
            LerpType = lerpType;
        }
    }


    public class ItemParams
    {
        public int CoinItemAmount { private set; get; }
        public void SetCoinItemAmount(int coinItemAmount)
        {
            CoinItemAmount = coinItemAmount;
        }


        public float MagnetItemFrequency { private set; get; }
        public void SetMagnetItemFrequency(float magnetItemFrequency)
        {
            MagnetItemFrequency = magnetItemFrequency;
        }

        public float ShieldItemFrequency { private set; get; }
        public void SetShieldItemFrequency(float shieldItemFrequency)
        {
            ShieldItemFrequency = shieldItemFrequency;
        }
    }



    public class LeaderboardData
    {
        public string Username { private set; get; }
        public void SetUsername(string username)
        {
            Username = username;
        }

        public int Level { private set; get; }
        public void SetLevel(int level)
        {
            Level = level;
        }
    }

    public class LeaderboardDataComparer : IComparer<LeaderboardData>
    {
        public int Compare(LeaderboardData dataX, LeaderboardData dataY)
        {
            if (dataX.Level < dataY.Level)
                return 1;
            if (dataX.Level > dataY.Level)
                return -1;
            else
                return 0;
        }
    }

    #endregion
}
