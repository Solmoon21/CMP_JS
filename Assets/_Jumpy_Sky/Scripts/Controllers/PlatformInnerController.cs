using System.Collections;
using UnityEngine;

namespace ClawbearGames
{
    public class PlatformInnerController : MonoBehaviour
    {
        private MeshRenderer meshRenderer = null;


        /// <summary>
        /// Setup the color for this object.
        /// </summary>
        /// <param name="newColor"></param>
        public void SetColor(Color newColor)
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = newColor;
        }


        /// <summary>
        /// Change the color of this object from the current color to newColor.
        /// </summary>
        /// <param name="isFade"></param>
        public void ChangeColor(Color newColor)
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            StartCoroutine(CRChangeColor(newColor));
        }


        /// <summary>
        /// Coroutine change color.
        /// </summary>
        /// <param name="newColor"></param>
        /// <returns></returns>
        private IEnumerator CRChangeColor(Color newColor)
        {
            float t = 0;
            float changingTime = 0.25f;
            Color startColor = meshRenderer.material.color;
            while (t < changingTime)
            {
                t += Time.deltaTime;
                float factor = t / changingTime;
                meshRenderer.material.color = Color.Lerp(startColor, newColor, factor);
                yield return null;
            }
        }
    }
}
