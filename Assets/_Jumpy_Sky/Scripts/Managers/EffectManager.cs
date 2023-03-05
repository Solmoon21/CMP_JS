using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ClawbearGames
{
    public class EffectManager : MonoBehaviour
    {

        public static EffectManager Instance { private set; get; }

        [SerializeField] private ParticleSystem collectCoinItemEffectPrefab = null;
        [SerializeField] private ParticleSystem collectMagnetItemEffectPrefab = null;
        [SerializeField] private ParticleSystem collectShieldItemEffectPrefab = null;
        [SerializeField] private ParticleSystem playerExplodeEffectPrefab = null;
        [SerializeField] private ParticleSystem colorSplashEffectPrefab = null;
        [SerializeField] private PlatformFader platformFaderPrefab = null;
        [SerializeField] private PlatformCenterFader platformCenterFaderPrefab = null;

        private List<ParticleSystem> listCollectCoinItemEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listCollectMagnetItemEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listCollectShieldItemEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listPlayerExplodeEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listColorSplashEffect = new List<ParticleSystem>();
        private List<PlatformFader> listPlatformFader = new List<PlatformFader>();
        private List<PlatformCenterFader> listPlatformCenterFader = new List<PlatformCenterFader>();

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


        /// <summary>
        /// Coroutine play the given particle then disable it.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private IEnumerator CRPlayParticle(ParticleSystem par)
        {
            par.Play();
            yield return new WaitForSeconds(2f);
            par.gameObject.SetActive(false);
            par.transform.SetParent(null);
        }



        /// <summary>
        /// Create a collect coin effect at given position.
        /// </summary>
        /// <param name="pos"></param>
        public void CreateCollectCoinItemEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem collectCoinItemEffect = listCollectCoinItemEffect.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

            if (collectCoinItemEffect == null)
            {
                //Didn't find one -> create new one
                collectCoinItemEffect = Instantiate(collectCoinItemEffectPrefab, pos, Quaternion.identity);
                collectCoinItemEffect.gameObject.SetActive(false);
                listCollectCoinItemEffect.Add(collectCoinItemEffect);
            }

            collectCoinItemEffect.transform.position = pos;
            collectCoinItemEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(collectCoinItemEffect));
        }


        /// <summary>
        /// Create a collect magnet effect at given position.
        /// </summary>
        /// <param name="pos"></param>
        public void CreateCollectMagnetItemEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem collectMagnetItemEffect = listCollectMagnetItemEffect.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

            if (collectMagnetItemEffect == null)
            {
                //Didn't find one -> create new one
                collectMagnetItemEffect = Instantiate(collectMagnetItemEffectPrefab, pos, Quaternion.identity);
                collectMagnetItemEffect.gameObject.SetActive(false);
                listCollectMagnetItemEffect.Add(collectMagnetItemEffect);
            }

            collectMagnetItemEffect.transform.position = pos;
            collectMagnetItemEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(collectMagnetItemEffect));
        }



        /// <summary>
        /// Create a collect shield item effect at given position.
        /// </summary>
        /// <param name="pos"></param>
        public void CreateCollectShieldItemEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem collectShieldItemEffect = listCollectShieldItemEffect.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

            if (collectShieldItemEffect == null)
            {
                //Didn't find one -> create new one
                collectShieldItemEffect = Instantiate(collectShieldItemEffectPrefab, pos, Quaternion.identity);
                collectShieldItemEffect.gameObject.SetActive(false);
                listCollectShieldItemEffect.Add(collectShieldItemEffect);
            }

            collectShieldItemEffect.transform.position = pos;
            collectShieldItemEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(collectShieldItemEffect));
        }


        /// <summary>
        /// Create player explode effect at given position.
        /// </summary>
        /// <param name="pos"></param>
        public void CreatePlayerExplodeEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem playerExplodeEffect = listPlayerExplodeEffect.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

            if (playerExplodeEffect == null)
            {
                //Didn't find one -> create new one
                playerExplodeEffect = Instantiate(playerExplodeEffectPrefab, pos, Quaternion.identity);
                playerExplodeEffect.gameObject.SetActive(false);
                listPlayerExplodeEffect.Add(playerExplodeEffect);
            }

            playerExplodeEffect.transform.position = pos;
            playerExplodeEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(playerExplodeEffect));
        }


        /// <summary>
        /// Create color splash efffect at given position.
        /// </summary>
        /// <param name="pos"></param>
        public void CreateColorSplashEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem colorSplashEffect = listColorSplashEffect.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

            if (colorSplashEffect == null)
            {
                //Didn't find one -> create new one
                colorSplashEffect = Instantiate(colorSplashEffectPrefab, pos, Quaternion.identity);
                colorSplashEffect.gameObject.SetActive(false);
                listColorSplashEffect.Add(colorSplashEffect);
            }

            colorSplashEffect.transform.position = pos;
            colorSplashEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(colorSplashEffect));
        }


        /// <summary>
        /// Create PlatformCenterFader effect at given position and parent.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="parent"></param>
        public void CreatePlatformCenterFader(Vector3 pos, Transform parent)
        {
            //Find in the list
            PlatformCenterFader platformCenterFader = listPlatformCenterFader.Where(a => a.gameObject.activeSelf).FirstOrDefault();

            if (platformCenterFader == null)
            {
                //Didn't find one -> create new one
                platformCenterFader = Instantiate(platformCenterFaderPrefab, pos, Quaternion.identity);
                platformCenterFader.gameObject.SetActive(false);
                listPlatformCenterFader.Add(platformCenterFader);
            }

            platformCenterFader.transform.position = pos;
            platformCenterFader.transform.SetParent(parent);
            platformCenterFader.gameObject.SetActive(true);
            platformCenterFader.ScaleUpAndFadeOut();
        }



        /// <summary>
        /// Create PlatformFader effect at given position and parent with mesh.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="parent"></param>
        /// <param name="mesh"></param>
        public void CreatePlatformFader(Vector3 pos, Transform parent, Mesh mesh)
        {
            //Find in the list
            PlatformFader platformFader = listPlatformFader.Where(a => a.gameObject.activeSelf).FirstOrDefault();

            if (platformFader == null)
            {
                //Didn't find one -> create new one
                platformFader = Instantiate(platformFaderPrefab, pos, Quaternion.identity);
                platformFader.gameObject.SetActive(false);
                listPlatformFader.Add(platformFader);
            }

            platformFader.transform.position = pos + Vector3.down * 0.05f;
            platformFader.transform.SetParent(parent);
            platformFader.gameObject.SetActive(true);
            platformFader.ScaleUpAndFadeOut(mesh);
        }
    }
}