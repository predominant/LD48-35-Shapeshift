using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShapeShift;
using UnityEngine.Analytics;

namespace ShapeShift.Player
{
    [RequireComponent(typeof (Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        private float _moveSpeed = 2f;
        private float _rotateSpeed = 0f;
        public float SquareSpeed = 2f;
        public float CircleSpeed = 3f;
        public float RocketSpeed = 0f;

        public float RocketForce = 3f;
        public float RocketRotateRate = 1f;
        public float JumpForce = 10f;

        public float DistanceScoreMultiple = 3f;

        public float BulletDamage = 20f;

        public BoxCollider2D RocketCollider;
        public BoxCollider2D SquareCollider;
        public CircleCollider2D CircleCollider;

        public GameObject RocketFlames;
        public ParticleSystem MorphParticleSystem;

        public GameController GameController;

        public RigidbodyConstraints2D SquareConstraints;
        public RigidbodyConstraints2D CircleConstraints;
        public RigidbodyConstraints2D RocketConstraints;
        public PlayerClass PlayerClass = PlayerClass.Square;

        private bool _alive = true;
        private bool _grounded = false;
        private bool _handlersRegistered = false;
        private Rigidbody2D _rigidbody;
        private float _currentSpeed = 0f;
        private Animator _animator;
        private float _rocketForce = 0f;

        private Vector2 _startPosition = Vector2.zero;

        public void Start()
        {
            this.Init();
        }

        public void Awake()
        {
            this.Init();
        }

        private void Init()
        {
            this._startPosition = this.transform.position;
            this.RegisterHandlers();
            this._rigidbody = this.GetComponent<Rigidbody2D>();
            this._animator = this.GetComponent<Animator>();
            this.ActivateSquare();
        }

        public void OnDisable()
        {
            this.Shutdown();
        }

        public void OnDestroy()
        {
            this.Shutdown();
        }

        private void Shutdown()
        {
            this.UnregisterHandlers();
        }

        public void RegisterHandlers()
        {
            if (this._handlersRegistered)
                return;

            this._handlersRegistered = true;

            InputController.OnMoveLeft += this.MoveLeft;
            InputController.OnStopMoveLeft += this.StopMoveLeft;
            InputController.OnMoveRight += this.MoveRight;
            InputController.OnStopMoveRight += this.StopMoveRight;

            InputController.OnAction1 += this.ActivateSquare;
            InputController.OnAction2 += this.ActivateCircle;
            InputController.OnAction3 += this.ActivateRocket;

            InputController.OnJumpStart += this.JumpStart;
            InputController.OnJumpStop += this.JumpStop;

            InputController.OnUpright += this.Upright;
        }

        public void UnregisterHandlers()
        {
            if (!this._handlersRegistered)
                return;

            InputController.OnMoveLeft -= this.MoveLeft;
            InputController.OnStopMoveLeft -= this.StopMoveLeft;
            InputController.OnMoveRight -= this.MoveRight;
            InputController.OnStopMoveRight -= this.StopMoveRight;

            InputController.OnAction1 -= this.ActivateSquare;
            InputController.OnAction2 -= this.ActivateCircle;
            InputController.OnAction3 -= this.ActivateRocket;

            InputController.OnJumpStart -= this.JumpStart;
            InputController.OnJumpStop -= this.JumpStop;

            InputController.OnUpright -= this.Upright;

            this._handlersRegistered = false;
        }

        public void FixedUpdate()
        {
            //this._rigidbody.velocity = this._currentForce;
            //this._rigidbody.AddForce(this._currentForce*this._moveSpeed);
            switch (this.PlayerClass)
            {
                case PlayerClass.Rocket:
                    this.transform.localRotation *= Quaternion.AngleAxis(this._rotateSpeed, Vector3.forward);
                    this._rigidbody.AddForce(this.transform.up * this._rocketForce);
                    break;
                default:
                    this._rigidbody.velocity = new Vector2(
                        this._currentSpeed,
                        this._rigidbody.velocity.y);
                    break;
            }
        }

        public void Update()
        {
            GameController.Score = (this.transform.position.x - this._startPosition.x) * this.DistanceScoreMultiple + 15f;
            this.HealthCheck();
        }

        private void HealthCheck()
        {
            if (GameController.Health <= 0f)
                this.Death();
        }

        private void MoveLeft()
        {
            Debug.Log("Move Left");
            switch (this.PlayerClass)
            {
                case PlayerClass.Rocket:
                    this._rotateSpeed += this.RocketRotateRate;
                    break;
                default:
                    this._currentSpeed -= this._moveSpeed;
                    break;
            }
        }

        private void MoveRight()
        {
            Debug.Log("Move Right");
            switch (this.PlayerClass)
            {
                case PlayerClass.Rocket:
                    this._rotateSpeed -= this.RocketRotateRate;
                    break;
                default:
                    this._currentSpeed += this._moveSpeed;
                    break;
            }
        }

        private void StopMoveLeft()
        {
            Debug.Log("Move Left - STOP");
            switch (this.PlayerClass)
            {
                case PlayerClass.Rocket:
                    this._rotateSpeed = 0f;
                    break;
                default:
                    if (this._currentSpeed != 0)
                        this._currentSpeed += this._moveSpeed;
                    break;
            }
        }

        private void StopMoveRight()
        {
            Debug.Log("Move Right - STOP");
            switch (this.PlayerClass)
            {
                case PlayerClass.Rocket:
                    this._rotateSpeed = 0f;
                    break;
                default:
                    if (this._currentSpeed != 0)
                        this._currentSpeed -= this._moveSpeed;
                    break;
            }
        }

        private void ActivateSquare()
        {
            this.MorphEffect();
            this._animator.SetBool("Rocket", false);
            this.RocketCollider.enabled = false;
            this._animator.SetBool("Circle", false);
            this.CircleCollider.enabled = false;

            this._animator.SetBool("Square", true);
            this.SquareCollider.enabled = true;

            this.RocketFlames.SetActive(false);

            this.PlayerClass = PlayerClass.Square;
            this._rigidbody.constraints = this.SquareConstraints;
            this.transform.localRotation = Quaternion.identity;
            this._moveSpeed = this.SquareSpeed;
            this.ResetSpeed();
        }

        private void ActivateCircle()
        {
            this.MorphEffect();
            this._animator.SetBool("Rocket", false);
            this.RocketCollider.enabled = true;
            this._animator.SetBool("Square", false);
            this.SquareCollider.enabled = true;

            this._animator.SetBool("Circle", true);
            this.CircleCollider.enabled = true;

            this.RocketFlames.SetActive(false);

            this.PlayerClass = PlayerClass.Circle;
            this._rigidbody.constraints = this.CircleConstraints;
            this.transform.localRotation = Quaternion.identity;
            this._moveSpeed = this.CircleSpeed;
            this.ResetSpeed();
        }

        private void ActivateRocket()
        {
            this.MorphEffect();
            this._animator.SetBool("Square", false);
            this.SquareCollider.enabled = true;
            this._animator.SetBool("Circle", false);
            this.CircleCollider.enabled = true;

            this._animator.SetBool("Rocket", true);
            this.RocketCollider.enabled = true;

            this.PlayerClass = PlayerClass.Rocket;
            this._rigidbody.constraints = this.RocketConstraints;
            this.transform.localRotation = Quaternion.identity;
            this._moveSpeed = this.RocketSpeed;
            this._currentSpeed = 0f;
            this.ResetSpeed();
        }

        public void RandomMorph(int i)
        {
            if (i >= 0 && i <= 9)
                if (this.PlayerClass != PlayerClass.Square)
                    this.ActivateSquare();
            if (i >= 10 && i <= 19)
                if (this.PlayerClass != PlayerClass.Circle)
                    this.ActivateCircle();
            if (i >= 20 && i <= 29)
                if (this.PlayerClass != PlayerClass.Rocket)
                    this.ActivateRocket();
        }

        private void ResetSpeed()
        {
            // current - THe speed being set to the rigidbody in FixedUpdate
            // _moveSpeed - The speed of the class currently selected
            // desired - The newly selected class speed
            if (this._currentSpeed != 0)
                this._currentSpeed = this._currentSpeed / Mathf.Abs(this._currentSpeed) * this._moveSpeed;
        }

        private void JumpStart()
        {
            Debug.Log("Jump Start");
            switch (this.PlayerClass)
            {
                case PlayerClass.Square:
                    if (this._grounded)
                    {
                        this._rigidbody.AddForce(Vector2.up*this.JumpForce, ForceMode2D.Impulse);
                        this._grounded = false;
                    }
                    break;
                case PlayerClass.Circle:
                    this._rigidbody.AddForce(new Vector2(
                        Vector2.SqrMagnitude(this._rigidbody.velocity),
                        0f));
                    break;
                case PlayerClass.Rocket:
                    this.RocketFlames.SetActive(true);
                    this._rocketForce = this.RocketForce;
                    break;
            }
        }

        private void JumpStop()
        {
            this._rocketForce = 0f;
            this.RocketFlames.SetActive(false);
        }

        private void Upright()
        {
            this.transform.localRotation = Quaternion.identity;
        }

        private void MorphEffect()
        {
            this.MorphParticleSystem.Play();
        }

        public void OnCollisionEnter2D(Collision2D c)
        {
            Debug.Log("Collision with: " + c.gameObject.name);
            if (c.gameObject.layer == GameController.LayerLand || c.gameObject.layer == GameController.LayerEnemy)
                this._grounded = true;

            if (c.gameObject.layer == GameController.LayerBullet)
            {
                GameController.Health -= this.BulletDamage;
                GameObject.Destroy(c.gameObject);
            }

            if (c.gameObject.name == "Exit Area")
            {
                this.Death(true);
            }
        }

        private void Death(bool success = false)
        {
            if (!this._alive)
                return;

            this._alive = false;
            this.UnregisterHandlers();
            this._rigidbody.isKinematic = true;
            this._rigidbody.velocity = Vector2.zero;
            this._currentSpeed = 0f;

#if UNITY_ANALYTICS
            Debug.Log(Analytics.CustomEvent("Death", new Dictionary<string, object>
            {
                { "PlayerClass", this.PlayerClass.ToString() },
                { "Score", (int)GameController.Score },
                { "Health", (int)GameController.Health },
                { "Success", success }
            }));
#endif

            this.GameController.ShowEndGame(success);
        }
    }
}