using UnityEngine;
using System.Collections;

namespace ClawbearGames
{
    public class HomeManager : MonoBehaviour
    {
        [Header("HomeManager References")]
        [SerializeField] private Transform characterTrans = null;
        [SerializeField] private Transform platformTrans = null;
        [SerializeField] private MeshFilter characterMeshFilter = null;
        [SerializeField] private MeshRenderer characterMeshRenderer = null;


        private float fallDownVelocity = -60;
        private float currentJumpVelocity = 0;

        private void Awake()
        {
            //Set current level
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_SAVED_LEVEL))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1);
            }
        }


        private void Start()
        {
            Application.targetFrameRate = 60;
            ViewManager.Instance.OnShowView(ViewType.HOME_VIEW);

            //Setup character
            CharacterInforController charControl = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            characterMeshFilter.mesh = charControl.Mesh;
            characterMeshRenderer.material = charControl.Material;
            StartCoroutine(CRJumpUp());
        }


        /// <summary>
        /// Coroutine jump up.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRJumpUp()
        {
            float t = 0;
            float jumpingTime = 0.4f;
            float startY = characterTrans.position.y;
            float endY = 4f;
            Quaternion startQuater = Quaternion.Euler(characterMeshRenderer.transform.localEulerAngles);
            Quaternion endQuater = Quaternion.Euler(new Vector3(25f, 0, 0));
            while (t < jumpingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutCubic, t / jumpingTime);
                Vector3 pos = characterTrans.position;
                float newY = Mathf.Lerp(startY, endY, factor);
                pos.y = newY;
                characterTrans.position = pos;
                characterMeshRenderer.transform.localRotation = Quaternion.Lerp(startQuater, endQuater, factor);
                yield return null;
            }

            currentJumpVelocity = 0;
            StartCoroutine(CRFallDown());
        }


        /// <summary>
        /// Coroutine fall down.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRFallDown()
        {
            while (gameObject.activeSelf)
            {
                characterTrans.position = characterTrans.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallDownVelocity * Time.deltaTime * Time.deltaTime / 2);

                if (currentJumpVelocity < fallDownVelocity)
                    currentJumpVelocity = fallDownVelocity;
                else
                    currentJumpVelocity = currentJumpVelocity + fallDownVelocity * Time.deltaTime;

                //Adjust angles
                Vector3 angles = characterMeshRenderer.transform.localEulerAngles;
                angles.x -= Time.deltaTime * 200f;
                characterMeshRenderer.transform.localEulerAngles = angles;


                if (currentJumpVelocity < 0)
                {
                    if (characterTrans.position.y <= 0f)
                    {
                        StartCoroutine(CRJumpUp());
                        StartCoroutine(CRBouncePlatform());
                        yield break;
                    }
                }
                yield return null;
            }
        }


        /// <summary>
        /// Coroutine bounce the platform.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRBouncePlatform()
        {
            float bouncingTime = 0.1f;
            float t = 0;
            float startY = platformTrans.position.y;
            float endY = startY - 0.5f;

            while (t < bouncingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.Liner, t / bouncingTime);
                Vector3 currentPos = platformTrans.position;
                currentPos.y = Mathf.Lerp(startY, endY, factor);
                platformTrans.position = currentPos;
                yield return null;
            }

            t = 0;
            while (t < bouncingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.Liner, t / bouncingTime);
                Vector3 currentPos = platformTrans.position;
                currentPos.y = Mathf.Lerp(endY, startY, factor);
                platformTrans.position = currentPos;
                yield return null;
            }
        }

    }
}
