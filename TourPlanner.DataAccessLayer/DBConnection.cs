using log4net;
using log4net.Config;
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

        private static readonly ILog _logger = LogManager.GetLogger(typeof(DBConnection));
        

        public DBConnection(Config config)
        {
            _connectionString = config.ConnectionString;
            _conn = new NpgsqlConnection(_connectionString);
        }

        public List<TourItem> GetTours()
        {
            List<TourItem> tours = new List<TourItem>();
            try
            {
                //XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config.xml"));

                _logger.Info("DB Get Tours");
                _logger.Error("DB Get Tours");

                _conn.Open();
                _sql = "select * from tour";
                _cmd = new NpgsqlCommand(_sql, _conn);
                var reader = _cmd.ExecuteReader();

                while (reader.Read())
                    tours.Add(new Models.TourItem { Name = reader.GetString(0), Description = reader.GetString(1), 
                        From = reader.GetString(2), To = reader.GetString(3), Distance = reader.GetFloat(4), TransportMode = reader.GetString(5),
                        Duration = reader.GetFloat(6), FuelUsed = reader.GetFloat(7)});

                _conn.Close();
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
            }

            return tours;

        }

        public bool AddTour(TourItem tour)
        {
            try
            {
                _conn.Open();
                _sql = "insert into tour values (@tourname, @description, @from, @to, @distance, @mode, @duration, @fuel)";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("tourname", tour.Name);
                _cmd.Parameters.AddWithValue("description", tour.Description);
                _cmd.Parameters.AddWithValue("from", tour.From);
                _cmd.Parameters.AddWithValue("to", tour.To);
                _cmd.Parameters.AddWithValue("distance", tour.Distance);
                _cmd.Parameters.AddWithValue("mode", tour.TransportMode);
                _cmd.Parameters.AddWithValue("duration", tour.Duration);
                _cmd.Parameters.AddWithValue("fuel", tour.FuelUsed);
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
                        TourName = reader.GetString(0),
                        Date = reader.GetDateTime(1),
                        Report = reader.GetString(2),
                        Rating = reader.GetInt32(3),
                        Weather = reader.GetString(4),
                        Effort = reader.GetInt32(5),
                        Duration = reader.GetFloat(6),
                        AverageSpeed = reader.GetFloat(7),
                        FuelUsed = reader.GetFloat(8),
                        Distance = reader.GetFloat(9)
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
        public bool AddTourLog(TourLogItem tourLog)
        {
            try
            {
                _conn.Open();
                _sql = "insert into tourlog values (@tourname, @date, @report, @rating, @weather, @effort, @duration, @speed, @fuel, @distance)";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("tourname", tourLog.TourName);
                _cmd.Parameters.AddWithValue("date", tourLog.Date);
                _cmd.Parameters.AddWithValue("duration", tourLog.Duration);
                _cmd.Parameters.AddWithValue("report", tourLog.Report);
                _cmd.Parameters.AddWithValue("rating", tourLog.Rating);
                _cmd.Parameters.AddWithValue("speed", tourLog.AverageSpeed);
                _cmd.Parameters.AddWithValue("fuel", tourLog.FuelUsed);
                _cmd.Parameters.AddWithValue("weather", tourLog.Weather);
                _cmd.Parameters.AddWithValue("effort", tourLog.Effort);
                _cmd.Parameters.AddWithValue("distance", tourLog.Distance);
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

                _cmd.Parameters.AddWithValue("name", tourLog.TourName);
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
