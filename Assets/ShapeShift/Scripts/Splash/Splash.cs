using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Slavitica.Splash
{
    public enum EndAction
    {
        LoadScene
    }

    public class Splash : MonoBehaviour
    {
        public float Duration = 2f;
        public float PostDelay = 0.25f;
        public Image FirstImage;
        public Image SecondImage;

        public EndAction Action = EndAction.LoadScene;
        public string LoadScene = "";

        public void Awake()
        {
            StartCoroutine(this.FadeOut(this.FirstImage, this.SecondImage, this.Duration));
        }

        private IEnumerator FadeOut(Image img1, Image img2, float duration)
        {
            yield return this.FadeOut(img1, duration);
            yield return this.FadeOut(img2, duration / 3f * 2f);

            if (this.Action == EndAction.LoadScene)
                SceneManager.LoadScene(this.LoadScene);

        }

        private IEnumerator FadeOut(Image img, float duration)
        {
            var alpha = img.color.a;
            for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
            {
                var newColor = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(alpha, 0f, t));
                img.color = newColor;
                yield return null;
            }

            yield return new WaitForSeconds(this.PostDelay);
        }
    }
}