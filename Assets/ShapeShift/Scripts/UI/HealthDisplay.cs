using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ShapeShift.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        public int MaxHealth = 100;
        public int MaxWidth = 200;

        public Text Text;

        public void Update()
        {
            float width = (float)GameController.Health / (float)this.MaxHealth * (float)this.MaxWidth;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 30f);

            var health = ((float)GameController.Health / (float)this.MaxHealth * 100f);
            health = Mathf.Min(Mathf.Max(health, 0), this.MaxHealth);
            this.Text.text = string.Format("{0}", (int)health);
        }
    }
}