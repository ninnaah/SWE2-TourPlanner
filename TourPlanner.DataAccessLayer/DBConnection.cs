using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class DBConnection : IDataAccess
    {
        private string _connectionString;
        private NpgsqlConnection _conn;
        private string _sql;
        private NpgsqlCommand _cmd;

        public DBConnection(Config config)
        {
            _connectionString = $"Server={config.server};Port={config.port};User Id={config.user};Password={config.password};Database={config.database};";
            _conn = new NpgsqlConnection(_connectionString);
        }

        public List<TourItem> GetTours()
        {
            List<TourItem> tours = new List<TourItem>();
            try
            {
                _conn.Open();
                _sql = "select * from tour";
                _cmd = new NpgsqlCommand(_sql, _conn);
                var reader = _cmd.ExecuteReader();

                while (reader.Read())
                    tours.Add(new Models.TourItem { Name = reader.GetString(0), Description = reader.GetString(1), 
                        From = reader.GetString(2), To = reader.GetString(3), Distance = reader.GetFloat(4) });

                _conn.Close();
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
            }

            return tours;

        }

        public bool ImportTour(TourItem tour)
        {
            try
            {
                _conn.Open();
                _sql = "insert into tour values (@tourname, @description, @from, @to, @distance)";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("tourname", tour.Name);
                _cmd.Parameters.AddWithValue("description", tour.Description);
                _cmd.Parameters.AddWithValue("from", tour.From);
                _cmd.Parameters.AddWithValue("to", tour.To);
                _cmd.Parameters.AddWithValue("distance", tour.Distance);
                _cmd.ExecuteNonQuery();

                _conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
                return false;
            }

        }
        public bool DeleteTour(TourItem tour)
        {
            try
            {
                _conn.Open();
                _sql = "delete from tour where tourname=@name";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("name", tour.Name);
                _cmd.ExecuteNonQuery();

                _conn.Close();

                DeleteTourLog(tour.Name);

                return true;
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
                return false;
            }

        }




        public List<TourLogItem> GetTourLogs()
        {
            List<TourLogItem> tourLogs = new List<TourLogItem>();
            try
            {
                _conn.Open();
                _sql = "select * from tourlog";
                _cmd = new NpgsqlCommand(_sql, _conn);
                var reader = _cmd.ExecuteReader();

                while (reader.Read())
                    tourLogs.Add(new Models.TourLogItem
                    {
                        Name = reader.GetString(0),
                        Date = reader.GetDateTime(1),
                        Duration = reader.GetFloat(2),
                        Report = reader.GetString(3),
                        Rating = reader.GetInt32(4)
                    });

                _conn.Close();
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
            }

            return tourLogs;

        }
        public bool ImportTourLog(TourLogItem tourLog)
        {
            try
            {
                _conn.Open();
                _sql = "insert into tourlog values (@tourname, @date, @duration, @report, @rating)";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("tourname", tourLog.Name);
                _cmd.Parameters.AddWithValue("date", tourLog.Date);
                _cmd.Parameters.AddWithValue("duration", tourLog.Duration);
                _cmd.Parameters.AddWithValue("report", tourLog.Report);
                _cmd.Parameters.AddWithValue("rating", tourLog.Rating);
                _cmd.ExecuteNonQuery();

                _conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
                return false;
            }

        }
        public bool DeleteTourLogDate(TourLogItem tourLog)
        {
            try
            {
                _conn.Open();
                _sql = "delete from tourLog where tourname=@name and logdate=@date";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("name", tourLog.Name);
                _cmd.Parameters.AddWithValue("date", tourLog.Date);
                _cmd.ExecuteNonQuery();

                _conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
                return false;
            }

        }

        public bool DeleteTourLog(string tourName)
        {
            try
            {
                _conn.Open();
                _sql = "delete from tourLog where tourname=@name";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("name", tourName);
                _cmd.ExecuteNonQuery();

                _conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
                return false;
            }

        }




    }
}
