using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
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

                _cmd.Parameters.Add(new NpgsqlParameter("tourname", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("description", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("from", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("to", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("distance", NpgsqlDbType.Numeric));
                _cmd.Parameters.Add(new NpgsqlParameter("mode", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("duration", NpgsqlDbType.Numeric));
                _cmd.Parameters.Add(new NpgsqlParameter("fuel", NpgsqlDbType.Numeric));

                _cmd.Prepare();

                _cmd.Parameters[0].Value = tour.Name;
                _cmd.Parameters[1].Value = tour.Description;
                _cmd.Parameters[2].Value = tour.From;
                _cmd.Parameters[3].Value = tour.To;
                _cmd.Parameters[4].Value = tour.Distance;
                _cmd.Parameters[5].Value = tour.TransportMode;
                _cmd.Parameters[6].Value = tour.Duration;
                _cmd.Parameters[7].Value = tour.FuelUsed;

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
        public bool DeleteTour(string tourName)
        {
            try
            {
                _conn.Open();
                _sql = "delete from tour where tourname=@name";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));

                _cmd.Prepare();

                _cmd.Parameters[0].Value = tourName;

                _cmd.ExecuteNonQuery();

                _conn.Close();

                DeleteTourLog(tourName);

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
                        Distance = reader.GetFloat(9),
                        Temperature = reader.GetFloat(10)
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
                _sql = "insert into tourlog values (@tourname, @date, @report, @rating, @weather, @effort, @duration, @speed, @fuel, @distance, @temperature)";
                _cmd = new NpgsqlCommand(_sql, _conn);


                _cmd.Parameters.Add(new NpgsqlParameter("tourname", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("date", NpgsqlDbType.Timestamp));
                _cmd.Parameters.Add(new NpgsqlParameter("report", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("rating", NpgsqlDbType.Integer));
                _cmd.Parameters.Add(new NpgsqlParameter("weather", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("effort", NpgsqlDbType.Integer));
                _cmd.Parameters.Add(new NpgsqlParameter("duration", NpgsqlDbType.Numeric));
                _cmd.Parameters.Add(new NpgsqlParameter("speed", NpgsqlDbType.Numeric));
                _cmd.Parameters.Add(new NpgsqlParameter("fuel", NpgsqlDbType.Numeric));
                _cmd.Parameters.Add(new NpgsqlParameter("distance", NpgsqlDbType.Numeric));
                _cmd.Parameters.Add(new NpgsqlParameter("temperature", NpgsqlDbType.Numeric));

                _cmd.Prepare();

                _cmd.Parameters[0].Value = tourLog.TourName;
                _cmd.Parameters[1].Value = tourLog.Date;
                _cmd.Parameters[2].Value = tourLog.Report;
                _cmd.Parameters[3].Value = tourLog.Rating;
                _cmd.Parameters[4].Value = tourLog.Weather;
                _cmd.Parameters[5].Value = tourLog.Effort;
                _cmd.Parameters[6].Value = tourLog.Duration;
                _cmd.Parameters[7].Value = tourLog.AverageSpeed;
                _cmd.Parameters[8].Value = tourLog.FuelUsed;
                _cmd.Parameters[9].Value = tourLog.Distance;
                _cmd.Parameters[10].Value = tourLog.Temperature;

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

                _cmd.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));
                _cmd.Parameters.Add(new NpgsqlParameter("date", NpgsqlDbType.Timestamp));

                _cmd.Prepare();

                _cmd.Parameters[0].Value = tourLog.TourName;
                _cmd.Parameters[1].Value = tourLog.Date;

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

                _cmd.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));

                _cmd.Prepare();

                _cmd.Parameters[0].Value = tourName;

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
