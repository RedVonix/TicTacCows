using UnityEngine;

namespace TicTacCows
{
    public class CameraController : MonoBehaviour
    {
        public delegate void CameraEvent();
        public static event CameraEvent TransitionCompleted;

        public Transform camTransform;
        public Transform gameplayPosition;
        public Transform cinematicPosition;

        private bool transitionActive = false;
        private Transform targetTransform = null;
        private float moveSpeed = 0.01f;

        void Update()
        {
            if (transitionActive)
            {
                camTransform.localPosition = Vector3.MoveTowards(camTransform.localPosition, targetTransform.localPosition, moveSpeed);
                camTransform.localEulerAngles = Vector3.MoveTowards(camTransform.localEulerAngles, targetTransform.localEulerAngles, moveSpeed);
                if (camTransform.localPosition == targetTransform.localPosition && camTransform.localEulerAngles == targetTransform.localEulerAngles)
                {
                    CompleteTransition();
                }
            }
        }

        public void MoveCamToGameplay()
        {
            transitionActive = true;
            targetTransform = gameplayPosition;
        }

        public void JumpCamToGameplay()
        {
            JumpCamToNode(gameplayPosition);
        }

        public void MoveCamToCinematic()
        {
            transitionActive = true;
            targetTransform = cinematicPosition;
        }

        public void JumpCamToCinematic()
        {
            JumpCamToNode(cinematicPosition);
        }

        private void CompleteTransition()
        {
            transitionActive = false;
            targetTransform = null;
            TransitionCompleted?.Invoke();
        }

        private void JumpCamToNode(Transform inNode)
        {
            camTransform.localPosition = inNode.localPosition;
            camTransform.localEulerAngles = inNode.localEulerAngles;
        }
    }
}