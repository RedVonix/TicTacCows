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

        public GameObject RUNTIME_PieceOnSpace;
        public GameValues.SpaceStates RUNTIME_CurrentSpaceState { get; private set; } = GameValues.SpaceStates.Empty;
        public GameValues.TicTacPlayers RUNTIME_SpaceOwner { get; private set; } = GameValues.TicTacPlayers.NONE;

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

        public void PiecePlacedOnSpace(GameValues.TicTacPlayers inPlayer, GameObject pieceOnSpace)
        {
            RUNTIME_CurrentSpaceState = GameValues.SpaceStates.Populated;
            RUNTIME_SpaceOwner = inPlayer;
            RUNTIME_PieceOnSpace = pieceOnSpace;
        }

        public void ClearSpace()
        {
            RUNTIME_CurrentSpaceState = GameValues.SpaceStates.Empty;
            RUNTIME_SpaceOwner = GameValues.TicTacPlayers.NONE;

            if (RUNTIME_PieceOnSpace != null)
            {
                SpawnSystem.singleton.ReturnObjectToPool(RUNTIME_PieceOnSpace);
                RUNTIME_PieceOnSpace = null;
            }
        }
    }
}