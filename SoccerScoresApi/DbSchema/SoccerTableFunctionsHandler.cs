using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using MySql;
using MySql.Data.MySqlClient;

using SoccerScoresApi.TableModel;
using SoccerScoresApi.RequestModel;
using SoccerScoresApi.Extension;
using SoccerScoresApi.MatchModels;

namespace SoccerScoresApi.DbSchema
{
    public class SoccerTableFunctionsHandler
    {
        private const string Table = "soccer_table";

        public static async Task<List<ScoreTable>> GetMatchesForTeam(string name)
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

        public static async Task<bool> DeleteEmptyScores()
        {
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                string commandText = $"DELETE FROM {Table} WHERE (home_score = @home_score AND away_score = @away_score);";
                MySqlCommand cmd = new MySqlCommand(commandText, connection.Connection);
                cmd.Parameters.AddWithValue("@home_score", null);
                cmd.Parameters.AddWithValue("@away_score", null);
                int rowsAffected = cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                await connection.Disconnect();
            }
        }
        public static Dictionary<string, object> SerializeReader(MySqlDataReader reader)
        {
            var results = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                results.Add(reader.GetName(i), reader.GetValue(i));
            }
            return results;
        }
        private static ScoreTable MySqlDataReaderToScore(MySqlDataReader reader)
        {
            Dictionary<string, object> dict = SerializeReader(reader);

            int id = Int32.Parse(reader["id"].ToString());
            string date = reader["date"].ToString();
            string homeTeam = reader["home_team"].ToString();
            string awayTeam = reader["away_team"].ToString();

            _ = dict.TryGetValue("home_score", out object home_obj);
            _ = dict.TryGetValue("away_score", out object away_obj);

            int? homeScore = null;
            int? awayScore = null;
            if(home_obj != null)
            {
                homeScore = Int32.Parse(home_obj.ToString());
            }
            if(away_obj != null)
            {
                awayScore = Int32.Parse(away_obj.ToString());
            }
            
            return new ScoreTable(id, date, homeTeam, awayTeam, homeScore, awayScore);
        }
    }
}
