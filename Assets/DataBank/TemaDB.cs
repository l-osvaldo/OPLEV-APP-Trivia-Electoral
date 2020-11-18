using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class TemaDB : SqliteHelper
    {
        private const String CodistanTag = "Codistan: TemaDB:\t";

        private const String TABLE_NAME = "temas";
        private const String KEY_ID = "id";
        private const String KEY_TEMA = "tema";
        //private String[] COLUMNS = new String[] { KEY_ID, KEY_NOMBRE, KEY_EMAIL, KEY_EDAD, KEY_SEXO, KEY_MUNICIPIO, KEY_PASSWORD, KEY_SCORE, KEY_REGISTRADO };

        public TemaDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_TEMA + " TEXT )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(Tema tema)
        {
            IDbCommand dbcmd = getDbCommand();

            string query;
            query =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_ID + ", "
                + KEY_TEMA + " ) "

                + "VALUES ( '"
                + tema.id + "', '"
                + tema.tema + "' )";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader countTemas()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT COUNT(" + KEY_TEMA + ") FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader allTemas()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT " + KEY_TEMA + " FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader filtroTemas(string filtro)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT " + KEY_TEMA + " FROM " + TABLE_NAME + " WHERE " + KEY_TEMA + " LIKE '%" + filtro + "%'";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }
    }
}
