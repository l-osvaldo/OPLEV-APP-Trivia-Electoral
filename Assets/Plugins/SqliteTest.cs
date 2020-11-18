using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SqliteTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //string connection = "URI=file:" + Application.persistentDataPath + "/Database_Trivia";
        //IDbConnection dbcon = new SqliteConnection(connection);
        //dbcon.Open();

        //IDbCommand dbcmd;
        //IDataReader reader;

        //dbcmd = dbcon.CreateCommand();
        //string q_createTable =
        //  "CREATE TABLE IF NOT EXISTS my_table (id INTEGER PRIMARY KEY, val INTEGER )";

        //dbcmd.CommandText = q_createTable;
        //reader = dbcmd.ExecuteReader();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
