using UnityEngine;

namespace TicTacCows.TicTacToeEngine
{
    public class TicTacToePiece : MonoBehaviour
    {
        public GameValues.TicTacToePieceTypes pieceType = GameValues.TicTacToePieceTypes.NONE;
        public Animation beamAnim;

        public void PlayBeam()
        {
            beamAnim.Play();
        }
    }
}