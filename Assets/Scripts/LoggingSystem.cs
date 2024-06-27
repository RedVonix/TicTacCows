using UnityEngine;

namespace TicTacCows.Logging
{
    public class LoggingSystem
    {
        public static void AddLog(GameValues.LoggingTypes inLogType, string inMessage)
        {
            // All game logging goes through a standardized Logging System. This allows us
            // to easily integrate things like an online logging system and error reporting
            // as needed for the project.

            switch (inLogType)
            {
                case GameValues.LoggingTypes.Log:
                    Debug.Log(inMessage);
                break;

                case GameValues.LoggingTypes.Warn:
                    Debug.LogWarning(inMessage);
                break;

                case GameValues.LoggingTypes.Error:
                    Debug.LogError(inMessage);
                break;

                case GameValues.LoggingTypes.Exception:
                    Debug.LogError(inMessage);
                break;
            }
        }
    }
}