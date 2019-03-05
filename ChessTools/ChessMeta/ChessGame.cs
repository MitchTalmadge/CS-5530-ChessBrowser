using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTools.ChessMeta;

namespace ChessTools.ChessMeta
{
    public class ChessGame
    {
        public enum GameResult
        {
            Draw,
            Black,
            White
        }

        public ChessEvent Event { get; set; }

        public ChessPlayer BlackPlayer { get; set; }

        public ChessPlayer WhitePlayer { get; set; }

        public GameResult Result { get; private set; }

        public string Moves { get; set; }

        public void setResult(string result)
        {
            if (result.Equals("1/2-1/2"))
                Result = GameResult.Draw;
            else if (result.Equals("1-0"))
                Result = GameResult.White;
            else
                Result = GameResult.Black;
        }

    }
}