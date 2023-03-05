using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClawbearGames
{
    public class PlayerController : MonoBehaviour
    {

        public static PlayerController Instance { private set; get; }
        public static event System.Action<PlayerState> PlayerStateChanged = delegate { };


        [Header("Player Configuration")]
        [SerializeField] private float thresholdSpeed = 200f;
        [SerializeField] private float swipeSmoothTime = 0.08f;
        [SerializeField] private float fallDownVelocity = -60;
        [SerializeField][Range(1, 30)] private int magnetModeActiveTime = 15;
        [SerializeField][Range(1, 30)] private int shieldModeActiveTime = 10;

        [Header("Player References")]
        [SerializeField] private MeshFilter meshFilter = null;
        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private MeshCollider meshCollider = null;
        [SerializeField] private MeshFilter shieldMeshFilter = null;
        [SerializeField] private ParticleSystem trailEffect = null;
        [SerializeField] private LayerMask playerLayerMask = new LayerMask();
        [SerializeField] private LayerMask platformLayerMask = new LayerMask();
        [SerializeField] private LayerMask obstacleLayerMask = new LayerMask();

        public PlayerState PlayerState
        {
            get { return playerState; }
            private set
            {
                if (value != playerState)
                {
                    value = playerState;
                    PlayerStateChanged(playerState);
                }
            }
        }


        public float TargetY { private set; get; }
        public bool IsActiveMagnetMode { private set; get; }
        public bool IsActiveShieldMode { private set; get; }


        private PlayerState playerState = PlayerState.Player_Prepare;
        private Vector3 velocity = Vector3.zero;
        private Vector3 savedPlatformPos = Vector3.zero;
        private float movementSpeed = 0;
        private float firsInputtX = 0;
        private float currentJumpVelocity = 0;
        private int centerJumpCount = 0;

        private void OnEnable()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }
        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }
        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            if (obj == IngameState.Ingame_Playing)
            {
                PlayerLiving();
            }
            else if (obj == IngameState.Ingame_CompleteLevel)
            {
                PlayerCompletedLevel();
            }
        }




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
            //Fire event
            PlayerState = PlayerState.Player_Prepare;
            playerState = PlayerState.Player_Prepare;

            //Add other actions here


            //Setup character
            CharacterInforController charControl = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            meshFilter.mesh = charControl.Mesh;
            meshRenderer.material = charControl.Material;
            shieldMeshFilter.mesh = charControl.Mesh;

            //Setup parameters and objects
            IsActiveMagnetMode = false;
            IsActiveShieldMode = false;
            shieldMeshFilter.gameObject.SetActive(false);
            trailEffect.gameObject.SetActive(false);
            trailEffect.transform.SetParent(transform);
        }

        private void Update()
        {
            if (playerState == PlayerState.Player_Living)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    firsInputtX = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)).x;
                }
                else if (Input.GetMouseButton(0))
                {
                    float currentX = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)).x;
                    float amount = Mathf.Abs(Mathf.Abs(currentX) - Mathf.Abs(firsInputtX));

                    if (currentX > firsInputtX)
                    {
                        Vector3 newPos = transform.position;
                        newPos += new Vector3(amount * thresholdSpeed * Time.deltaTime, 0, 0);
                        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, swipeSmoothTime);
                    }
                    else
                    {
                        Vector3 newPos = transform.position;
                        newPos -= new Vector3(amount * thresholdSpeed * Time.deltaTime, 0, 0);
                        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, swipeSmoothTime);
                    }

                    firsInputtX = currentX;
                }
            }
        }



        /// <summary>
        /// Call PlayerState.Player_Living event and handle other actions.
        /// </summary>
        private void PlayerLiving()
        {
            //Fire event
            PlayerState = PlayerState.Player_Living;
            playerState = PlayerState.Player_Living;

            //Add other actions here
            if (IngameManager.Instance.IsRevived)
            {
                StartCoroutine(CRHandleActionsAfterRevived());
            }
            else
            {
                trailEffect.gameObject.SetActive(true);
                StartCoroutine(CRJumpUp(IngameManager.Instance.GetHigherPlatformPoint() + 2f));
            }
        }


        /// <summary>
        /// Call PlayerState.Player_Died event and handle other actions.
        /// </summary>
        public void PlayerDied()
        {
            //Fire event
            PlayerState = PlayerState.Player_Died;
            playerState = PlayerState.Player_Died;

            //Add other actions here
            ServicesManager.Instance.ShareManager.CreateScreenshot();
            CameraController.Instance.Shake();
            trailEffect.gameObject.SetActive(false);
            meshCollider.enabled = false;       
        }



        /// <summary>
        /// Fire Player_CompletedLevel event and handle other actions.
        /// </summary>
        private void PlayerCompletedLevel()
        {
            //Fire event
            PlayerState = PlayerState.Player_CompletedLevel;
            playerState = PlayerState.Player_CompletedLevel;

            //Add others action here
            ServicesManager.Instance.ShareManager.CreateScreenshot();
        }



        /// <summary>
        /// Coroutine jump up.
        /// </summary>
        /// <param name="endY"></param>
        /// <returns></returns>
        private IEnumerator CRJumpUp(float endY)
        {
            float t = 0;
            float startY = transform.position.y;
            float jumpingTime = Mathf.Abs(endY - startY) / movementSpeed;
            Quaternion startQuater = Quaternion.Euler(meshRenderer.transform.localEulerAngles);
            Quaternion endQuater = Quaternion.Euler(new Vector3(15f, 0, 0));
            while (t < jumpingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutCubic, t / jumpingTime);
                Vector3 pos = transform.position;
                float newY = Mathf.Lerp(startY, endY, factor);
                pos.y = newY;
                transform.position = pos;
                meshRenderer.transform.localRotation = Quaternion.Lerp(startQuater, endQuater, factor);
                yield return null;
            }

            currentJumpVelocity = 0;
            StartCoroutine(CRFallDown(endY));
        }



        /// <summary>
        /// Coroutine fall down.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRFallDown(float endY)
        {
            while (true)
            {
                transform.position = transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallDownVelocity * Time.deltaTime * Time.deltaTime / 2);
                if (currentJumpVelocity < fallDownVelocity)
                    currentJumpVelocity = fallDownVelocity;
                else
                    currentJumpVelocity = currentJumpVelocity + fallDownVelocity * Time.deltaTime;

                //Adjust angles
                Vector3 angles = meshRenderer.transform.localEulerAngles;
                angles.x -= Time.deltaTime * 100f;
                meshRenderer.transform.localEulerAngles = angles;


                if (transform.position.y <= endY - 2f)
                {

                    if (!IsActiveShieldMode)
                    {
                        //Check collide with obstacle
                        Collider[] obstacleColliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f, meshRenderer.bounds.extents.x, obstacleLayerMask);
                        if (obstacleColliders.Length > 0)
                        {
                            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.PlayerExploded);
                            meshRenderer.enabled = false;
                            EffectManager.Instance.CreatePlayerExplodeEffect(transform.position);
                            IngameManager.Instance.HandlePlayerDied();
                            PlayerDied();
                            yield break;
                        }
                    }


                    //check collide with platform and the center of that platform
                    Collider[] platformColliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f, meshRenderer.bounds.extents.x, platformLayerMask);
                    if (platformColliders.Length > 0)
                    {
                        PlatformController platformController = PoolManager.Instance.FindPlatformController(platformColliders[0].transform.root);
                        platformController.Bounce();
                        if (platformController.PlatformCenterController.IsCollidedPlayer(playerLayerMask))
                        {
                            if (!platformController.PlatformCenterController.IsFadedOut)
                            {
                                platformController.PlatformCenterController.HandleCollidedPlayer(platformController.transform);

                                //Play sound effects
                                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.CenterJumps[centerJumpCount]);
                                centerJumpCount = (centerJumpCount == ServicesManager.Instance.SoundManager.CenterJumps.Length - 1) ? 0 : centerJumpCount + 1;
                            }
                            else
                            {
                                centerJumpCount = 0;
                                ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.NormalJump);
                            }
                        }
                        else
                        {
                            centerJumpCount = 0;
                            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.NormalJump);
                        }

                        TargetY = platformColliders[0].transform.position.y;
                        if (IngameManager.Instance.IsHighestYAxis(TargetY))
                        {
                            StartCoroutine(CRJumpUp(TargetY + 5f));
                            if (playerState == PlayerState.Player_Living)
                            {
                                IngameManager.Instance.CompletedLevel();
                            }
                        }
                        else
                        {
                            StartCoroutine(CRJumpUp(IngameManager.Instance.GetHigherPlatformPoint() + 2f));
                        }

                        savedPlatformPos = platformController.transform.position;
                        IngameManager.Instance.IncreaseJumpCount();
                        yield break;
                    }
                }           
                yield return null;

                //Check player fall out of the screen
                Vector2 viewPort = Camera.main.WorldToViewportPoint(transform.position);
                if (viewPort.y <= -0.2f)
                {
                    PlayerDied();
                    IngameManager.Instance.HandlePlayerDied();
                    yield break;
                }
            }
        }



        /// <summary>
        /// Coroutine handle actions after player revived.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRHandleActionsAfterRevived()
        {
            meshRenderer.enabled = true;
            meshCollider.enabled = true;
            transform.position = savedPlatformPos;
            trailEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ActiveShieldMode();
            StartCoroutine(CRJumpUp(IngameManager.Instance.GetHigherPlatformPoint() + 2f));
        }


        /// <summary>
        /// Coroutine count down the magnet mode.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRCountDownMagnetMode()
        {
            float currentActiveTime = magnetModeActiveTime;
            float maxActiveTime = magnetModeActiveTime;
            ViewManager.Instance.IngameViewController.UpdateMagnetModeActiveTime(maxActiveTime, currentActiveTime);
            while (currentActiveTime > 0)
            {
                yield return null;
                currentActiveTime -= Time.deltaTime;
                ViewManager.Instance.IngameViewController.UpdateMagnetModeActiveTime(maxActiveTime, currentActiveTime);
                if (playerState != PlayerState.Player_Living)
                {
                    break;
                }
            }
            IsActiveMagnetMode = false;
            ViewManager.Instance.IngameViewController.DisableMagnetModePanel();
        }


        /// <summary>
        /// Coroutine count down shield mode.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRCountDownShieldMode()
        {
            float currentActiveTime = shieldModeActiveTime;
            float maxActiveTime = currentActiveTime;
            ViewManager.Instance.IngameViewController.UpdateShieldModeActiveTime(maxActiveTime, currentActiveTime);
            while (currentActiveTime > 0)
            {
                yield return null;
                currentActiveTime -= Time.deltaTime;
                ViewManager.Instance.IngameViewController.UpdateShieldModeActiveTime(maxActiveTime, currentActiveTime);
                if (playerState != PlayerState.Player_Living)
                {
                    break;
                }
            }
            shieldMeshFilter.gameObject.SetActive(false);
            IsActiveShieldMode = false;
            ViewManager.Instance.IngameViewController.DisableShieldModePanel();
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////Public functions


        /// <summary>
        /// Set movement speed for the player.
        /// </summary>
        /// <param name="speed"></param>
        public void SetMovementSpeed(float speed)
        {
            movementSpeed = speed;
        }



        /// <summary>
        /// Active the magnet mode for this player.
        /// </summary>
        public void ActiveMagnetMode()
        {
            if (!IsActiveMagnetMode)
            {
                IsActiveMagnetMode = true;
                StartCoroutine(CRCountDownMagnetMode());
            }
        }


        /// <summary>
        /// Active the shield mode for this player.
        /// </summary>
        public void ActiveShieldMode()
        {
            if (!IsActiveShieldMode)
            {
                IsActiveShieldMode = true;
                shieldMeshFilter.gameObject.SetActive(true);
                StartCoroutine(CRCountDownShieldMode());
            }
        }
    }
}
