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

        public async Task<bool> UpdateScore(ScoreRequest request)
        {

            try
            {
                foreach(ScoreUpdateModel s in request.Matches)
                {
                    bool success = await UpdateScore(s);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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
                string commandText = "INSERT INTO {Table} (id, date, home_team, away_team, home_score, away_score) Values(null,@date @home_team, @away_team, @home_score, @away_score)";
                MySqlCommand cmd = new MySqlCommand(commandText, connection.Connection);
                cmd.Parameters.AddWithValue("@date", request.date);
                cmd.Parameters.AddWithValue("@home_team", request.homeTeam);
                cmd.Parameters.AddWithValue("@away_team", request.awayTeam);
                cmd.Parameters.AddWithValue("@home_score", request.homeScore);
                cmd.Parameters.AddWithValue("@away_score", request.awayScore);
               

                MySqlDataReader reader = cmd.ExecuteReader();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
