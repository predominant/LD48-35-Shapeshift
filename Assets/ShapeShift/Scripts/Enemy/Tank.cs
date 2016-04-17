using UnityEngine;
using System.Collections;
using System.Security.Principal;
using ShapeShift.Player;

namespace ShapeShift.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Tank : MonoBehaviour
    {
        public float Speed = 2f;
        public float ShootDelay = 2f;
        public PlayerController Target;
        public Animator ExplosionAnimator;
        public GameObject BulletPrefab;
        public float BulletSpeed = 5f;

        private bool _alive = true;
        private IEnumerator _wanderCoroutine;
        private IEnumerator _shootCoroutine;
        private Rigidbody2D _rigidbody;
        private float _currentSpeed = 0f;
        private SpriteRenderer _renderer;
        private Animator _animator;

        public void Start()
        {
            this._rigidbody = this.GetComponent<Rigidbody2D>();
            this._renderer = this.GetComponent<SpriteRenderer>();
            this._animator = this.GetComponent<Animator>();

            this._wanderCoroutine = this.Wander(10f);
            StartCoroutine(this._wanderCoroutine);

            this._shootCoroutine = this.Shoot(Random.Range(this.ShootDelay - 0.5f, this.ShootDelay + 0.5f));
            StartCoroutine(this._shootCoroutine);
        }

        public void Update()
        {
            if (this._rigidbody.velocity.x > 0 && !this._renderer.flipX)
                this._renderer.flipX = true;
            else if (this._rigidbody.velocity.x < 0 && this._renderer.flipX)
                this._renderer.flipX = false;

            if (this.Target != null)
            {
                var vel = new Vector2(
                    (this.Target.gameObject.transform.position - this.transform.position).normalized.x * this.Speed,
                    this._rigidbody.velocity.y);

                this._rigidbody.velocity = vel;
            }
        }

        public void FixedUpdate()
        {
            if (Mathf.Abs(this._currentSpeed) > 0.1f)
                this._rigidbody.velocity = this.transform.right * this._currentSpeed;
        }

        private IEnumerator Wander(float wait)
        {
            while (this._alive && this.Target == null)
            {
                this._currentSpeed = Random.Range(-1f, 1f) * this.Speed;
                yield return new WaitForSeconds(wait);
            }
            this._currentSpeed = 0f;
            Debug.Log("Ending wander");
        }

        public void OnCollisionEnter2D(Collision2D c)
        {
            if (c.gameObject.layer == GameController.LayerPlayer)
                if (c.gameObject.GetComponent<PlayerController>().PlayerClass == PlayerClass.Circle)
                    this.Destroy();
        }

        private void Destroy()
        {
            this._alive = false;
            StopCoroutine(this._shootCoroutine);
            this.ExplosionAnimator.SetTrigger("Explode");
            this._rigidbody.velocity = Vector2.zero;
            this._rigidbody.isKinematic = true;
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            GameObject.Destroy(this.gameObject, 1f);
        }

        private IEnumerator Shoot(float wait)
        {
            while (this._alive)
            {
                if (this.Target != null)
                {
                    this.DoShoot();
                }
                yield return new WaitForSeconds(wait);
            }
        }

        private void DoShoot()
        {
            var bullet = (GameObject)GameObject.Instantiate(this.BulletPrefab, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = (this.Target.transform.position - this.transform.position).normalized * this.BulletSpeed;
            this._animator.SetTrigger("Shooting");
        }
    }
}