using UnityEngine;
using System.Collections;


namespace ClawbearGames
{
    public class PlatformCenterController : MonoBehaviour
    {
        public bool IsFadedOut { private set; get; }
        private MeshRenderer meshRenderer = null;

        private void OnDisable()
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
            IsFadedOut = false;
        }


        /// <summary>
        /// Determine whether this platform center is collided with player.
        /// </summary>
        /// <param name="playerLayer"></param>
        /// <returns></returns>
        public bool IsCollidedPlayer(LayerMask playerLayer)
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();

            Collider[] playerColliders = Physics.OverlapSphere(transform.position, meshRenderer.bounds.extents.x, playerLayer);
            return playerColliders.Length > 0;
        }



        /// <summary>
        /// Handle actions when this platform center collided with player.
        /// </summary>
        /// <param name="parent"></param>
        public void HandleCollidedPlayer(Transform parent)
        {
            IsFadedOut = true;
            meshRenderer.enabled = false;
            EffectManager.Instance.CreateColorSplashEffect(transform.position);
            EffectManager.Instance.CreatePlatformCenterFader(transform.position, parent);
        }
    }
}
