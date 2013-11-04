using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace Music_Player
{
    public sealed class DBManager
    {
        private static volatile DBManager instance;
        private static object Monitor = new Object();
        private static string DBConnectionLink;
        private DBManager()
        {
            string DBFile = AppDomain.CurrentDomain.BaseDirectory +"\\DB\\musiclibrary.sqlite";
            Console.WriteLine(DBFile);
            if(!File.Exists(DBFile))
            {
                Console.WriteLine("DB is nonexistent. Creating DB from sql!");
                SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "\\DB\\musiclibrary.sqlite");
                DBConnectionLink = "Data Source="+DBFile+";Version=3;";
                int ra = ExecuteScript(AppDomain.CurrentDomain.BaseDirectory+"\\DB\\musicplayer.sql");
                if (ra == -1)
                    throw new SQLiteException("Couldnt execute DDL script!");
            }
            DBConnectionLink = "Data Source=" + DBFile;
        }
        public static DBManager Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(Monitor)
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
                MessageBox.Show(e.Message+"\n"+e.StackTrace+"\n"+SQL);
            }
            return dt;
        }
        public int executeNonQuery(string SQL)
        {
            int RowsAffected = -1;
            try
            {
                using (SQLiteConnection DBConnection = new SQLiteConnection(DBConnectionLink))
                {
                    DBConnection.Open();
                    SQLiteCommand Query = new SQLiteCommand(SQL, DBConnection);
                    RowsAffected = Query.ExecuteNonQuery();
                    DBConnection.Close();
                }
            }
            catch (SQLiteException e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace + "\n Check console for SQL");
                Console.WriteLine(SQL);
            }
            return RowsAffected;
        }
        public int ExecuteScript(string path)
        {
            int RowsAffected = 0;

            try
            {
                string[] script = File.ReadAllText(path).Split(';');
                using (SQLiteConnection DBConnection = new SQLiteConnection(DBConnectionLink))
                {
                    DBConnection.Open();
                    foreach (string Line in script)
                    {
                        try
                        {
                            SQLiteCommand Query = new SQLiteCommand(Line, DBConnection);
                            RowsAffected += Query.ExecuteNonQuery();
                        }
                        catch (SQLiteException e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    DBConnection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace + "\n" + e.GetType());
            }
            return RowsAffected;
        }
        public static void reset()
        {
            instance = null;
        } 
    }
}
