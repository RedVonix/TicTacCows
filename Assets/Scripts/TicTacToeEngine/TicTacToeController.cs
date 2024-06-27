using TicTacCows.Logging;
using TicTacCows.Tools.SpawnSystem;
using UnityEngine;

namespace TicTacCows.TicTacToeEngine
{
    public class TicTacToeController : MonoBehaviour
    {
        public TicTacToeBoard gameBoard;

        [Header("Prefab References")]
        public GameObject xGamePiece;
        public GameObject oGamePiece;
        
        private GameValues.TicTacPlayers RUNTIME_CurrentPlayer;
        private TicTacLogicResolver RUNTIME_logicResolver = new TicTacLogicResolver();

        public void SetupForRuntime()
        {
            gameBoard.SetupForRuntime();
        }

        public void StartNewGame()
        {
            gameBoard.ResetBoardForNewGame();
            BeginFirstGameRound();
        }

        private void SwapActivePlayer()
        {
            if (RUNTIME_CurrentPlayer == GameValues.TicTacPlayers.Player0)
            {
                RUNTIME_CurrentPlayer = GameValues.TicTacPlayers.Player1;
            }
            else
            {
                RUNTIME_CurrentPlayer = GameValues.TicTacPlayers.Player0;
            }        
        }

        public void SpaceInteracted(TicTacToeSpace inSpace)
        {
            if(MainGameController.singleton.currentGameState != GameValues.GameStates.PlayerChoosingSpace ||
               inSpace.RUNTIME_CurrentSpaceState == GameValues.SpaceStates.Populated)
            {
                return;
            }

            PlacePieceOnSpace(inSpace);
            ResolvePiecePlacement(inSpace);
        }

        private void PlacePieceOnSpace(TicTacToeSpace inSpace)
        {
            GameObject piecePrefab = GetPiecePrefabForCurrentPlayer();
            if(piecePrefab == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Error, "--- TicTacToeController:PlacePieceOnSpace - Failed to place piece as piece prefab was null!");
                return;
            }

            LoggingSystem.AddLog(GameValues.LoggingTypes.Log, $"--- TicTacToeController:PlacePieceOnSpace - {RUNTIME_CurrentPlayer} placed piece on space {inSpace.spaceX}/{inSpace.spaceY}");
            GameObject spawnedPiece = SpawnSystem.singleton.SpawnFromPrefab(piecePrefab, inSpace.transform);
            inSpace.PiecePlacedOnSpace(RUNTIME_CurrentPlayer, spawnedPiece);
        }

        private GameObject GetPiecePrefabForCurrentPlayer()
        {
            if(RUNTIME_CurrentPlayer == GameValues.TicTacPlayers.Player0)
            {
                return xGamePiece;
            }
            else
            {
                return oGamePiece;
            }
        }

        private void ResolvePiecePlacement(TicTacToeSpace inSpacePlaced)
        {
            MainGameController.singleton.ChangeGameState(GameValues.GameStates.ResolvingPiecePlacement);

            // Let's see if anybody won...
            (GameValues.RuleResolverResults logicResult, GameValues.TicTacPlayers targetPlayer) = RUNTIME_logicResolver.GetResults(gameBoard);
            Debug.Log("RULE RESULT: " + logicResult + " / " + targetPlayer);
            switch (logicResult)
            {
                case GameValues.RuleResolverResults.NoResults:
                    BeginNewGameRound();
                break;

                case GameValues.RuleResolverResults.PlayerWins:
                    ResolveGameEnd(GameValues.RuleResolverResults.PlayerWins, targetPlayer);
                break;

                case GameValues.RuleResolverResults.Tie:
                    ResolveGameEnd(GameValues.RuleResolverResults.Tie);
                break;
            }
        }

        private void BeginFirstGameRound()
        {
            RUNTIME_CurrentPlayer = GameValues.TicTacPlayers.Player0;
            MainGameController.singleton.ChangeGameState(GameValues.GameStates.PlayerChoosingSpace);
        }

        private void BeginNewGameRound()
        {
            MainGameController.singleton.ChangeGameState(GameValues.GameStates.PlayerChoosingSpace);
            SwapActivePlayer();
        }

        private void ResolveGameEnd(GameValues.RuleResolverResults winResult, GameValues.TicTacPlayers winningPlayer = GameValues.TicTacPlayers.NONE)
        {
            StartNewGame();
        }
    }
}