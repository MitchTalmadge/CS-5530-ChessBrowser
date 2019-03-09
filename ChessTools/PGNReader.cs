using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChessTools.ChessMeta;

namespace ChessTools
{
    // ReSharper disable once InconsistentNaming
    public class PGNReader
    {
        private static readonly Regex EventNameRegex =
            new Regex("\\[Event \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex EventSiteNameRegex =
            new Regex("\\[Site \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex EventDateRegex =
            new Regex("\\[EventDate \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex BlackPlayerRegex =
            new Regex("\\[Black \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex WhitePlayerRegex =
            new Regex("\\[White \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex BlackPlayerEloRegex =
            new Regex("\\[BlackElo \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex WhitePlayerEloRegex =
            new Regex("\\[WhiteElo \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex ResultRegex =
            new Regex("\\[Result \"(.+)\"\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // ReSharper disable once InconsistentNaming
        public static Dictionary<string, ChessEvent>.ValueCollection ParseEventsFromPGN(string fileName)
        {
            // Maps event names to their instances.
            var eventsMap = new Dictionary<string, ChessEvent>();

            var file = new StreamReader(fileName);
            string line;

            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("[Event "))
                {
                    // Read all tags into metadata string.
                    var metadataBuilder = new StringBuilder(line).AppendLine();

                    while (!string.IsNullOrWhiteSpace(line = file.ReadLine()))
                    {
                        metadataBuilder.AppendLine(line);
                    }

                    var metadata = metadataBuilder.ToString();

                    // Parse metadata.
                    var eventName = EventNameRegex.Match(metadata).Groups[1].Value;
                    var siteName = EventSiteNameRegex.Match(metadata).Groups[1].Value;
                    var eventDate = EventDateRegex.Match(metadata).Groups[1].Value;

                    var blackPlayerName = BlackPlayerRegex.Match(metadata).Groups[1].Value;
                    var whitePlayerName = WhitePlayerRegex.Match(metadata).Groups[1].Value;
                    var blackPlayerElo = int.Parse(BlackPlayerEloRegex.Match(metadata).Groups[1].Value);
                    var whitePlayerElo = int.Parse(WhitePlayerEloRegex.Match(metadata).Groups[1].Value);

                    var result = ResultRegex.Match(metadata).Groups[1].Value;

                    // Find or create event.
                    eventsMap.TryGetValue(eventName, out var chessEvent);
                    if (chessEvent == null)
                    {
                        chessEvent = new ChessEvent
                            {Name = eventName, Site = siteName, Date = eventDate};
                        eventsMap[eventName] = chessEvent;
                    }

                    // Create and assign game.
                    var game = new ChessGame {Event = chessEvent};
                    game.setResult(result);
                    chessEvent.Games.Add(game);

                    // Read and set game moves.
                    var gameMovesBuilder = new StringBuilder();
                    while (!string.IsNullOrWhiteSpace(line = file.ReadLine()))
                    {
                        gameMovesBuilder.Append(line);
                    }
                    game.Moves = gameMovesBuilder.ToString();

                    // Update and assign players.
                    var blackPlayer = chessEvent.getOrCreatePlayer(blackPlayerName);
                    blackPlayer.ELO = blackPlayer.ELO < blackPlayerElo ? blackPlayerElo : blackPlayer.ELO;
                    game.BlackPlayer = blackPlayer;
                    blackPlayer.Games.Add(game);

                    var whitePlayer = chessEvent.getOrCreatePlayer(whitePlayerName);
                    whitePlayer.ELO = whitePlayer.ELO < whitePlayerElo ? whitePlayerElo : whitePlayer.ELO;
                    game.WhitePlayer = whitePlayer;
                    whitePlayer.Games.Add(game);
                }
            }

            return eventsMap.Values;
        }
    }
}