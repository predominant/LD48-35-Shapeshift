using UnityEngine;
using System.Collections;
using ShapeShift;
using UnityEngine.UI;

namespace ShapeShift.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        public void Update()
        {
            this.GetComponent<Text>().text = this.FormatScore();
        }

        private string FormatScore()
        {
            return string.Format("{0:n0}m", GameController.Score);
        }
    }
}