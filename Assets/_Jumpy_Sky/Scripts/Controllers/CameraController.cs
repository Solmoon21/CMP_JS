using System.Collections;
using UnityEngine;

namespace ClawbearGames
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { private set; get; }

        [Header("Camera Configurations")]
        [SerializeField] private float smoothTime = 0.25f;
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakeAmount = 0.25f;
        [SerializeField] private float decreaseFactor = 1.5f;

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

        private Vector3 velocity = Vector3.zero;
        private float originalDistance = 0;

        private void Start()
        {
            originalDistance = transform.position.y - PlayerController.Instance.transform.position.y;
        }

        private void LateUpdate()
        {
            if (PlayerController.Instance.PlayerState == PlayerState.Player_Living)
            {
                float currentDistance = transform.position.y - PlayerController.Instance.TargetY;
                if (currentDistance < originalDistance)
                {
                    float distance = originalDistance - currentDistance;
                    Vector3 targetPos = transform.position + Vector3.up * distance;
                    targetPos.x = PlayerController.Instance.transform.position.x;
                    transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
                }
            }
        }


        /// <summary>
        /// Shake this camera.
        /// </summary>
        public void Shake()
        {
            StartCoroutine(CRShake());
        }


        /// <summary>
        /// Coroutine skae this camera.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRShake()
        {
            yield return new WaitForSeconds(0.15f);
            Vector3 originalPos = transform.position;
            float shakeDurationTemp = shakeDuration;
            while (shakeDurationTemp > 0)
            {
                Vector3 newPos = originalPos + Random.insideUnitSphere * shakeAmount;
                newPos.z = originalPos.z;
                transform.position = newPos;
                shakeDurationTemp -= Time.deltaTime * decreaseFactor;
                yield return null;
            }

            transform.position = originalPos;
        }
    }
}
