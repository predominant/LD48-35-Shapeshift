using UnityEngine;
using System.Collections;

namespace ShapeShift.Camera
{
    public class FollowPlayer : MonoBehaviour
    {
        public GameObject Player;
        public float FollowSpeed = 2.5f;

        private float _playerX;

        public void Start()
        {
            this._playerX = this.Player.transform.position.x;
        }

        public void FixedUpdate()
        {
            this._playerX = this.Player.transform.position.x;
        }

        public void Update()
        {
            this.transform.localPosition = Vector3.Lerp(
                this.transform.position,
                new Vector3(
                    this._playerX,
                    this.transform.position.y,
                    this.transform.position.z),
                Time.deltaTime*this.FollowSpeed);
        }
    }
}