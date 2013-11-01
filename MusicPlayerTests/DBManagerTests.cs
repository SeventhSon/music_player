using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Music_Player;
using System.Data;

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
            int result = dbm.executeNonQuery("DELETE FROM Songs");
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void Execute_Query()
        {
            DBManager dbm = DBManager.Instance;
            DataTable dt = dbm.executeQuery("SELECT * FROM Songs");
            Assert.AreEqual(dt.TableName,"Songs");
            Assert.AreEqual(9, dt.Columns.Count);
        }
        [TestMethod]
        public void Insert_And_Retrieve_Resultset()
        {
            DBManager dbm = DBManager.Instance;
            int rowsAffected = dbm.executeNonQuery("Insert into Songs (Title, Album, Path) values ('GhostWriter','RJD2','Path1'),('GhostWriter','RJD2','Path2'),('GhostWriter','RJD2','Path3'),('GhostWriter','RJD2','Path4')");
            Assert.AreNotEqual(0, rowsAffected);
            DataTable dt = dbm.executeQuery("SELECT * FROM Songs");
            Assert.AreEqual(4, dt.Rows.Count);
        }

        [ClassCleanup()]
        public static void CleanupAll()
        {
            DBManager dbm = DBManager.Instance;
            dbm.executeNonQuery("DELETE FROM Songs;");
            dbm.executeNonQuery("DELETE FROM PlaylistContent;");
            dbm.executeNonQuery("DELETE FROM Playlists;");
            DBManager.reset();
        }
    }
}
