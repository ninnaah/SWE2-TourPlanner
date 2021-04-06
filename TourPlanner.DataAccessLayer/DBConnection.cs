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

        public DBConnection()
        {
            _connectionString = loadConfig("../../../../database.json");
            _conn = new NpgsqlConnection(_connectionString);
        }

        public string loadConfig(string fileName)
        {
            string connection = null;
            string jsonString = File.ReadAllText(fileName);
            DBConnObject conn = JsonConvert.DeserializeObject<DBConnObject>(jsonString);

            Debug.WriteLine($"Server={conn.server};Port={conn.port};User Id={conn.user};Password={conn.password};Database={conn.database};");

            connection = $"Server={conn.server};Port={conn.port};User Id={conn.user};Password={conn.password};Database={conn.database};";
            return connection;
        }

        public List<TourItem> GetTours()
        {
            List<TourItem> tours = new List<TourItem>();

            _conn.Open();
            _sql = "select * from tour";
            _cmd = new NpgsqlCommand(_sql, _conn);
            var reader = _cmd.ExecuteReader();

            while (reader.Read())
                tours.Add(new Models.TourItem { Name = reader.GetString(0), Description = reader.GetString(1), Distance = reader.GetInt32(2) });
            _conn.Close();

            foreach (TourItem item in tours)
                Debug.WriteLine(item.Name + item.Description + item.Distance);

            return tours;

        }
    }
}
