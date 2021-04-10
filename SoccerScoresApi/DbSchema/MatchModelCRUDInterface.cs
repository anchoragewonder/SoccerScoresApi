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
    public class MatchModelCRUDInterface
    {
        private const string Table = "soccer_table";

        public static async Task<bool> TryUpsertMatches(UpdateMatchesRequest request)
        {
            foreach (MatchModel s in request.Matches)
            {
                bool exists = await TryGetMatch(s);
                bool success = (!exists) ? await TryInsertMatch(s) : await TryUpdateMatch(s);
                if (!success) { return false; }
            }
            return true;
        }

        public static async Task<bool> TryInsertFixtures(UpdateMatchesRequest request)
        {
            foreach (MatchModel s in request.Matches)
            {
                bool exists = await TryGetMatch(s);
                if (!exists) { await TryInsertMatch(s); }
            }
            return true;
        }

        public static async Task<bool> TryInsertMatch(MatchModel request)
        {
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                string commandText = $@"INSERT INTO {Table} (id, date, home_team, away_team, home_score, away_score) VALUES(null, @date, @home_team, @away_team, @home_score, @away_score)";
                MySqlCommand cmd = MatchModelCommandBuilder(commandText, connection.Connection, request);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exeption caught: {ex.Message}");
                return false;
            }
            finally
            {
                await connection.Disconnect();
            }
        }

        public static async Task<bool> TryGetMatch(MatchModel request)
        {
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                string commandText = $@"SELECT * FROM {Table} WHERE date = @date AND home_team = @home_team AND away_team = @away_team";
                MySqlCommand cmd = MatchModelCommandBuilder(commandText, connection.Connection, request);
                var reader = await cmd.ExecuteReaderAsync();

                return reader.HasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exeption caught: {ex.Message}");
                return false;
            }
            finally
            {
                await connection.Disconnect();
            }
        }
        public static async Task<bool> TryUpdateMatch(MatchModel request)
        {
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                string commandText = $@"UPDATE {Table} SET home_score = @home_score, away_score = @away_score WHERE date = @date AND home_team = @home_team AND away_team = @away_team";
                MySqlCommand cmd = MatchModelCommandBuilder(commandText, connection.Connection, request);
                int rows = await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exeption caught: {ex.Message}");
                return false;
            }
            finally
            {
                await connection.Disconnect();
            }
        }
        public static async Task<bool> TryDeletetMatch(MatchModel request)
        {
            DbConnector connection = new DbConnector();
            if (!(await connection.IsConnected()))
            {
                throw new Exception();
            }
            try
            {
                string commandText = $@"DELETE * FROM {Table} WHERE date = @date AND home_team = @home_team AND away_team = @away_team";
                MySqlCommand cmd = MatchModelCommandBuilder(commandText, connection.Connection, request);
                var reader = await cmd.ExecuteReaderAsync();

                return reader.HasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exeption caught: {ex.Message}");
                return false;
            }
            finally
            {
                await connection.Disconnect();
            }
        }
        public static MySqlCommand MatchModelCommandBuilder(string text, MySqlConnection connection, MatchModel request)
        {
            MySqlCommand cmd = new MySqlCommand(text, connection);
            cmd.Parameters.AddWithValue("@date", request.date);
            cmd.Parameters.AddWithValue("@home_team", request.homeTeam);
            cmd.Parameters.AddWithValue("@away_team", request.awayTeam);
            cmd.Parameters.AddWithValue("@home_score", request.homeScore);
            cmd.Parameters.AddWithValue("@away_score", request.awayScore);
            return cmd;
        }
    }
}
