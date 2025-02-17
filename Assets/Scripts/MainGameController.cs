using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacCows.Logging;
using TicTacCows.TicTacToeEngine;
using TicTacCows.Tools.SpawnSystem;
using TicTacCows.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace TicTacCows
{
    public class MainGameController : MonoBehaviour
    {
        public SpawnSystem spawnSys;
        public TicTacToeController ticTacControl;
        public UIController uiControl;

        [Header("Cinematics")]
        public Animation cinematicAnimation;
        public AnimationClip introCinematic;
        public AnimationClip FirstTurnStartClip;
        public AnimationClip Player1Turn;
        public AnimationClip Player2Turn;

        [Header("Audio References")]
        public AudioSource tractorBeamAudio;
        public AudioSource piecePlacedAudio;


        public static MainGameController singleton;

        private bool introPlayed;

        [HideInInspector] public GameValues.GameStates currentGameState { get; private set; } = GameValues.GameStates.NONE;

        void Start()
        {
            // In Unity, Start is actually a dangerous function if not treated carefully, as it's very easy to create race
            // conditions if relying on it too much. Because of that, this will be the only use of Start throughout this project.

            singleton = this;

            spawnSys.SetupForRuntime();
            ticTacControl.SetupForRuntime();

            uiControl.OpenDialog(GameValues.DialogTypes.TitleScreen);
            ChangeGameState(GameValues.GameStates.TitleScreen);
        }

        public void ChangeGameState(GameValues.GameStates inGameState)
        {
            currentGameState = inGameState;
            LoggingSystem.AddLog(GameValues.LoggingTypes.Log, "--- MainGameController:ChangeGameState - Changing game state to " + inGameState);
        }

        public void TitleButton_1Player()
        {
            StartGameWithPlayerCount(1);
        }

        public void TitleButton_2Player()
        {
            StartGameWithPlayerCount(2);
        }

        public void GameOverButton_NewGame()
        {
            uiControl.CloseDialog(GameValues.DialogTypes.PlayerOneWins);
            uiControl.CloseDialog(GameValues.DialogTypes.PlayerTwoWins);
            uiControl.CloseDialog(GameValues.DialogTypes.Draw);

            ticTacControl.gameBoard.boardSpaces.ForEach(s => s.ClearSpace());

            uiControl.OpenDialog(GameValues.DialogTypes.TitleScreen);
            ChangeGameState(GameValues.GameStates.TitleScreen);
        }

        private void StartGameWithPlayerCount(int inPlayerCount)
        {
            if(currentGameState != GameValues.GameStates.TitleScreen)
            {
                return;
            }

            uiControl.FadeOutDialog(GameValues.DialogTypes.TitleScreen);

            ticTacControl.RUNTIME_PlayerCount = inPlayerCount;
            if (introPlayed)
            {
                // Jump straight to the gameplay
                FirstTurnStart();
            }
            else
            {
                // Show intro once
                introPlayed = true;
                PerformIntro();
            }
        }

        #region Cinematic Controls
        private async void PerformIntro()
        {
            ChangeGameState(GameValues.GameStates.Cinematic);

            cinematicAnimation.clip = introCinematic;
            cinematicAnimation.Play();

            while (cinematicAnimation.isPlaying)
            {
                await Task.Delay(100);
            }

            FirstTurnStart();
        }

        public async void FirstTurnStart()
        {
            ChangeGameState(GameValues.GameStates.Cinematic);

            cinematicAnimation.clip = FirstTurnStartClip;
            cinematicAnimation.Play();

            while (cinematicAnimation.isPlaying)
            {
                await Task.Delay(100);
            }

            ticTacControl.StartNewGame();
        }

        public async void PlayerOneTurnStart()
        {
            ChangeGameState(GameValues.GameStates.Cinematic);

            cinematicAnimation.clip = Player1Turn;
            cinematicAnimation.Play();

            while (cinematicAnimation.isPlaying)
            {
                await Task.Delay(100);
            }

            ticTacControl.BeginNextMove();
        }

        public async void PlayerTwoTurnStart()
        {
            ChangeGameState(GameValues.GameStates.Cinematic);

            cinematicAnimation.clip = Player2Turn;
            cinematicAnimation.Play();

            while (cinematicAnimation.isPlaying)
            {
                await Task.Delay(100);
            }

            ticTacControl.BeginNextMove();
        }

        public async void ShowGameEnd_PlayerWon(GameValues.TicTacPlayers winPlayer)
        {
            List<TicTacToeSpace> spaceList = ticTacControl.gameBoard.boardSpaces.FindAll(s => s.RUNTIME_SpaceOwner == winPlayer && s.isWinningSpace);
            spaceList.ForEach(s =>
            {
                s.cowObj.SetActive(false);
                s.RUNTIME_PieceOnSpace.PlayBeam();
            });

            tractorBeamAudio.Play();

            await Task.Delay(2000);

            if(winPlayer == GameValues.TicTacPlayers.Player0)
            {
                uiControl.OpenDialog(GameValues.DialogTypes.PlayerOneWins);
            }
            else if (winPlayer == GameValues.TicTacPlayers.Player1)
            {
                uiControl.OpenDialog(GameValues.DialogTypes.PlayerTwoWins);
            }
        }

        public void ShowGameEnd_Draw()
        {
            uiControl.OpenDialog(GameValues.DialogTypes.Draw);
        }
        #endregion
    }
}