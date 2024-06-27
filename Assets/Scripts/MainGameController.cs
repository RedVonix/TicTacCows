using System.Threading.Tasks;
using TicTacCows.Logging;
using TicTacCows.TicTacToeEngine;
using TicTacCows.Tools.SpawnSystem;
using UnityEngine;

namespace TicTacCows
{
    public class MainGameController : MonoBehaviour
    {
        public SpawnSystem spawnSys;
        public TicTacToeController ticTacControl;

        [Header("Cinematics")]
        public Animation cinematicAnimation;
        public AnimationClip introCinematic;

        public static MainGameController singleton;

        [HideInInspector] public GameValues.GameStates currentGameState { get; private set; } = GameValues.GameStates.NONE;

        void Start()
        {
            // In Unity, Start is actually a dangerous function if not treated carefully, as it's very easy to create race
            // conditions if relying on it too much. Because of that, this will be the only use of Start throughout this project.

            singleton = this;

            spawnSys.SetupForRuntime();
            ticTacControl.SetupForRuntime();

            // Show intro once when the game starts.
            PerformIntro();
        }

        public void ChangeGameState(GameValues.GameStates inGameState)
        {
            currentGameState = inGameState;
            LoggingSystem.AddLog(GameValues.LoggingTypes.Log, "--- MainGameController:ChangeGameState - Changing game state to " + inGameState);
        }

        private async void PerformIntro()
        {
            ChangeGameState(GameValues.GameStates.Cinematic);

            cinematicAnimation.clip = introCinematic;
            cinematicAnimation.Play();

            while(cinematicAnimation.isPlaying)
            {
                await Task.Delay(100);
            }

            ticTacControl.StartNewGame();
        }
    }
}