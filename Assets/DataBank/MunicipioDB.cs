using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class MunicipioDB : SqliteHelper
    {
        private const String CodistanTag = "Codistan: MunicipioDB:\t";

        private const String TABLE_NAME = "municipios";
        private const String KEY_NUMDTO = "numdto";
        private const String KEY_NOMBREDTO = "nombredto";
        private const String KEY_NUMMPIO = "nummpio";
        private const String KEY_NOMBREMPIO = "nombrempio";
        private String[] COLUMNS = new String[] { KEY_NUMDTO, KEY_NOMBREDTO, KEY_NUMMPIO, KEY_NOMBREMPIO };

        public MunicipioDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_NUMDTO + " TEXT, " +
                KEY_NOMBREDTO + " TEXT, " +
                KEY_NUMMPIO + " TEXT, " +
                KEY_NOMBREMPIO + " TEXT )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(Municipio municipio)
        {
            IDbCommand dbcmd = getDbCommand();

            string query;
            query =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_NUMDTO + ", "
                + KEY_NOMBREDTO + ", "
                + KEY_NUMMPIO + ", "
                + KEY_NOMBREMPIO + " ) "

                + "VALUES ( '"
                + municipio.numdto + "', '"
                + municipio.nombredto + "', '"
                + municipio.nummpio + "', '"
                + municipio.nombrempio + "' )";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader countMunicipios()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT COUNT(" + KEY_NOMBREMPIO + ") FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader filtroMunicipios(string filtro)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_NOMBREMPIO + " LIKE '%" + filtro + "%'";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

    }
}
