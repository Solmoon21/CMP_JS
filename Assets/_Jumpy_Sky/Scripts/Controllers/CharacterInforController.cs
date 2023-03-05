using System.Collections;
using UnityEngine;

namespace ClawbearGames
{
    public class CharacterInforController : MonoBehaviour
    {
        [Header("The name of this character. This field must be unique.")]
        [Header("Character Infor Configuration")]
        [SerializeField] private string characterName = string.Empty;

        [Header("Price of the character.")]
        [SerializeField] private int characterPrice = 0;

        [Header("The material's color of this character when it locked.")]
        [SerializeField] private Color lockedColor = Color.black;

        [Header("The material's color of this character when it unlocked.")]
        [SerializeField] private Color unlockedColor = Color.white;

        [Header("Character References")]
        [SerializeField] private MeshFilter meshFilter = null;
        [SerializeField] private MeshRenderer meshRenderer = null;

        /// <summary>
        /// The mesh of this character.
        /// </summary>
        public Mesh Mesh { get { return meshFilter.sharedMesh; } }


        /// <summary>
        /// the material of this character.
        /// </summary>
        public Material Material { get { return meshRenderer.sharedMaterial; } }


        /// <summary>
        /// The sequence number of this character in CharacterContainer
        /// </summary>
        public int SequenceNumber { private set; get; }


        /// <summary>
        /// The price of this character.
        /// </summary>
        public int CharacterPrice { get { return characterPrice; } }


        /// <summary>
        /// Is this character unlocked or not.
        /// </summary>
        public bool IsUnlocked { get { return PlayerPrefs.GetInt(characterName, 0) == 1; } }


        private void Awake()
        {
            //If the price equals to 0 -> set this character to be unlocked
            if (characterPrice == 0)
            {
                PlayerPrefs.SetInt(characterName, 1);
                PlayerPrefs.Save();
            }


            //Set the material's colors for this character
            meshRenderer.material.color = (IsUnlocked) ? unlockedColor : lockedColor;
        }


        /// <summary>
        /// Set the sequence number of this character
        /// </summary>
        /// <param name="number"></param>
        public void SetSequenceNumber(int number)
        {
            SequenceNumber = number;
        }


        /// <summary>
        /// Unlock this character.
        /// </summary>
        /// <returns></returns>
        public void Unlock()
        {
            PlayerPrefs.SetInt(characterName, 1);
            PlayerPrefs.Save();
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.Unlock);
            ServicesManager.Instance.CoinManager.RemoveTotalCoins(characterPrice, 0.15f);

            //Set the material's colors for this skin
            meshRenderer.material.color = unlockedColor;
        }

        /// <summary>
        /// Move this character by given vector.
        /// </summary>
        /// <param name="dir"></param>
        public void MoveByVector(Vector3 dir)
        {
            transform.position += dir;
        }
    }
}
