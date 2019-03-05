using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTools.ChessMeta
{
    public class ChessPlayer
    {

        public string Name { get; set; }

        // ReSharper disable once InconsistentNaming
        public int ELO { get; set; } = 0; 

        public ICollection<ChessGame> Games { get; } = new List<ChessGame>();

    }
}
