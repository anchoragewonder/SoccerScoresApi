using MySql.Data.MySqlClient;
using SoccerScoresApi.Extension;
using SoccerScoresApi.RequestModel;
using SoccerScoresApi.ResponseModel;
using SoccerScoresApi.TableModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoccerScoresApi.DbSchema
{
    public class FixturesQuery
    {
        private const string Table = "fixtures";

        public async Task<bool> UpdateFixture(FixtureRequest request)
        {
            foreach (FixtureUpdateModel s in request.Matches)
            {
                bool success = await UpdateFixture(s);

                if (!success)
                {
                    return false;
                }
            }
            return true;
        }


        public async Task<bool> UpdateFixture(FixtureUpdateModel request)
        {

            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                //python scraping inputs go here
                string commandText = "INSERT INTO {Table} (id, date, home_team, away_team) Values(null,@date @home_team, @away_team)";
                MySqlCommand cmd = new MySqlCommand(commandText, connection.Connection);
                cmd.Parameters.AddWithValue("@date", request.date);
                cmd.Parameters.AddWithValue("@home_team", request.homeTeam);
                cmd.Parameters.AddWithValue("@away_team", request.awayTeam);
                

                MySqlDataReader reader = cmd.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<List<FixtureTable>> GetTeam(string name)
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

                List<FixtureTable> teamMatches = new List<FixtureTable>();
                while (reader.Read())
                {
                    FixtureTable team = MySqlDataReaderFixtures(reader);
                    teamMatches.Add(team);
                }
                reader.Close();
                await connection.Disconnect();
                return teamMatches;
            }
            catch (Exception ex)
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
        private FixtureTable MySqlDataReaderFixtures(MySqlDataReader reader)
        {
            Dictionary<string, object> dict = SerializeReader(reader);

            int id = Int32.Parse(reader["id"].ToString());
            string date = reader["date"].ToString();
            string homeTeam = reader["home_team"].ToString();
            string awayTeam = reader["away_team"].ToString();
            

            return new FixtureTable(id, date, homeTeam, awayTeam);

        }
    }
}
}
