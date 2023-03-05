using System.Collections;
using UnityEngine;

namespace ClawbearGames
{
    public class PlatformCenterFader : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer = null;


        /// <summary>
        /// Scale up and fade out.
        /// </summary>
        public void ScaleUpAndFadeOut()
        {
            StartCoroutine(CRScaleUpAndFadeOut());
        }
        

        /// <summary>
        /// Coroutine scale up and fade out.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRScaleUpAndFadeOut()
        {
            float t = 0;
            float actionTime = 0.5f;
            Color startColor = meshRenderer.material.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
            while (t < actionTime)
            {
                t += Time.deltaTime;
                float factor = t / actionTime;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 3f, factor);
                meshRenderer.material.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }

            transform.SetParent(null);
            meshRenderer.material.color = startColor;
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }


    }
}
