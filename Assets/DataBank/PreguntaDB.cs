using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class PreguntaDB : SqliteHelper
    {
        private const String CodistanTag = "Codistan: AppUserDB:\t";

        private const String TABLE_NAME = "preguntas";
        private const String KEY_ID = "id";
        private const String KEY_PREGUNTA = "pregunta";
        private const String KEY_OPCION_A = "opcion_a";
        private const String KEY_OPCION_B = "opcion_b";
        private const String KEY_OPCION_C = "opcion_c";
        private const String KEY_OPCION_D = "opcion_d";
        private const String KEY_RESPUESTA = "respuesta";
        private const String KEY_RUBRO = "rubro";
        private const String KEY_SUBRUBRO = "subrubro";
        private const String KEY_ETIQUETAS = "etiquetas";
        private const String KEY_VERSION     = "version";
        private const String KEY_NUMERO_RESPUESTAS = "numero_respuestas";
        // private String[] COLUMNS = new String[] {KEY_ID, KEY_PREGUNTA, KEY_OPCION_A, KEY_OPCION_B, KEY_OPCION_C, KEY_OPCION_D, KEY_RESPUESTA };

        public PreguntaDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_PREGUNTA + " TEXT, " +
                KEY_OPCION_A + " TEXT, " +
                KEY_OPCION_B + " TEXT, " +
                KEY_OPCION_C + " TEXT, " +
                KEY_OPCION_D + " TEXT, " +
                KEY_RESPUESTA + " TEXT, " +
                KEY_RUBRO + " TEXT, " +
                KEY_SUBRUBRO + " TEXT, " +
                KEY_ETIQUETAS + " TEXT, " +
                KEY_VERSION + " TEXT, " +
                KEY_NUMERO_RESPUESTAS + " TEXT )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(Pregunta pregunta)
        {
            IDbCommand dbcmd = getDbCommand();

            string query;
            query =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_ID + ", "
                + KEY_PREGUNTA + ", "
                + KEY_OPCION_A + ", "
                + KEY_OPCION_B + ", "
                + KEY_OPCION_C + ", "
                + KEY_OPCION_D + ", "
                + KEY_RESPUESTA + ", "
                + KEY_RUBRO + ", "
                + KEY_SUBRUBRO + ", "
                + KEY_ETIQUETAS + ", "
                + KEY_VERSION + ", "
                + KEY_NUMERO_RESPUESTAS + " ) "

                + "VALUES ( '"
                + pregunta.id + "', '"
                + pregunta.pregunta + "', '"
                + pregunta.opcion_a + "', '"
                + pregunta.opcion_b + "', '"
                + pregunta.opcion_c + "', '"
                + pregunta.opcion_d + "', '"
                + pregunta.respuesta + "', '"
                + pregunta.rubro + "', '"
                + pregunta.subrubro + "', '"
                + pregunta.etiquetas + "', '"
                + pregunta.version + "', '"
                + pregunta.numero_respuestas + "' )";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader getAllPreguntas()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader countPreguntas()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT COUNT(" + KEY_PREGUNTA + ") FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override void deleteTable()
        {
            IDbCommand dbCommand = getDbCommand();
            string query = "DELETE FROM " + TABLE_NAME;
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }

        public override IDataReader versionPreguntas()
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT DISTINCT " + KEY_VERSION + " FROM " + TABLE_NAME;

            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader filtroPorRubroAndSubrubroPreguntas(string rubro, string subrubro)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT " + KEY_ID + ", " + KEY_PREGUNTA + ", " + KEY_OPCION_A + ", " + KEY_OPCION_B + ", " 
                + KEY_OPCION_C + ", " + KEY_OPCION_D + ", " + KEY_RESPUESTA  + ", " + KEY_NUMERO_RESPUESTAS + " FROM " + TABLE_NAME + " WHERE " 
                + KEY_RUBRO + " = '" + rubro + "' AND " + KEY_SUBRUBRO + " = '" + subrubro + "'";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

        public override IDataReader filtroPorTemaPreguntas(string tema)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "SELECT " + KEY_ID + ", " + KEY_PREGUNTA + ", " + KEY_OPCION_A + ", " + KEY_OPCION_B + ", "
                + KEY_OPCION_C + ", " + KEY_OPCION_D + ", " + KEY_RESPUESTA + ", " + KEY_NUMERO_RESPUESTAS + " FROM " + TABLE_NAME + " WHERE "
                + KEY_ETIQUETAS + " LIKE '%" + tema + "%'";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            return dbcmd.ExecuteReader();
        }

    }
}
