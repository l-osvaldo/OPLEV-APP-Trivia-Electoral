using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataBank;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;
    [SerializeField] private GameObject m_homeUI = null;
    [SerializeField] private GameObject m_questionsUI = null;
    [SerializeField] private GameObject m_respuestaUI = null;
    [SerializeField] private GameObject m_resultadosUI = null;

    [Header("Login")]
    [SerializeField] private Text m_infoLoginTxt = null;
    [SerializeField] private InputField m_emailLoginInput = null;
    [SerializeField] private InputField m_passwordLoginInput = null;
    private int IDUser = 0;


    [Header("Register")]
    [SerializeField] private InputField m_nameInput = null;
    [SerializeField] private InputField m_emailInput = null;
    [SerializeField] private InputField m_edadInput = null;
    [SerializeField] private Dropdown m_sexoInput = null;
    [SerializeField] private InputField m_filtroMunicipioInput = null;
    [SerializeField] private Dropdown m_municipioInput = null;
    [SerializeField] private InputField m_passwordInput = null;
    [SerializeField] private Toggle m_privacidadToggle = null;
    [SerializeField] private Text m_infoErrorTxt = null;

    [Header("Preguntas")]
    private List<Pregunta> preguntasRestantes = new List<Pregunta>();
    [SerializeField] private Text m_preguntaTxt = null;
    [SerializeField] private Button m_opcion1Btn = null;
    [SerializeField] private Button m_opcion2Btn = null;
    [SerializeField] private Button m_opcion3Btn = null;
    [SerializeField] private Button m_opcion4Btn = null;

    [Header("Respuesta")]
    [SerializeField] private Text m_respuestaTxt = null;
    [SerializeField] private Image m_respuestaImg = null;
    private string respuestaCorrecta = "";
    private int contadorAciertos = 0;
    private int contadorErrores = 0;
    private string bitacoraDeResultados = "";

    [Header("ResumenPartida")]
    [SerializeField] private Text m_aciertosTxt = null;
    [SerializeField] private Text m_erroresTxt = null;
    [SerializeField] private Text m_totalTxt = null;

    private NetworkManager m_networkManager = null;

    private void Awake()
    {
        m_networkManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {        
        ShowLogin();
    }

    // Inicio de vistas -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void ShowLogin()
    {
        registrarEnDB();
        m_emailLoginInput.text = "";
        m_passwordLoginInput.text = "";

        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);
        m_homeUI.SetActive(false);
        m_questionsUI.SetActive(false);
        m_respuestaUI.SetActive(false);
        m_resultadosUI.SetActive(false);
    }

    public void ShowRegister()
    {
        iniciarMunicipios();
        m_nameInput.text = "";
        m_emailInput.text = "";
        m_edadInput.text = "";
        m_filtroMunicipioInput.text = "";
        m_municipioInput.value = 0;
        m_passwordInput.text = "";
        m_infoErrorTxt.text = "";
        m_sexoInput.value = 0;
        m_privacidadToggle.isOn = false;
        m_infoLoginTxt.text = "";

        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
        m_homeUI.SetActive(false);
        m_questionsUI.SetActive(false);
        m_respuestaUI.SetActive(false);
        m_resultadosUI.SetActive(false);
    }

    public void ShowHome()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(false);
        m_homeUI.SetActive(true);
        m_questionsUI.SetActive(false);
        m_respuestaUI.SetActive(false);
        m_resultadosUI.SetActive(false);
        saveResultadosSQLite("INICIO");
    }

    public void InicioPartida()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(false);
        m_homeUI.SetActive(false);
        m_questionsUI.SetActive(true);
        m_respuestaUI.SetActive(false);
        m_resultadosUI.SetActive(false);
        contadorAciertos = 0;
        contadorErrores = 0;
        bitacoraDeResultados = "";
        Preguntas();
    }

    public void Resultados()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(false);
        m_homeUI.SetActive(false);
        m_questionsUI.SetActive(false);
        m_respuestaUI.SetActive(false);
        m_resultadosUI.SetActive(true);

        m_aciertosTxt.text = "" + contadorAciertos;
        m_erroresTxt.text = "" + contadorErrores;
        m_totalTxt.text = "" + (contadorAciertos + contadorErrores);

        //registrarEnDB();

        saveResultadosSQLite("FIN");
    }

    // submit -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void SubmitRegister()
    {

        string sexo = "";
        string municipio = "";

        char[] charsToTrim = { '*', ' ', '\'' };
        m_nameInput.text = m_nameInput.text.Trim(charsToTrim);
        m_emailInput.text = m_emailInput.text.Trim(charsToTrim);
        m_edadInput.text = m_edadInput.text.Trim(charsToTrim);
        m_passwordInput.text = m_passwordInput.text.Trim(charsToTrim);
        if (m_nameInput.text == "")
        {
            m_infoErrorTxt.text = "* Ingrese su nombre";
            return;
        }
        if (m_emailInput.text == "")
        {
            m_infoErrorTxt.text = "* Ingrese su correo electrónico";
            return;
        }
        if (m_edadInput.text == "")
        {
            m_infoErrorTxt.text = "* Ingrese su edad";
            return;
        }
        if (m_sexoInput.value == 0)
        {
            m_infoErrorTxt.text = "* Seleccione su sexo";
            return;
        }
        else
        {
            switch (m_sexoInput.value)
            {
                case 1:
                    sexo = "M";
                    break;
                case 2:
                    sexo = "F";
                    break;
            }
        }
        if (m_municipioInput.value == 0)
        {
            m_infoErrorTxt.text = "* Seleccione su municipio";
            return;
        }
        else
        {
            int m_DropdownValue = m_municipioInput.value;
            string textMunicipio = m_municipioInput.options[m_DropdownValue].text;
            string[] municipioArray = textMunicipio.Split('-');
            municipio = municipioArray[1].Trim(charsToTrim);
        }
        if (m_passwordInput.text == "")
        {
            m_infoErrorTxt.text = "* Ingrese su contraseña";
            return;
        }
        if (!m_privacidadToggle.isOn)
        {
            m_infoErrorTxt.text = "* Aceptar el aviso de privacidad";
            return;
        }

        m_infoErrorTxt.text = "Procesando...";

        AppUserDB mAppUserDB = new AppUserDB();

        mAppUserDB.addData(new AppUser("0", m_nameInput.text, m_emailInput.text, m_edadInput.text, sexo,
        municipio, m_passwordInput.text, "NO"));

        mAppUserDB.close();

        m_infoErrorTxt.text = "Usuario Registrado";
        m_infoLoginTxt.text = "Usuario Registrado , ya puedes iniciar sesión";
        ShowLogin();

    }

    public void SubmitLogin()
    {
        char[] charsToTrim = { '*', ' ', '\'' };
        m_emailLoginInput.text = m_emailLoginInput.text.Trim(charsToTrim);
        m_passwordLoginInput.text = m_passwordLoginInput.text.Trim(charsToTrim);

        if (m_emailLoginInput.text == "")
        {
            m_infoLoginTxt.text = "* Ingrese su correo electrónico";
            return;
        }
        if (m_passwordLoginInput.text == "")
        {
            m_infoLoginTxt.text = "* Ingrese su contraseña";
            return;
        }

        m_infoLoginTxt.text = "Procesando...";

        AppUserDB mAppUserDB = new AppUserDB();
        IDataReader reader = mAppUserDB.getDataByEmailAndPassword(m_emailLoginInput.text, m_passwordLoginInput.text);

        bool credenciales = false;
        while (reader.Read())
        {
            credenciales = true;
            IDUser = int.Parse(reader[0].ToString());
        }
        reader.Close();
        if (credenciales)
        {
            mAppUserDB.close();
            ShowHome();
        }
        else
        {
            if (m_networkManager.verifyInternetAccess())
            {
                m_networkManager.LoginUserApp(m_emailLoginInput.text, m_passwordLoginInput.text, delegate (Response response)
                {
                    m_infoLoginTxt.text = response.message;
                    if (response.message == "Logueado")
                    {
                        IDUser = response.id;
                        AppUser appUser = new AppUser(response.id.ToString(), "", m_emailLoginInput.text, "", "", "", m_passwordLoginInput.text, "SI");
                        mAppUserDB.addData(appUser);
                        mAppUserDB.close();
                        ShowHome();
                    }
                    else
                    {
                        m_infoLoginTxt.text = response.message;
                    }
                });
            }
            else
            {
                m_infoLoginTxt.text = "Credenciales Invalidas";
            }
        }
        
    }

    // funciones para el registro -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void filtroMunicipio()
    {
        string filtro = m_filtroMunicipioInput.text;

        MunicipioDB municipioDB = new MunicipioDB();

        m_municipioInput.ClearOptions();

        if (filtro.Length > 0)
        {
            IDataReader reader = municipioDB.filtroMunicipios(filtro);

            bool coincidencias = false;

            List<string> m_DropOptions = new List<string> { "Seleccione su municipio" };

            while (reader.Read())
            {
                coincidencias = true;
                m_DropOptions.Add(reader[0].ToString() + " - " + reader[3].ToString());
            }

            if (coincidencias)
            {
                m_municipioInput.AddOptions(m_DropOptions);
                m_municipioInput.interactable = true;
            }
            else
            {
                m_infoErrorTxt.text = "* No existe coincidencias ";
                m_municipioInput.ClearOptions();
                m_municipioInput.interactable = false;
            }

            reader.Close();

        }
        else
        {
            m_municipioInput.interactable = false;
        }

        municipioDB.close();
    }

    // registros en WS -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void registrarEnDB()
    {
        
        if (m_networkManager.verifyInternetAccess())
        {
            AppUserDB mAppUserDB = new AppUserDB();
            IDataReader reader = mAppUserDB.registrados("NO");

            while (reader.Read())
            {
                string nombre = reader[1].ToString();
                string email = reader[2].ToString();
                string pass = reader[6].ToString();
                m_networkManager.CreateUserApp(reader[1].ToString(), reader[2].ToString(), int.Parse(reader[3].ToString()),
                    reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), delegate (Response response)
                    {
                        if (response.message == "Usuario Registrado")
                        {
                            mAppUserDB.actualizarID(response.id.ToString(), nombre, email);
                            mAppUserDB.actualizarStatusRegistrado("SI", nombre, email);
                            IDUser = response.id;
                        }
                        else
                        {
                             m_networkManager.LoginUserApp(email, pass, delegate (Response response1)
                             {
                                 if (response.message == "Logueado")
                                 {
                                     mAppUserDB.actualizarID(response1.id.ToString(), nombre, email);
                                     mAppUserDB.actualizarStatusRegistrado("SI", nombre, email);
                                     IDUser = response.id;
                                     
                                 }
                             });
                        }
                    });
            }
            reader.Close();
        }
    }

    // funciones para el home -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void cargarPreguntas()
    {
        preguntasRestantes.Clear();

        Debug.Log("preguntasRestantes: cP: " + preguntasRestantes.Count);

        PreguntaDB mPreguntaDB = new PreguntaDB();

        IDataReader reader = mPreguntaDB.countPreguntas();

        // Debug.Log(reader[0].ToString());

        int registrosPreguntas = int.Parse(reader[0].ToString());

        if (registrosPreguntas == 0)
        {
            mPreguntaDB.deleteTable();
            Pregunta pregunta = new Pregunta("1", 
                "El sistema electoral mexicano a nivel federal lo integran", 
                "Respuesta incorrecta",
                "Respuesta correcta", 
                "Respuesta incorrecta", 
                "Respuesta incorrecta", 
                "b");
            mPreguntaDB.addData(pregunta);
            Pregunta pregunta02 = new Pregunta("2",
                "El Poder Ejecutivo a nivel Federal es ejercido por",
                "Respuesta incorrecta",
                "Respuesta incorrecta",
                "Respuesta correcta",
                "Respuesta incorrecta", 
                "c");
            mPreguntaDB.addData(pregunta02);
            Pregunta pregunta03 = new Pregunta("3",
                "El Poder Legislativo a nivel Federal es ejercido por",
                "Respuesta correcta",
                "Respuesta incorrecta",
                "Respuesta incorrecta",
                "Respuesta incorrecta",
                "a");
            mPreguntaDB.addData(pregunta03);
            Pregunta pregunta04 = new Pregunta("4",
                "Es un medio de impugnación en contra de actos de autoridades administrativas electorales, que podrá ser interpuesto por un partido político, coalición, candidatura independiente a través de sus representantes legítimos, o candidato independiente de manera individual",
                "Respuesta incorrecta",
                "Respuesta correcta",
                "Respuesta incorrecta",
                "Respuesta incorrecta",
                "b");
            mPreguntaDB.addData(pregunta04);
            Pregunta pregunta05 = new Pregunta("5",
                "Cuál de las siguientes opciones es un derecho de los partidos políticos",
                "Respuesta incorrecta",
                "Respuesta incorrecta",
                "Respuesta incorrecta",
                "Respuesta correcta",
                "d");
            mPreguntaDB.addData(pregunta05);

            reader = mPreguntaDB.getAllPreguntas();
            iniciarPreguntas(reader);

            mPreguntaDB.close();
            reader.Close();
        }
        else
        {
            if (m_networkManager.verifyInternetAccess())
            {
                m_networkManager.allPreguntas(delegate (Preguntas preguntas)
                {
                    // Debug.Log("WS preguntas: " + preguntas.pregunta.Count);
                    if (registrosPreguntas != preguntas.pregunta.Count)
                    {
                        mPreguntaDB.deleteTable();
                        for (int i = 0; i < preguntas.pregunta.Count; i++)
                        {
                            // Debug.Log(preguntas.pregunta[i].id);
                            Pregunta pre = new Pregunta(preguntas.pregunta[i].id, preguntas.pregunta[i].pregunta,
                                preguntas.pregunta[i].opcion_a, preguntas.pregunta[i].opcion_b, preguntas.pregunta[i].opcion_c,
                                preguntas.pregunta[i].opcion_d, preguntas.pregunta[i].respuesta);
                            mPreguntaDB.addData(pre);
                        }
                        reader = mPreguntaDB.getAllPreguntas();
                        iniciarPreguntas(reader);

                        mPreguntaDB.close();
                        reader.Close();
                    }
                    else
                    {
                        reader = mPreguntaDB.getAllPreguntas();
                        iniciarPreguntas(reader);

                        mPreguntaDB.close();
                        reader.Close();
                    }
                });
            }
            else
            {
                reader = mPreguntaDB.getAllPreguntas();
                iniciarPreguntas(reader);

                mPreguntaDB.close();
                reader.Close();
            }
        }
    }

    public void iniciarPreguntas(IDataReader reader)
    {
        Debug.Log("preguntasRestantes: iPi: " + preguntasRestantes.Count);

        while (reader.Read())
        {
            Pregunta preguntas = new Pregunta(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(),
                reader[4].ToString(), reader[5].ToString(), reader[6].ToString());
            preguntasRestantes.Add(preguntas);
        }
        Debug.Log("preguntasRestantes: iPf: " + preguntasRestantes.Count);
    }

    // funciones para las preguntas -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void Preguntas()
    {
        // Debug.Log("preguntasRestantes: Preguntas: " + preguntasRestantes.Count);
        
        int numeroPregunta = NumPreguntaAleatorio(preguntasRestantes.Count);

        Debug.Log("preguntasRestantes: id: " + preguntasRestantes[numeroPregunta].id);

        Debug.Log("preguntasRestantes: length: " + preguntasRestantes[numeroPregunta].pregunta.Length);

        m_preguntaTxt.text = "¿" + preguntasRestantes[numeroPregunta].pregunta + "?";

        if (preguntasRestantes[numeroPregunta].pregunta.Length >= 100 && preguntasRestantes[numeroPregunta].pregunta.Length <= 160)
        {
            m_preguntaTxt.fontSize = 14;
        }
        else
        {
            if (preguntasRestantes[numeroPregunta].pregunta.Length > 160)
            {
                m_preguntaTxt.fontSize = 12;
            }
            else
            {
                m_preguntaTxt.fontSize = 18;
            }
                
        }            

        bitacoraDeResultados += preguntasRestantes[numeroPregunta].id;

        respuestaCorrecta = preguntasRestantes[numeroPregunta].respuesta;

        if (respuestaCorrecta == "a")
        {
            respuestaCorrecta = preguntasRestantes[numeroPregunta].opcion_a;
        }
        if (respuestaCorrecta == "b")
        {
            respuestaCorrecta = preguntasRestantes[numeroPregunta].opcion_b;
        }
        if (respuestaCorrecta == "c")
        {
            respuestaCorrecta = preguntasRestantes[numeroPregunta].opcion_c;
        }
        if (respuestaCorrecta == "d")
        {
            respuestaCorrecta = preguntasRestantes[numeroPregunta].opcion_d;
        }

        string[] opc = { preguntasRestantes[numeroPregunta].opcion_a,
                            preguntasRestantes[numeroPregunta].opcion_b,
                            preguntasRestantes[numeroPregunta].opcion_c,
                            preguntasRestantes[numeroPregunta].opcion_d};

        OrdenarRespuestas(opc);

        m_opcion1Btn.GetComponentInChildren<Text>().text = opc[0];
        m_opcion2Btn.GetComponentInChildren<Text>().text = opc[1];
        m_opcion3Btn.GetComponentInChildren<Text>().text = opc[2];
        m_opcion4Btn.GetComponentInChildren<Text>().text = opc[3];

        preguntasRestantes.Remove(preguntasRestantes[numeroPregunta]);
    }

    public int NumPreguntaAleatorio(int rango)
    {
        //Debug.Log(rango);
        if (rango > 1)
        {
            var seed = Environment.TickCount;
            var random = new System.Random(seed);
            var value = 0;

            for (int i = 0; i <= 5; i++)
            {
                value = random.Next(0, rango);
                // Debug.Log($"Iteración {i} - semilla {seed} - valor {value}");
            }
            return value;
        }
        return 0;
    }

    public static void OrdenarRespuestas<T>(IList<T> values)
    {
        var n = values.Count;
        var rnd = new System.Random();
        for (int i = n - 1; i > 0; i--)
        {
            var j = rnd.Next(0, i);
            var temp = values[i];
            values[i] = values[j];
            values[j] = temp;
        }
    }

    public void Continuar()
    {
        m_respuestaUI.SetActive(false);
        if (preguntasRestantes.Count > 0)
        {
            Preguntas();
        }
        else
        {
            Resultados();
        }

    }

    // funciones para las respuestas -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void RespuestaPreguntaOpcA()
    {

        if (respuestaCorrecta == m_opcion1Btn.GetComponentInChildren<Text>().text)
        {
            m_respuestaTxt.text = "Respuesta Correcta";
            contadorAciertos++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
            bitacoraDeResultados += "C,";
        }
        else
        {
            m_respuestaTxt.text = "Respuesta Incorrecta";
            contadorErrores++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
            bitacoraDeResultados += "I,";
        }
        m_respuestaUI.SetActive(true);

    }

    public void RespuestaPreguntaOpcB()
    {

        if (respuestaCorrecta == m_opcion2Btn.GetComponentInChildren<Text>().text)
        {
            m_respuestaTxt.text = "Respuesta Correcta";
            contadorAciertos++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
            bitacoraDeResultados += "C,";
        }
        else
        {
            m_respuestaTxt.text = "Respuesta Incorrecta";
            contadorErrores++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
            bitacoraDeResultados += "I,";
        }

        m_respuestaUI.SetActive(true);

    }

    public void RespuestaPreguntaOpcC()
    {

        if (respuestaCorrecta == m_opcion3Btn.GetComponentInChildren<Text>().text)
        {
            m_respuestaTxt.text = "Respuesta Correcta";
            contadorAciertos++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
            bitacoraDeResultados += "C,";
        }
        else
        {
            m_respuestaTxt.text = "Respuesta Incorrecta";
            contadorErrores++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
            bitacoraDeResultados += "I,";
        }

        m_respuestaUI.SetActive(true);

    }

    public void RespuestaPreguntaOpcD()
    {

        if (respuestaCorrecta == m_opcion4Btn.GetComponentInChildren<Text>().text)
        {
            m_respuestaTxt.text = "Respuesta Correcta";
            contadorAciertos++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
            bitacoraDeResultados += "C,";
        }
        else
        {
            m_respuestaTxt.text = "Respuesta Incorrecta";
            contadorErrores++;
            m_respuestaImg.sprite = Resources.Load<Sprite>("<Sprites/incorrecto");
            bitacoraDeResultados += "I,";
        }

        m_respuestaUI.SetActive(true);

    }

    // funciones para los resultados -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public async void saveResultadosSQLite(string modo)
    {
        // Debug.Log(bitacoraDeResultados);
        // Debug.Log(IDUser);

        ResultadoDB resultadoDB = new ResultadoDB();

        if (IDUser != 0)
        { 

            if (modo == "INICIO")
            {
                IDataReader reader = resultadoDB.existeRegistroResultado(IDUser.ToString(), "NO");

                if (reader[0].ToString() == "0")
                {
                    reader = resultadoDB.existeRegistroResultado2("0");

                    if (reader[0].ToString() != "0")
                    {
                        resultadoDB.updateResultados2(IDUser.ToString());
                    }
                }
                reader.Close();
            }
            else
            {
                IDataReader reader = resultadoDB.existeRegistroResultado2(IDUser.ToString());

                if (reader[0].ToString() != "0")
                {
                    reader.Close();
                    resultadoDB.updateResultados(IDUser.ToString(), contadorAciertos.ToString(), contadorErrores.ToString(), bitacoraDeResultados);
                }
                else
                {
                    IDataReader data = resultadoDB.existeRegistroResultado2("0");

                    if (data[0].ToString() != "0")
                    {
                        data.Close();
                        resultadoDB.updateResultados2(IDUser.ToString());
                        resultadoDB.updateResultados(IDUser.ToString(), contadorAciertos.ToString(), contadorErrores.ToString(), bitacoraDeResultados);
                    }else
                    {
                        data.Close();
                        Resultado resultado = new Resultado("0", IDUser.ToString(), contadorAciertos.ToString(), contadorErrores.ToString(),
                                            bitacoraDeResultados, "NO");
                        resultadoDB.addData(resultado);
                    }
                }
            }

            IDataReader dataReader = resultadoDB.registradoResultado(IDUser.ToString(), "NO");

            while (dataReader.Read())
            {
                Resultado resultado = new Resultado(dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(),
                    dataReader[3].ToString(), dataReader[4].ToString(), dataReader[5].ToString());

                saveResultadosWS(resultado, IDUser.ToString());
            }
            dataReader.Close();
        }
        else
        {
            if (modo == "FIN")
            {
                registrarEnDB();

                await Task.Delay(3000);

                IDataReader data = resultadoDB.existeRegistroResultado2("0");
                if (data[0].ToString() != "0")
                {
                    data.Close();
                    resultadoDB.updateResultados(IDUser.ToString(), contadorAciertos.ToString(), contadorErrores.ToString(), bitacoraDeResultados);
                }
                else
                {
                    data.Close();
                    Resultado resultado = new Resultado("0", IDUser.ToString(), contadorAciertos.ToString(), contadorErrores.ToString(),
                                        bitacoraDeResultados, "NO");
                    resultadoDB.addData(resultado);
                }

            }
        }

        resultadoDB.close();
        cargarPreguntas();
    }

    public void saveResultadosWS(Resultado resultado, string id_user_app)
    {
        if (m_networkManager.verifyInternetAccess())
        {
            ResultadoDB resultadoDB = new ResultadoDB();

            m_networkManager.SaveResultados(resultado, delegate (Resultado resultadoWS)
            {
                resultadoDB.updateResultadoWS(id_user_app, resultadoWS.id.ToString(), "SI");
                resultadoDB.close();
            });
            

        }
    }

    // funciones generales -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void salirSesion()
    {
        m_infoLoginTxt.text = "";
        ShowLogin();
    }

    // funciones para el desarrollador -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    public void iniciarMunicipios()
    {

        Debug.Log("IniciarMunicipios");

        MunicipioDB municipioDB = new MunicipioDB();

        IDataReader reader = municipioDB.countMunicipios();


        int registrosMunicipios = int.Parse(reader[0].ToString());

        Debug.Log("Municipios: " + registrosMunicipios);

        if (registrosMunicipios == 0)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Json");
            path = Path.Combine(path, "Municipios.txt");

            Debug.Log(path);

            string json = "";

            if (path.Contains("://") || path.Contains(":///"))
            {
                Debug.Log("path: 1");
                UnityWebRequest file = UnityWebRequest.Get(path);
                Debug.Log("path: 1.1");
                file.SendWebRequest();
                while (!file.isDone) { }
                Debug.Log("path: 1.2");
                json = file.downloadHandler.text;
                Debug.Log("path: 1.3");
                Debug.Log("json: " + json);
            }
            else
            {
                Debug.Log("path: 2");
                json = File.ReadAllText(path);
            }

           
            Debug.Log("MunicipiosJson: " + json);

            json = json.Remove(0, 1);
            json = json.Remove(json.Length - 1, 1);

            string[] municipiosJSON = json.Split('{');

            for (int i = 0; i < municipiosJSON.Length; i++)
            {
                if (municipiosJSON[i].Length > 0)
                {
                    municipiosJSON[i] = "{" + municipiosJSON[i];

                    if (municipiosJSON[i].Substring(municipiosJSON[i].Length - 1, 1) == ",")
                    {
                        municipiosJSON[i] = municipiosJSON[i].Remove(municipiosJSON[i].Length - 1, 1);
                    }

                    Municipio m = JsonUtility.FromJson<Municipio>(municipiosJSON[i]);

                    municipioDB.addData(m);

                    //Debug.Log("numDto: " + m.numdto + ", nombreDto: " + m.nombredto + ", numMpio: " + m.nummpio + ", nombreMpio:" + m.nombrempio);

                }
            }
            municipioDB.close();
            //Debug.Log(json);
            
        }
    }

    public void BackHome()
    {
        ShowHome();
    }

    
}