using UnityEngine;
using System.Collections;

namespace ShapeShift
{
    public class InputController : MonoBehaviour
    {
        public delegate void MoveLeft();
        public static event MoveLeft OnMoveLeft;
        public delegate void StopMoveLeft();
        public static event StopMoveLeft OnStopMoveLeft;

        public delegate void MoveRight();
        public static event MoveLeft OnMoveRight;
        public delegate void StopMoveRight();
        public static event StopMoveRight OnStopMoveRight;

        public delegate void Upright();
        public static event Upright OnUpright;

        public delegate void Action1();
        public static event Action1 OnAction1;
        public delegate void Action2();
        public static event Action2 OnAction2;
        public delegate void Action3();
        public static event Action3 OnAction3;

        public delegate void JumpStart();
        public static event JumpStart OnJumpStart;
        public delegate void JumpStop();
        public static event JumpStop OnJumpStop;

        public void Update()
        {
            this.KeyboardEvents();
            this.MouseEvents();
        }

        private void KeyboardEvents()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                if (OnMoveLeft != null)
                    OnMoveLeft();
            if (Input.GetKeyUp(KeyCode.LeftArrow))
                if (OnStopMoveLeft != null)
                    OnStopMoveLeft();

            if (Input.GetKeyDown(KeyCode.RightArrow))
                if (OnMoveRight != null)
                    OnMoveRight();
            if (Input.GetKeyUp(KeyCode.RightArrow))
                if (OnStopMoveRight != null)
                    OnStopMoveRight();

            if (Input.GetKeyDown(KeyCode.UpArrow))
                if (OnUpright != null)
                    OnUpright();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                if (OnAction1 != null)
                    OnAction1();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                if (OnAction2 != null)
                    OnAction2();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                if (OnAction3 != null)
                    OnAction3();

            if (Input.GetKeyDown(KeyCode.Space))
                if (OnJumpStart != null)
                    OnJumpStart();
            if (Input.GetKeyUp(KeyCode.Space))
                if (OnJumpStop != null)
                    OnJumpStop();
        }

        private void MouseEvents()
        {

        }
    }
}