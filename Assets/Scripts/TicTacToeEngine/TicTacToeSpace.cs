using System.Collections;
using System.Collections.Generic;
using TicTacCows.Tools.SpawnSystem;
using UnityEngine;

namespace TicTacCows.TicTacToeEngine
{
    [ExecuteInEditMode]
    public class TicTacToeSpace : MonoBehaviour
    {
        public int spaceX = 0;
        public int spaceY = 0;
        public bool isWinningSpace = false;
        public GameObject cowObj;

        public TicTacToePiece RUNTIME_PieceOnSpace;
        public GameValues.SpaceStates RUNTIME_CurrentSpaceState { get; private set; } = GameValues.SpaceStates.Empty;
        public GameValues.TicTacPlayers RUNTIME_SpaceOwner { get; private set; } = GameValues.TicTacPlayers.NONE;

        void Start()
        {
            // NOTE: Typically I avoid the use of Start because of race conditions, but in this case we are just
            //       clearing this space which has no dependencies, so it's safe and gets us the visual state we want
            //       at the game start.
            cowObj.transform.localEulerAngles = new Vector3(0f, Random.Range(0f, 359f), 0f);
        }

        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                string targetName = $"Space_{spaceX},{spaceY}";
                if(gameObject.name != targetName)
                {
                    gameObject.name = targetName;
                }
            }
#endif
        }

        public void PiecePlacedOnSpace(GameValues.TicTacPlayers inPlayer, TicTacToePiece pieceOnSpace)
        {
            RUNTIME_CurrentSpaceState = GameValues.SpaceStates.Populated;
            RUNTIME_SpaceOwner = inPlayer;
            RUNTIME_PieceOnSpace = pieceOnSpace;
        }

        public void ClearSpace()
        {
            RUNTIME_CurrentSpaceState = GameValues.SpaceStates.Empty;
            RUNTIME_SpaceOwner = GameValues.TicTacPlayers.NONE;
            isWinningSpace = false;
            cowObj.SetActive(true);

            if (RUNTIME_PieceOnSpace != null)
            {
                SpawnSystem.singleton.ReturnObjectToPool(RUNTIME_PieceOnSpace.gameObject);
                RUNTIME_PieceOnSpace = null;
            }
        }
    }
}