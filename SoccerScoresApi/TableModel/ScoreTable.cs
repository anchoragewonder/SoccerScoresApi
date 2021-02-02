using System;
using System.Collections.Generic;
using System.Text;

namespace SoccerScoresApi.TableModel
{
    public class ScoreTable
    {
        public int id { get; set; }
        public string date { get; set; }
        public string homeTeam { get; set; }
        public string awayTeam { get; set; }
        public int? homeScore { get; set; }
        public int? awayScore { get; set; }

        public ScoreTable() { }

        public ScoreTable(int id, string date, string homeTeam, string awayTeam, int? homeScore, int? awayScore)
        {
            this.id = id;
            this.date = date;
            this.homeTeam = homeTeam;
            this.awayTeam = awayTeam;
            this.homeScore = homeScore;
            this.awayScore = awayScore;
        }
    }
}