using UnityEngine;

namespace ClawbearGames
{
    public class CharacterContainer : MonoBehaviour
    {
        public int SelectedCharacterIndex { get { return PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SELECTED_CHARACTER, 0); } }
        public CharacterInforController[] CharacterInforControllers { get { return characterInforControllers; } }
        [SerializeField] private CharacterInforController[] characterInforControllers = null;
        public void SetSelectedCharacterIndex(int index)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SELECTED_CHARACTER, index);
            PlayerPrefs.Save();
        }
    }

}
