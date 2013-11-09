using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace Music_Player.Model
{
    public sealed class DBManager
    {
        private static volatile DBManager instance;
        private static object Monitor = new Object();
        private static string DBConnectionLink;
        /// <summary>
        /// Checks for presence of database file. If no DB file is found the 
        /// DB is recreated from DDL script and placed in the DB folder. 
        /// </summary>
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
        /// <summary>
        /// Holds the handle to singleton instance
        /// </summary>
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
        /// <summary>
        /// Executes user specified query on database
        /// </summary>
        /// <param name="SQL">Select query</param>
        /// <returns>DataTable with results of the query</returns>
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
                MessageBox.Show(e.Message + "\n" + e.StackTrace + "\n Check console for SQL Query");
                Console.WriteLine(SQL);
            }
            return dt;
        }
        /// <summary>
        /// Executes user specified nonQuery (such as UPDATE, INSERT, etc.) on DB
        /// </summary>
        /// <param name="SQL">UPDATE, INSERT etc. Query</param>
        /// <returns></returns>
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
                MessageBox.Show(e.Message + "\n" + e.StackTrace + "\n Check console for SQL Query");
                Console.WriteLine(SQL);
            }
            return RowsAffected;
        }
        /// <summary>
        /// Executes an SQL script from given path
        /// </summary>
        /// <param name="path">The location of the script file</param>
        /// <returns>Number of rows affected</returns>
        public int ExecuteScript(string path)
        {
            int RowsAffected = 0;

            try
            {
                //Split the file by delimiters
                string[] Script = File.ReadAllText(path).Split(';');
                using (SQLiteConnection DBConnection = new SQLiteConnection(DBConnectionLink))
                {
                    DBConnection.Open();
                    foreach (string Command in Script)
                    {
                        try
                        {
                            SQLiteCommand Query = new SQLiteCommand(Command, DBConnection);
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
        /// <summary>
        /// Allows for reseting the singleton and disposing of the previous instance
        /// </summary>
        public static void reset()
        {
            instance = null;
        } 
    }
}
