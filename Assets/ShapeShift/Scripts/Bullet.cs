using UnityEngine;
using System.Collections;

namespace ShapeShift
{
    public class Bullet : MonoBehaviour
    {
        public float Lifetime = 1.5f;

        public void Awake()
        {
            GameObject.Destroy(this.gameObject, this.Lifetime);
        }
    }
}