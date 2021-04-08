using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class DBConnection : IDataAccess
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
                    tours.Add(new Models.TourItem { Name = reader.GetString(0), Description = reader.GetString(1), Distance = reader.GetInt32(2) });

                _conn.Close();
            }
            catch (Exception ex)
            {
                _conn.Close();
                Debug.WriteLine("Error: {0}", ex);
            }

            return tours;

        }

        public bool ImportTour(ref TourItem tour, string fileName)
        {
            try
            {
                _conn.Open();
                _sql = "insert into tour values (@tourname, @description, @distance, 'image')";
                _cmd = new NpgsqlCommand(_sql, _conn);

                _cmd.Parameters.AddWithValue("tourname", tour.Name);
                _cmd.Parameters.AddWithValue("description", tour.Description);
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


    }
}
