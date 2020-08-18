using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class ResultadoDB : SqliteHelper
    {
        private const String CodistanTag = "Codistan: ResultadoDB:\t";

        private const String TABLE_NAME = "resultados";
        private const String KEY_ID = "id";
        private const String KEY_ID_USER_APP = "id_user_app";
        private const String KEY_ACIERTOS = "aciertos";
        private const String KEY_ERRORES = "errores";
        private const String KEY_DETALLE = "detalle";
        private const String KEY_REGISTRADO = "registrado";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_ID_USER_APP, KEY_ACIERTOS, KEY_ERRORES, KEY_DETALLE, KEY_REGISTRADO };

        public ResultadoDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_ID_USER_APP + " TEXT, " +
                KEY_ACIERTOS + " TEXT, " +
                KEY_ERRORES + " TEXT, " +
                KEY_DETALLE + " TEXT, " +
                KEY_REGISTRADO + " TEXT )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(Resultado resultado)
        {
            IDbCommand dbcmd = getDbCommand();

            string query;
            query =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_ID + ", "
                + KEY_ID_USER_APP + ", "
                + KEY_ACIERTOS + ", "
                + KEY_ERRORES + ", "
                + KEY_DETALLE + ", "
                + KEY_REGISTRADO + " ) "

                + "VALUES ( '"
                + resultado.id + "', '"
                + resultado.id_user_app + "', '"
                + resultado.aciertos + "', '"
                + resultado.errores + "', '"
                + resultado.detalle + "', '"
                + resultado.registrado + "' )";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader existeRegistroResultado(string id_user_app, string registrado)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT COUNT(" + KEY_ID_USER_APP + ") FROM " + TABLE_NAME 
                            + " WHERE " + KEY_ID_USER_APP + " = '" + id_user_app + "' AND " + KEY_REGISTRADO + " ='" + registrado + "'";

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader existeRegistroResultado2(string id_user_app)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT COUNT(" + KEY_ID_USER_APP + ") FROM " + TABLE_NAME
                            + " WHERE " + KEY_ID_USER_APP + " = '" + id_user_app + "'";

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override void updateResultados(string id_user_app, string aciertos, string errores, string detalle)
        {
            IDbCommand dbCommand = getDbCommand();
            string query = "UPDATE " + TABLE_NAME + " SET " + KEY_ACIERTOS + " ='" + aciertos + "', " 
                            + KEY_ERRORES + " ='" + errores + "', " + KEY_DETALLE + " ='" + detalle + "', " 
                            + KEY_REGISTRADO + " ='NO' WHERE " + KEY_ID_USER_APP + " = '" + id_user_app + "'";
            // Debug.Log(query);
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }

        public override void updateResultados2(string id_user_app)
        {
            IDbCommand dbCommand = getDbCommand();
            string query = "UPDATE " + TABLE_NAME + " SET " + KEY_ID_USER_APP + " ='" + id_user_app + "', "
                            + KEY_REGISTRADO + " ='NO' WHERE " + KEY_ID_USER_APP + " = '0'";
            // Debug.Log(query);
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }

        public override IDataReader registradoResultado(string id_user_app, string registro)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_REGISTRADO + " = '" 
                + registro + "' AND " + KEY_ID_USER_APP + " = '" + id_user_app + "'";
            // Debug.Log(query);
            dbcmd.CommandText = query;                
            return dbcmd.ExecuteReader();
        }

        public override void updateResultadoWS(string id_user_app, string id, string registrado)
        {
            IDbCommand dbCommand = getDbCommand();
            string query = "UPDATE " + TABLE_NAME + " SET " + KEY_ID + " ='" + id + "', "
                            + KEY_REGISTRADO + " ='" + registrado + "' WHERE " + KEY_ID_USER_APP + " = '" + id_user_app + "'";
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }



    }
}
