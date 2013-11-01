using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace Music_Player
{
    public sealed class DBManager
    {
        private static volatile DBManager instance;
        private static object monitor = new Object();
        private static string DBConnectionLink;
        private DBManager()
        {
            string dbfile = AppDomain.CurrentDomain.BaseDirectory +"\\library.sqlite";
            Console.WriteLine(dbfile);
            if(!File.Exists(dbfile))
            {
                Console.WriteLine("Database not present!");
                throw new Exception("Database file missing!");
            }
            else
            {
                DBConnectionLink = "Data Source=" + dbfile;
            }
        }
        public static DBManager Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(monitor)
                    {
                        if (instance == null)
                            instance = new DBManager();
                    }
                }
                return instance;
            }
        }
        public DataTable executeQuery(string SQL)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection DBConnection = new SQLiteConnection(DBConnectionLink))
                {
                    DBConnection.Open();
                    SQLiteCommand query = new SQLiteCommand(SQL, DBConnection);
                    SQLiteDataReader reader = query.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    DBConnection.Close();
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return dt;
        }
        public int executeNonQuery(string SQL)
        {
            int rowsAffected = -1;
            try
            {
                using (SQLiteConnection DBConnection = new SQLiteConnection(DBConnectionLink))
                {
                    DBConnection.Open();
                    SQLiteCommand query = new SQLiteCommand(SQL, DBConnection);
                    rowsAffected = query.ExecuteNonQuery();
                    DBConnection.Close();
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return rowsAffected;
        }
        public static void reset()
        {
            instance = null;
        } 
    }
}
