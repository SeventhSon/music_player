using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Music_Player.Model;
using System.Data;
using System.IO;

namespace MusicPlayerTests
{
    [TestClass]
    public class DBManagerTests
    {
        [TestMethod]
        public void Setup_DataBase_Connection()
        {
            DBManager dbm = DBManager.Instance;
        }
        [TestMethod]
        public void Execute_NonQuery()
        {
            DBManager dbm = DBManager.Instance;
            int result = dbm.executeNonQuery("DELETE FROM songs");
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void Execute_Query()
        {
            DBManager dbm = DBManager.Instance;
            DataTable dt = dbm.executeQuery("SELECT * FROM songs");
            Assert.AreEqual(dt.TableName,"songs");
            Assert.AreEqual(10, dt.Columns.Count);
        }
        [TestMethod]
        public void Insert_And_Retrieve_Resultset()
        {
            DBManager dbm = DBManager.Instance;
            int rowsAffected = dbm.executeNonQuery("Insert into songs (title, album, path,id_directory) values ('GhostWriter','RJD2','Path1',1),('GhostWriter','RJD2','Path2',1),('GhostWriter','RJD2','Path3',2),('GhostWriter','RJD2','Path4',3)");
            Assert.AreNotEqual(0, rowsAffected);
            DataTable dt = dbm.executeQuery("SELECT * FROM songs");
            Assert.AreEqual(4, dt.Rows.Count);
        }

        [ClassCleanup()]
        public static void CleanupAll()
        {
            File.Delete(AppDomain.CurrentDomain.BaseDirectory +"\\DB\\musiclibrary.sqlite");
            DBManager.reset();
        }
    }
}
