using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClawbearGames
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private PlatformType platformType = PlatformType.SQUARE;
        [SerializeField] private PlatformSize platformSize = PlatformSize.SMALL;
        [SerializeField] private MeshFilter meshFilter = null;
        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private PlatformInnerController platformInnerController = null;
        [SerializeField] private PlatformCenterController platformCenterController = null;

        public PlatformCenterController PlatformCenterController => platformCenterController;
        public PlatformType PlatformType { get { return platformType; } }
        public PlatformSize PlatformSize { get { return platformSize; } }

        private void Update()
        {
            if (PlayerController.Instance.PlayerState == PlayerState.Player_Living)
            {
                //Check and disable this platform
                Vector2 viewPort = Camera.main.WorldToViewportPoint(transform.position);
                if (viewPort.y <= -0.2f)
                {
                    //Disable all items
                    ItemController[] itemControllers = GetComponentsInChildren<ItemController>();
                    foreach (ItemController itemController in itemControllers)
                    {
                        itemController.transform.SetParent(null);
                        itemController.gameObject.SetActive(false);
                    }

                    //Disable all obstacles
                    ObstacleController[] obstacleControllers = GetComponentsInChildren<ObstacleController>();
                    foreach (ObstacleController obstacleController in obstacleControllers)
                    {
                        obstacleController.transform.SetParent(null);
                        obstacleController.gameObject.SetActive(false);
                    }

                    gameObject.SetActive(false);
                    IngameManager.Instance.CreateNextPlatform();
                }
            }
        }



        /// <summary>
        /// Setup this platform.
        /// </summary>
        /// <param name="platformParams"></param>
        public void OnSetup(PlatformParams platformParams)
        {
            if (!platformParams.IsEmptyPlatform)
            {
                //Set deviation
                Vector3 platformPos = transform.position;
                platformPos.x = platformParams.HorizontalDeviation;
                transform.position = platformPos;

                //Init list position for items
                List<Vector3> listItemPos = GetItemPositions();
                List<int> listUsedIndex = new List<int>();

                //Create magnet item
                if (Random.value <= platformParams.MagnetItemFrequency && listItemPos.Count > listUsedIndex.Count)
                {
                    int index = Random.Range(0, listItemPos.Count);
                    while (listUsedIndex.Contains(index))
                    {
                        index = Random.Range(0, listItemPos.Count);
                    }
                    listUsedIndex.Add(index);

                    ItemController itemController = PoolManager.Instance.GetItemController(ItemType.MAGNET);
                    itemController.transform.position = listItemPos[index];
                    itemController.transform.SetParent(transform);
                    itemController.gameObject.SetActive(true);
                    itemController.OnSetup();
                }

                //Create shield item
                if (Random.value <= platformParams.ShieldItemFrequency && listItemPos.Count > listUsedIndex.Count)
                {
                    int index = Random.Range(0, listItemPos.Count);
                    while (listUsedIndex.Contains(index))
                    {
                        index = Random.Range(0, listItemPos.Count);
                    }
                    listUsedIndex.Add(index);

                    ItemController itemController = PoolManager.Instance.GetItemController(ItemType.SHIELD);
                    itemController.transform.position = listItemPos[index];
                    itemController.transform.SetParent(transform);
                    itemController.gameObject.SetActive(true);
                    itemController.OnSetup();
                }

                //Create coin items
                int coinAmount = ((listItemPos.Count - listUsedIndex.Count) < platformParams.CoinItemAmount) ? (listItemPos.Count - listUsedIndex.Count) : platformParams.CoinItemAmount;
                while (Random.value <= platformParams.CoinItemFrequency && coinAmount > 0)
                {
                    int index = Random.Range(0, listItemPos.Count);
                    while (listUsedIndex.Contains(index))
                    {
                        index = Random.Range(0, listItemPos.Count);
                    }
                    listUsedIndex.Add(index);

                    ItemController itemController = PoolManager.Instance.GetItemController(ItemType.COIN);
                    itemController.transform.position = listItemPos[index];
                    itemController.transform.SetParent(transform);
                    itemController.gameObject.SetActive(true);
                    itemController.OnSetup();
                    coinAmount--;
                }



                //Create obstacle
                if (Random.value <= platformParams.ObstacleFrequency && listItemPos.Count > listUsedIndex.Count)
                {
                    int index = Random.Range(0, listItemPos.Count);
                    while (listUsedIndex.Contains(index))
                    {
                        index = Random.Range(0, listItemPos.Count);
                    }
                    listUsedIndex.Add(index);

                    ObstacleController obstacleController = PoolManager.Instance.GetObstacleController();
                    obstacleController.transform.position = listItemPos[index];
                    obstacleController.transform.SetParent(transform);
                    obstacleController.gameObject.SetActive(true);
                }


                if (Random.value <= platformParams.MovebleFrequency)
                {
                    StartCoroutine(CRLoopMove(platformParams.MovementSpeed, platformParams.LerpType));
                }
            }
        }



        /// <summary>
        /// Get the list position for items.
        /// </summary>
        /// <returns></returns>
        private List<Vector3> GetItemPositions()
        {
            List<Vector3> listResult = new List<Vector3>();
            if (platformSize == PlatformSize.SMALL)
            {
                listResult.Add(transform.position + Vector3.left * 0.5f);
                listResult.Add(transform.position + Vector3.right * 0.5f);
            }
            else if (platformSize == PlatformSize.NORMAL)
            {
                listResult.Add(transform.position);
                listResult.Add(transform.position + Vector3.left);
                listResult.Add(transform.position + Vector3.right);
            }
            else if (platformSize == PlatformSize.BIG)
            {
                listResult.Add(transform.position);
                listResult.Add(transform.position + Vector3.left * 0.75f);
                listResult.Add(transform.position + Vector3.right * 0.75f);
                listResult.Add(transform.position + Vector3.left * 1.5f);
                listResult.Add(transform.position + Vector3.right * 1.5f);
            }

            return listResult;
        }

        /// <summary>
        /// Couroutine move left and right on a loop.
        /// </summary>
        /// <param name="movementSpeed"></param>
        /// <param name="lerpType"></param>
        /// <returns></returns>
        private IEnumerator CRLoopMove(float movementSpeed, LerpType lerpType)
        {
            float t = 0;
            float movementTime = (Mathf.Abs(transform.position.x) * 2f) / movementSpeed;
            float startX = transform.position.x;
            float endX = -startX;

            while (gameObject.activeSelf)
            {
                t = 0;
                while (t < movementTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / movementTime);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Lerp(startX, endX, factor);
                    transform.position = pos;
                    yield return null;
                }


                t = 0;
                while (t < movementTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / movementTime);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Lerp(endX, startX, factor);
                    transform.position = pos;
                    yield return null;
                }
            }      
        }


        /// <summary>
        /// Bounce this platform down and up.
        /// </summary>
        public void Bounce()
        {
            EffectManager.Instance.CreatePlatformFader(transform.position, transform, meshFilter.sharedMesh);
            StartCoroutine(CRBounce());
        }


        /// <summary>
        /// Coroutine bounce this platform.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRBounce()
        {
            float bouncingTime = 0.1f;
            float t = 0;
            float startY = transform.position.y;
            float endY = startY - 0.5f;

            while (t < bouncingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.Liner, t / bouncingTime);
                Vector3 currentPos = transform.position;
                currentPos.y = Mathf.Lerp(startY, endY, factor);
                transform.position = currentPos;
                yield return null;
            }

            t = 0;
            while (t < bouncingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.Liner, t / bouncingTime);
                Vector3 currentPos = transform.position;
                currentPos.y = Mathf.Lerp(endY, startY, factor);
                transform.position = currentPos;
                yield return null;
            }
        }



        /// <summary>
        /// Set colors for this platform.
        /// </summary>
        /// <param name="platformColors"></param>
        public void SetColors(PlatformColorsConfiguration platformColors)
        {
            meshRenderer.material.color = platformColors.PlatformColor;
            platformInnerController.SetColor(platformColors.PlatformInnerColor);
        }



        /// <summary>
        /// Change the color of this object from the current color to new color using colorsConfig parameter.
        /// </summary>
        /// <param name="platformColors"></param>
        /// <param name="delay"></param>
        public void ChangeColor(PlatformColorsConfiguration platformColors, float delay)
        {
            StartCoroutine(CRChangeColor(platformColors, delay));
        }


        /// <summary>
        /// Coroutine change color for this platform.
        /// </summary>
        /// <param name="platformColor"></param>
        /// <param name="platformInnderColor"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator CRChangeColor(PlatformColorsConfiguration platformColors, float delay)
        {
            yield return new WaitForSeconds(delay);
            platformInnerController.ChangeColor(platformColors.PlatformInnerColor);

            float t = 0;
            float fadingTime = 0.25f;
            Color startColor = meshRenderer.material.color;
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                meshRenderer.material.color = Color.Lerp(startColor, platformColors.PlatformColor, factor);
                yield return null;
            }
        }
    }
}
