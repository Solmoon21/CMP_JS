using System.Collections;
using UnityEngine;


namespace ClawbearGames
{
    public class ItemController : MonoBehaviour
    {

        [Header("Item Config")]
        [SerializeField] private float minRotatingSpeed = 150f;
        [SerializeField] private float maxRotatingSpeed = 350f;

        [Header("Item References")]
        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private ItemType itemType = ItemType.COIN;
        [SerializeField] private LayerMask playerLayerMask = new LayerMask();

        public ItemType ItemType { get { return itemType; } }
        private Vector3 forwardDir = Vector3.forward;
        private float rotatingSpeed = 0;


        private void Update()
        {
            transform.localEulerAngles += Vector3.up * rotatingSpeed * Time.deltaTime;

            //Check collide with player
            Collider[] colliders = Physics.OverlapBox(meshRenderer.bounds.center, meshRenderer.bounds.extents, transform.rotation, playerLayerMask);
            if (colliders.Length > 0)
            {
                if (itemType == ItemType.COIN)
                {
                    ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.CollectCoinItem);
                    ServicesManager.Instance.CoinManager.AddCollectedCoins(1);
                    EffectManager.Instance.CreateCollectCoinItemEffect(meshRenderer.bounds.center);
                    transform.SetParent(null);
                    gameObject.SetActive(false);
                }
                else if (itemType == ItemType.MAGNET)
                {
                    ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.EnableMagnetMode);
                    EffectManager.Instance.CreateCollectMagnetItemEffect(meshRenderer.bounds.center);
                    PlayerController.Instance.ActiveMagnetMode();
                    transform.SetParent(null);
                    gameObject.SetActive(false);
                }
                else if (itemType == ItemType.SHIELD)
                {
                    ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.EnableShieldMode);
                    EffectManager.Instance.CreateCollectShieldItemEffect(meshRenderer.bounds.center);
                    PlayerController.Instance.ActiveShieldMode();
                    transform.SetParent(null);
                    gameObject.SetActive(false);
                }
            }
        }



        /// <summary>
        /// Coroutine move this coin to player when magnet mode of the player is active.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRMoveToPlayer()
        {
            yield return null;
            while (gameObject.activeSelf)
            {
                if (PlayerController.Instance.IsActiveMagnetMode)
                {
                    Vector3 playerPos = PlayerController.Instance.transform.position;
                    playerPos.x = transform.position.x;
                    float distance = Vector3.Distance(transform.position, playerPos);
                    if (distance > 0 && distance <= 25)
                    {
                        transform.SetParent(null);
                        break;
                    }
                }
                yield return null;
            }


            while (IngameManager.Instance.IngameState == IngameState.Ingame_Playing && gameObject.activeSelf)
            {
                Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
                transform.position += direction * 25f * Time.deltaTime;
                yield return null;
            }
        }




        /// <summary>
        /// Setup this item
        /// </summary>
        public void OnSetup()
        {
            transform.localPosition += Vector3.up * 0.05f;
            rotatingSpeed = Random.Range(minRotatingSpeed, maxRotatingSpeed);
            if (itemType == ItemType.COIN)
            {
                StartCoroutine(CRMoveToPlayer());
            }
        }
    }
}
