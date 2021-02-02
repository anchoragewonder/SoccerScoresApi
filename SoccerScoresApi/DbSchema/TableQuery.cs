using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using MySql;
using MySql.Data.MySqlClient;

using SoccerScoresApi.TableModel;
using SoccerScoresApi.ResponseModel;
using SoccerScoresApi.Extension;
using SoccerScoresApi.RequestModel;

namespace SoccerScoresApi.DbSchema
{
    public class TableQuery
    {
        private const string Table = "soccer_table";

        public async Task<bool> UpdateScore(ScoreResponseModel request)
        {
            foreach (ScoreUpdateModel s in request.Matches)
            {
                bool success = await UpdateScore(s);

                if (!success)
                {
                    return false;
                }
            }
            return true;
        }


        public async Task<bool> UpdateScore(ScoreUpdateModel request)
        {   
            
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                //python scraping inputs go here
                string commandText = $"INSERT INTO {Table} (id, date, home_team, away_team, home_score, away_score) VALUES(null, @date, @home_team, @away_team, @home_score, @away_score);";
                MySqlCommand cmd = new MySqlCommand(commandText, connection.Connection);
                cmd.Parameters.AddWithValue("@date", request.date);
                cmd.Parameters.AddWithValue("@home_team", request.homeTeam);
                cmd.Parameters.AddWithValue("@away_team", request.awayTeam);
                cmd.Parameters.AddWithValue("@home_score", request.homeScore);
                cmd.Parameters.AddWithValue("@away_score", request.awayScore);
               
                MySqlDataReader reader = cmd.ExecuteReader();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<ScoreTable>> GetTeam(string name)
        {
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                string commandText = $"Select * FROM {Table} WHERE home_team=@name OR away_team=@name";
                MySqlCommand cmd = new MySqlCommand(commandText, connection.Connection);
                cmd.Parameters.AddWithValue("@name", name);
                MySqlDataReader reader = cmd.ExecuteReader();

                List<ScoreTable> teamGamesScore = new List<ScoreTable>();
                while (reader.Read())
                {
                    ScoreTable team = MySqlDataReaderToScore(reader);
                    teamGamesScore.Add(team);
                }
                reader.Close();
                await connection.Disconnect();
                return teamGamesScore;
            }
            catch(Exception ex)
            {
                Console.WriteLine("exeption caught", ex);
                throw ex;
            }
        }

        public Dictionary<string, object> SerializeReader(MySqlDataReader reader)
        {
            var results = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                results.Add(reader.GetName(i), reader.GetValue(i));
            }
            return results;
        }
        private ScoreTable MySqlDataReaderToScore(MySqlDataReader reader)
        {
            Dictionary<string, object> dict = SerializeReader(reader);

            int id = Int32.Parse(reader["id"].ToString());
            string date = reader["date"].ToString();
            string homeTeam = reader["home_team"].ToString();
            string awayTeam = reader["away_team"].ToString();
            int homeScore = Int32.Parse(reader["home_score"].ToString());
            int awayScore = Int32.Parse(reader["away_score"].ToString());

            return new ScoreTable(id, date, homeTeam, awayTeam, homeScore, awayScore);

        }
    }
}
