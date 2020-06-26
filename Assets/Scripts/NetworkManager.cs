using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public void CreateUserApp(string nombre, string email, int edad, string sexo, string municipio, string password, Action<Response> response)
    {
        StartCoroutine(CO_CreateUser(nombre, email, edad, sexo, municipio, password, response));
    }

    private IEnumerator CO_CreateUser(string nombre, string email, int edad, string sexo, string municipio, string password, Action<Response> response)
    {
        WWWForm form = new WWWForm();

        form.AddField("nombre", nombre);
        form.AddField("email", email);
        form.AddField("edad", edad);
        form.AddField("sexo", sexo);
        form.AddField("municipio", municipio);
        form.AddField("password", password);

        UnityWebRequest ws = UnityWebRequest.Post("http://test.oplever.org.mx/triviasw/api/ws/registrarUsuarioApp",form);

        yield return ws.SendWebRequest();

        response(JsonUtility.FromJson<Response>(ws.downloadHandler.text));
    }

    public void LoginUserApp(string email, string password, Action<Response> response)
    {
        StartCoroutine(CO_LoginUser(email, password, response));
    }

    private IEnumerator CO_LoginUser(string email, string password, Action<Response> response)
    {
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest ws = UnityWebRequest.Post("http://test.oplever.org.mx/triviasw/api/ws/loginUsuarioApp", form);

        yield return ws.SendWebRequest();

        response(JsonUtility.FromJson<Response>(ws.downloadHandler.text));
    }
}

[Serializable]
public class Response
{
    public bool done = false;
    public string message = "";
    public int id = 0;
}


