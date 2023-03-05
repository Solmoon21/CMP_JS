using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ClawbearGames
{
    public class IngameViewController : BaseViewController
    {
        [SerializeField] private RectTransform lelfPanelTrans = null;
        [SerializeField] private RectTransform magnetModePanelTrans = null;
        [SerializeField] private RectTransform shieldModePanelTrans = null;
        [SerializeField] private RectTransform tutorialPanelTrans = null;
        [SerializeField] private Image magnetModeSlider = null;
        [SerializeField] private Image shieldModeSlider = null;
        [SerializeField] private Text levelText = null;


        /// <summary>
        ////////////////////////////////////////////////// Public Functions
        /// </summary>


        public override void OnShow()
        {
            MoveRectTransform(lelfPanelTrans, lelfPanelTrans.anchoredPosition, new Vector2(lelfPanelTrans.anchoredPosition.x, -10f), 0.5f);

            //Update texts and other fields, parameters
            levelText.text = "Level: " + IngameManager.Instance.CurrentLevel.ToString();
            if (!IngameManager.Instance.IsRevived)
            {
                magnetModePanelTrans.gameObject.SetActive(false);
                magnetModeSlider.fillAmount = 1f;
                shieldModePanelTrans.gameObject.SetActive(false);
                shieldModeSlider.fillAmount = 1f;
            }

            tutorialPanelTrans.gameObject.SetActive(!Utilities.IsShowTutorial());
        }

        public override void OnClose()
        {
            lelfPanelTrans.anchoredPosition = new Vector2(lelfPanelTrans.anchoredPosition.x, 150f);
            magnetModePanelTrans.gameObject.SetActive(false);
            shieldModePanelTrans.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }


        /// <summary>
        /// Update magnet mode active time on UI.
        /// </summary>
        /// <param name="maxActiveTime"></param>
        /// <param name="currentActiveTime"></param>
        public void UpdateMagnetModeActiveTime(float maxActiveTime, float currentActiveTime)
        {
            if (!magnetModePanelTrans.gameObject.activeInHierarchy)
                magnetModePanelTrans.gameObject.SetActive(true);
            magnetModeSlider.fillAmount = currentActiveTime / maxActiveTime;
        }


        /// <summary>
        /// Disable the magnet mode panel.
        /// </summary>
        public void DisableMagnetModePanel()
        {
            magnetModePanelTrans.gameObject.SetActive(false);
            magnetModeSlider.fillAmount = 1f;
        }



        /// <summary>
        /// Update shield mode active time on UI.
        /// </summary>
        /// <param name="maxActiveTime"></param>
        /// <param name="currentActiveTime"></param>
        public void UpdateShieldModeActiveTime(float maxActiveTime, float currentActiveTime)
        {
            if (!shieldModePanelTrans.gameObject.activeInHierarchy)
                shieldModePanelTrans.gameObject.SetActive(true);
            shieldModeSlider.fillAmount = currentActiveTime / maxActiveTime;
        }


        /// <summary>
        /// Disable the shield mode panel.
        /// </summary>
        public void DisableShieldModePanel()
        {
            shieldModePanelTrans.gameObject.SetActive(false);
            shieldModeSlider.fillAmount = 1f;
        }



        /// <summary>
        ////////////////////////////////////////////////// UI Buttons
        /// </summary>



        public void OnClickCloseTutorialButton()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Button);
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TUTORIAL, 1);
            tutorialPanelTrans.gameObject.SetActive(false);
            IngameManager.Instance.PlayingGame();
        }

    }
}
