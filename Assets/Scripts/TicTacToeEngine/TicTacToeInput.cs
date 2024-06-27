using TicTacCows.Logging;
using UnityEngine;

namespace TicTacCows.TicTacToeEngine
{
    public class TicTacToeInput : MonoBehaviour
    {
        public Camera inputCamera;
        public LayerMask hitMask;
        public TicTacToeController ticTacControl;

        void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            // So this is a very simplified input system just for this example, that assumes your input 
            // is always coming from a mouse. In a production situation this should probably check for things
            // like touch screen input, controllers, etc.

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray inputRay = inputCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, hitMask))
                {
                    TicTacToeSpace hitSpace = hit.collider.gameObject.GetComponent<TicTacToeSpace>();
                    if (hitSpace == null)
                    {
                        LoggingSystem.AddLog(GameValues.LoggingTypes.Warn, "--- TicTactToeInput::UpdateInput - User input found object but it was not a TicTacToeSpace!");
                    }
                    else
                    {
                        ticTacControl.SpaceInteracted(hitSpace);
                    }
                }
            }
        }
    }
}