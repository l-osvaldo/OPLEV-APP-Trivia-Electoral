using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using DataBank;
using System.IO;

public class NetworkManager : MonoBehaviour
{
    public bool verifyInternetAccess()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        else
        {
            return false;
        }        
    }

    public void CreateUserApp(string nombre, string email, int edad, string sexo, string municipio, string estado, string password, int score, Action<Response> response)
    {
        StartCoroutine(CO_CreateUser(nombre, email, edad, sexo, municipio, estado, password, score, response));
    }

    private IEnumerator CO_CreateUser(string nombre, string email, int edad, string sexo, string municipio, string estado,string password, int score, Action<Response> response)
    {
        WWWForm form = new WWWForm();

        form.AddField("nombre", nombre);
        form.AddField("email", email);
        form.AddField("edad", edad);
        form.AddField("sexo", sexo);
        form.AddField("municipio", municipio);
        form.AddField("estado", estado);
        form.AddField("password", password);
        form.AddField("score", score);

        UnityWebRequest ws = UnityWebRequest.Post("http://test.oplever.org.mx/triviasw/api/ws/registrarUsuarioApp",form);

        yield return ws.SendWebRequest();

        //Debug.Log(ws.downloadHandler.text);

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

        //Debug.Log(ws.downloadHandler.text);

        response(JsonUtility.FromJson<Response>(ws.downloadHandler.text));
    }

    public void allPreguntas(Action<Preguntas> preguntas)
    {
        StartCoroutine(CO_AllPreguntas(preguntas));
    }

    private IEnumerator CO_AllPreguntas(Action<Preguntas> preguntas)
    {
        UnityWebRequest ws = UnityWebRequest.Get("http://test.oplever.org.mx/triviasw/api/ws/allPreguntas");

        yield return ws.SendWebRequest();

        string resultJSON = ws.downloadHandler.text;

        resultJSON = resultJSON.Remove(0, 1);
        resultJSON = resultJSON.Remove(resultJSON.Length - 1, 1);

        List<Pregunta> tasks = new List<Pregunta>();

        string[] preguntasJSON = resultJSON.Split('{');
        for(int i = 0; i < preguntasJSON.Length; i++)
        {
            if (preguntasJSON[i].Length > 0)
            {
                preguntasJSON[i] = "{" + preguntasJSON[i];

                if (preguntasJSON[i].Substring(preguntasJSON[i].Length - 1 , 1) == ",")
                {
                    preguntasJSON[i] = preguntasJSON[i].Remove(preguntasJSON[i].Length - 1, 1);
                }



                Pregunta p = JsonUtility.FromJson<Pregunta>(preguntasJSON[i]);

                tasks.Add(p);

            }
            
        }

        Preguntas ps = new Preguntas();
        ps.pregunta = tasks;

        preguntas(ps);

    }

    public void allMunicipios()
    {
        StartCoroutine(CO_AllMunicipios());
    }

    private IEnumerator CO_AllMunicipios()
    {
        UnityWebRequest ws = UnityWebRequest.Get("http://test.oplever.org.mx/triviasw/api/ws/AllMunicipios");

        yield return ws.SendWebRequest();

        string resultJSON = ws.downloadHandler.text;

        File.WriteAllText("Assets/Json/Municipios.txt", resultJSON);
    }

    public void SaveResultados(Resultado resultado, Action<Resultado> responseResultado)
    {
        StartCoroutine(CO_SaveResultados(resultado, responseResultado));
    }

    private IEnumerator CO_SaveResultados(Resultado resultado, Action<Resultado> responseResultado)
    {
        // Debug.Log("WS save entre");
        WWWForm form = new WWWForm();

        form.AddField("id_user_app", resultado.id_user_app);
        form.AddField("aciertos", resultado.aciertos);
        form.AddField("errores", resultado.errores);
        form.AddField("detalle", resultado.detalle);

        UnityWebRequest ws = UnityWebRequest.Post("http://test.oplever.org.mx/triviasw/api/ws/saveResultados", form);

        yield return ws.SendWebRequest();

        // Debug.Log(ws.downloadHandler.text);

        string json = ws.downloadHandler.text;

        bool bandera = json.Contains("[");

        if (bandera)
        {
            json = json.Remove(0, 1);
            json = json.Remove(json.Length - 1, 1);
            
        }

        responseResultado(JsonUtility.FromJson<Resultado>(json));
    }

    public void UpdateScore(int id, int score, Action<AppUser> actualizado)
    {
        StartCoroutine(CO_UpdateScore(id ,score, actualizado));
    }

    private IEnumerator CO_UpdateScore(int id ,int score, Action<AppUser> actualizado)
    {
        // Debug.Log("WS save entre");
        WWWForm form = new WWWForm();

        form.AddField("id", id);
        form.AddField("score", score);

        UnityWebRequest ws = UnityWebRequest.Post("http://test.oplever.org.mx/triviasw/api/ws/UpdateScoreAppUser", form);

        yield return ws.SendWebRequest();

        // Debug.Log(ws.downloadHandler.text);

        string json = ws.downloadHandler.text;

        actualizado(JsonUtility.FromJson<AppUser>(json));
    }

}

[Serializable]
public class Response
{
    public bool done = false;
    public string message = "";
    public int id = 0;
    public string nombre = "";
    public string email = "";
    public int edad = 0;
    public string sexo = "";
    public string municipio = "";
    public string estado = "";
    public string password = "";
    public int score = 0;
    public int status = 0;
}


public class Preguntas
{
    public List<Pregunta> pregunta;
}
