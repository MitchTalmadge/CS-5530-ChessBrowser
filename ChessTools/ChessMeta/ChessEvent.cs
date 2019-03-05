using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTools.ChessMeta
{
    public class ChessEvent
    {
        public string Name { get; set; }

        public string Site { get; set; }

        public DateTime Date { get; set; }

        public ICollection<ChessGame> Games { get; } = new List<ChessGame>();

        private IDictionary<string, ChessPlayer> Players { get; } = new Dictionary<string, ChessPlayer>();

        public ChessPlayer getOrCreatePlayer(string name)
        {
            if (Players.ContainsKey(name))
                return Players[name];

            var player = new ChessPlayer {Name = name};
            Players[name] = player;

            return player;
        }
    }
}