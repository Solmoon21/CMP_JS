using UnityEngine;

namespace ClawbearGames
{
    public class ServicesManager : MonoBehaviour
    {
        public static ServicesManager Instance { private set; get; }

        [SerializeField] private SoundManager soundManager = null;
        [SerializeField] private CoinManager coinManager = null;
        [SerializeField] private ShareManager shareManager = null;
        [SerializeField] private CharacterContainer characterContainer = null;
        [SerializeField] private DailyRewardManager dailyRewardManager = null;
        [SerializeField] private LeaderboardManager leaderboardManager = null;
        [SerializeField] private NotificationManager notificationManager = null;
        [SerializeField] private AdManager adManager = null;

        public SoundManager SoundManager { get { return soundManager; } }
        public CoinManager CoinManager { get { return coinManager; } }
        public ShareManager ShareManager { get { return shareManager; } }
        public CharacterContainer CharacterContainer { get { return characterContainer; } }
        public DailyRewardManager DailyRewardManager { get { return dailyRewardManager; } }
        public LeaderboardManager LeaderboardManager { get { return leaderboardManager; } }
        public NotificationManager NotificationManager { get { return notificationManager; } }
        public AdManager AdManager { get { return adManager; } }

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}

