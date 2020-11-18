using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class EstadoDB : SqliteHelper
    {
        private const String CodistanTag = "Codistan: EstadoDB:\t";

        private const String TABLE_NAME = "estados";
        private const String KEY_CLAVE = "clave";
        private const String KEY_NOMBRE = "nombre";
        // private String[] COLUMNS = new String[] { KEY_NUMDTO, KEY_NOMBREDTO, KEY_NUMMPIO, KEY_NOMBREMPIO };

        public EstadoDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_CLAVE + " TEXT, " +
                KEY_NOMBRE + " TEXT )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(Estado estado)
        {
            IDbCommand dbcmd = getDbCommand();

            string query;
            query =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_CLAVE + ", "
                + KEY_NOMBRE + " ) "

                + "VALUES ( '"
                + estado.clave + "', '"
                + estado.nombre + "' )";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader countEstados()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT COUNT(" + KEY_NOMBRE + ") FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader allEstados()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT " + KEY_NOMBRE + " FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader filtroEstados(string filtro)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT DISTINCT " + KEY_NOMBRE + " FROM " + TABLE_NAME + " WHERE " + KEY_NOMBRE + " LIKE '%" + filtro + "%'";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

    }
}
