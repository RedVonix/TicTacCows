using System.Collections.Generic;
using UnityEngine;
using TicTacCows.Logging;

namespace TicTacCows.TicTacToeEngine
{
    public class TicTacToeBoard : MonoBehaviour
    {
        public List<TicTacToeSpace> boardSpaces = new List<TicTacToeSpace>();

        private TicTacToeSpace[,] Runtime_SpaceArray;

        public void SetupForRuntime()
        {
            // Setup the quick lookup array on game start to allow for easier game modifications.
            Runtime_SpaceArray = new TicTacToeSpace[GameValues.BoardSize_X, GameValues.BoardSize_Y];
            for(int x = 0; x < GameValues.BoardSize_X; x++)
            {
                for (int y = 0; y < GameValues.BoardSize_Y; y++)
                {
                    Runtime_SpaceArray[x, y] = GetBoardSpaceFromList(x, y);
                    if(Runtime_SpaceArray[x, y] == null)
                    {
                        // Throw an error but don't abort here in case there are more errors, ensuring we can see them all.
                        LoggingSystem.AddLog(GameValues.LoggingTypes.Exception, "--- TicTacToeBoard:SetupBoard - Failed to generate board due to space object missing at " + x + " / " + y);
                    }
                }
            }
        }

        public void ResetBoardForNewGame()
        {
            int spaceCount = boardSpaces.Count;
            for (int i = 0; i < spaceCount; i++)
            {
                boardSpaces[i].ClearSpace();
            }
        }

        private TicTacToeSpace GetBoardSpaceFromList(int spaceX, int spaceY)
        {
            TicTacToeSpace space = boardSpaces.Find(s => s.spaceX == spaceX && s.spaceY == spaceY);
            if(space == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Exception, "--- TicTacToeBoard:GetBoardSpaceFromList - No space object was found in boardSpaces of position " + spaceX + " / " + spaceY);
                return null;
            }

            return space;
        }

        public TicTacToeSpace GetRuntimeSpace(int spaceX, int spaceY)
        {
            return Runtime_SpaceArray[spaceX, spaceY];
        }

        public bool IsBoardFilled()
        {
            int spaceCount = boardSpaces.Count;
            for(int i = 0; i < spaceCount; i++)
            {
                if (boardSpaces[i].RUNTIME_SpaceOwner == GameValues.TicTacPlayers.NONE)
                {
                    return false;
                }
            }

            return true;
        }
    }
}