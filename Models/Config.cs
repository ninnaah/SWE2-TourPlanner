using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer
{
    public class Config
    {
        /*public string server;
        public int port;
        public string user;
        public string password;
        public string database;
        public string filePath;
        public string mapQuestKey;*/

        public string ConnectionString { get; set; }
        public string FilePath { get; set; }
        public string MapQuestKey { get; set; }

    }

}
