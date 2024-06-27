namespace TicTacCows
{
    public class GameValues
    {
        public const int BoardSize_X = 3;
        public const int BoardSize_Y = 3;

        public enum DialogTypes
        {
            NONE = 0,

        }

        public enum TicTacToePieceTypes
        {
            NONE = 0,
            X = 1,
            O = 2,
        }

        public enum TicTacPlayers
        {
            NONE = 0,
            Player0 = 1,
            Player1 = 2,
        }

        public enum GameStates
        {
            NONE = 0,
            PlayerChoosingSpace = 1,
            ResolvingPiecePlacement = 2,
            Cinematic = 3,
        }

        public enum SpaceStates
        {
            Empty = 0,
            Populated = 1
        }

        public enum RuleResolverResults
        {
            NoResults = 0,
            PlayerWins = 1,
            Tie = 2,
        }

        public enum LoggingTypes
        {
            NONE = 0,
            Log = 1,
            Warn = 2,
            Error = 3,
            Exception = 4
        }
    }
}