using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class AppUserDB : SqliteHelper
    {
        private const String CodistanTag = "Codistan: AppUserDB:\t";

        private const String TABLE_NAME = "app_users";
        private const String KEY_ID = "id";
        private const String KEY_NOMBRE = "nombre";
        private const String KEY_EMAIL = "email";
        private const String KEY_EDAD = "edad";
        private const String KEY_SEXO = "sexo";
        private const String KEY_MUNICIPIO = "municipio";
        private const String KEY_PASSWORD = "password";
        private const String KEY_REGISTRADO = "registrado";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_NOMBRE, KEY_EMAIL, KEY_EDAD, KEY_SEXO, KEY_MUNICIPIO, KEY_PASSWORD, KEY_REGISTRADO };

        public AppUserDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_NOMBRE + " TEXT, " +
                KEY_EMAIL + " TEXT, " +
                KEY_EDAD + " TEXT, " +
                KEY_SEXO + " TEXT, " +
                KEY_MUNICIPIO + " TEXT, " +
                KEY_PASSWORD + " TEXT, " +
                KEY_REGISTRADO + " TEXT )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(AppUser appUser)
        {
            IDbCommand dbcmd = getDbCommand();

            string query;
            query =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_ID + ", "
                + KEY_NOMBRE + ", "
                + KEY_EMAIL + ", "
                + KEY_EDAD + ", "
                + KEY_SEXO + ", "
                + KEY_MUNICIPIO + ", "
                + KEY_PASSWORD + ", "
                + KEY_REGISTRADO + " ) "

                + "VALUES ( '"
                + appUser._id           + "', '"
                + appUser._nombre       + "', '"
                + appUser._email        + "', '"
                + appUser._edad         + "', '"
                + appUser._sexo         + "', '"
                + appUser._municipio    + "', '"
                + appUser._password     + "', '"
                + appUser._registrado   + "' )";
            // Debug.Log(query);
            dbcmd.CommandText = query;
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader getDataByEmailAndPassword(string email, string password)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_EMAIL + " = '" + email + "' AND " + KEY_PASSWORD + " = '" + password + "'";
            return dbcmd.ExecuteReader();
        }

        public override IDataReader registrados(string registro)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_REGISTRADO + " = '" + registro + "'" ;
            return dbcmd.ExecuteReader();
        }

        public override void actualizarStatusRegistrado(string registro, string nombre, string email)
        {
            IDbCommand dbcmd = getDbCommand();
            string query = "UPDATE " + TABLE_NAME + " SET " + KEY_REGISTRADO + " = '" + registro
                + "' WHERE " + KEY_NOMBRE + " = '" + nombre + "' AND " + KEY_EMAIL + " = '" + email + "'";

            // Debug.Log(query);

            dbcmd.CommandText = query;
                
            dbcmd.ExecuteNonQuery();
        }

        public override void actualizarID(string id, string nombre, string email)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "UPDATE " + TABLE_NAME + " SET " + KEY_ID + " = '" + id 
                + "' WHERE " + KEY_NOMBRE + " = '" + nombre + "' AND " + KEY_EMAIL + " = '" + email + "'";
           dbcmd.ExecuteNonQuery();
        }

        

    }
}
