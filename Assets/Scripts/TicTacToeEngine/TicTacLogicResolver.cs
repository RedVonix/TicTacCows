using System.Collections.Generic;
using TicTacCows;
using TicTacCows.Logging;
using TicTacCows.TicTacToeEngine;

public class TicTacLogicResolver
{
    // Being that this is an example, this is a simple one-use resolver. In a more 'long term' scenario,
    // this resolver would be built with lots of virtuals and designed in a way that it would be the foundation
    // to write different kind of resolvers to support different kinds of rulesets. Thankfully,
    // this is TicTacToe, and the rules are pretty simple here.

    private class WinCondition
    {
        public int x1, x2, x3, y1, y2, y3;
    }

    private List<WinCondition> winConditionDict = new List<WinCondition>()
    {
        // Bottom Row
        { new WinCondition(){x1 = 0, y1 = 0, 
                             x2 = 1, y2 = 0, 
                             x3 = 2, y3 = 0 } },

        // Middle Row
        { new WinCondition(){x1 = 0, y1 = 1,
                             x2 = 1, y2 = 1,
                             x3 = 2, y3 = 1 } },
        
        // Top Row
        { new WinCondition(){x1 = 0, y1 = 2,
                             x2 = 1, y2 = 2,
                             x3 = 2, y3 = 2 } },

        // Left Column
        { new WinCondition(){x1 = 0, y1 = 0,
                             x2 = 0, y2 = 1,
                             x3 = 0, y3 = 2 } },

        // Middle Row
        { new WinCondition(){x1 = 1, y1 = 0,
                             x2 = 1, y2 = 1,
                             x3 = 1, y3 = 2 } },

        // Right Column
        { new WinCondition(){x1 = 2, y1 = 0,
                             x2 = 2, y2 = 1,
                             x3 = 2, y3 = 2 } },

        // Left Diagonal
        { new WinCondition(){x1 = 0, y1 = 0,
                             x2 = 1, y2 = 1,
                             x3 = 2, y3 = 2 } },

        // Right Diagonal
        { new WinCondition(){x1 = 0, y1 = 2,
                             x2 = 1, y2 = 1,
                             x3 = 2, y3 = 0 } },
    };

    public (GameValues.RuleResolverResults logicResult, GameValues.TicTacPlayers targetPlayer) GetResults(TicTacToeBoard inBoard)
    {
        // TicTacToe logic is super simple, so we're just going to go through the possible win conditions.
        // For a far more complex game, we'd want to do something like resolve from the last placed space
        // outwards and use something like A* logic to detect patterns and such.

        foreach(WinCondition thisWinCon in winConditionDict)
        {
            // Check all 3 spaces for this win condition.
            TicTacToeSpace checkSpace = inBoard.GetRuntimeSpace(thisWinCon.x1, thisWinCon.y1);
            if(checkSpace == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Exception, "--- TicTacLogicResolver:GetResults - Failed to get a space at position " + thisWinCon.x1 + "/" + thisWinCon.y1 + " and therefore unable to resolve logic!");
                return (GameValues.RuleResolverResults.NoResults, GameValues.TicTacPlayers.NONE);
            }

            GameValues.TicTacPlayers checkPlayer = checkSpace.RUNTIME_SpaceOwner;
            if(checkPlayer != GameValues.TicTacPlayers.NONE)
            {
                if(inBoard.GetRuntimeSpace(thisWinCon.x2, thisWinCon.y2).RUNTIME_SpaceOwner == checkPlayer &&
                   inBoard.GetRuntimeSpace(thisWinCon.x3, thisWinCon.y3).RUNTIME_SpaceOwner == checkPlayer)
                {
                    // We've got a winner!
                    return (GameValues.RuleResolverResults.PlayerWins, checkPlayer);
                }
            }
        }

        // If we got here, then we found no win condition yet. So let's see if the board is filled...
        if(inBoard.IsBoardFilled())
        {
            // Board is filled, and we have no winner - it's a tie!
            return (GameValues.RuleResolverResults.Tie, GameValues.TicTacPlayers.NONE);
        }

        // No winner and no tie yet - the battle rages on!
        return (GameValues.RuleResolverResults.NoResults, GameValues.TicTacPlayers.NONE);
    }

    public void HaveAIMakeMove(TicTacToeController inController)
    {
        // The AI moving for this TicTacToe is really simple - we just choose a random space.
        // If we wanted to make the AI more advanced we could implement a MiniMax algorithm.

        // Get a list of all available spaces.
        List<TicTacToeSpace> availableSpaces = inController.gameBoard.boardSpaces.FindAll(s => s.RUNTIME_SpaceOwner == GameValues.TicTacPlayers.NONE);

        // Randomly choose one and move to it.
        TicTacToeSpace roboChosenSpace = availableSpaces[UnityEngine.Random.Range(0, availableSpaces.Count)];
        inController.PlacePieceOnSpace(roboChosenSpace);
    }
}
