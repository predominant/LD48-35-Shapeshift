using UnityEngine;
using System.Collections;
using ShapeShift.Player;

namespace ShapeShift.Enemy
{
    public class EnemyTarget : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D c)
        {
            if (c.gameObject.layer == GameController.LayerPlayer)
                this.SetTarget(c.gameObject);
        }

        private void SetTarget(GameObject g)
        {
            this.transform.parent.GetComponent<Tank>().Target = g.GetComponent<PlayerController>();
        }
    }
}