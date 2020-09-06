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
    [SerializeField] private GameObject m_modalUI = null;
    [SerializeField] private GameObject m_resultadosUI = null;

    [SerializeField] private GameObject m_rubrosUI = null;
    [SerializeField] private GameObject m_subrubroCGUI = null;
    [SerializeField] private GameObject m_subrubroCPCCUI = null;
    [SerializeField] private GameObject m_aciertoUI = null;
    [SerializeField] private GameObject m_nivelUI = null;
    [SerializeField] private GameObject m_statusUI = null;
    [SerializeField] private GameObject m_buscarUI = null;

    [SerializeField] private GameObject m_opc3UI = null;
    [SerializeField] private GameObject m_opc4UI = null;

    [Header("Login")]
    [SerializeField] private Text m_infoLoginTxt = null;
    [SerializeField] private InputField m_emailLoginInput = null;
    [SerializeField] private InputField m_passwordLoginInput = null;
    private int IDUser = 0;
    private string nombreUsuario = "";
    private int score = 0;

    [Header("Home")]
    [SerializeField] private Text m_nombreUsuarioTxt = null;
    [SerializeField] private Text m_numeroCoincidenciasTxt = null;

    [Header("Buscar")]
    [SerializeField] private InputField m_buscarInput = null;

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
    [SerializeField] private Text m_preguntaTxt = null;
    [SerializeField] private Button m_opcion1Btn = null;
    [SerializeField] private Button m_opcion2Btn = null;
    [SerializeField] private Button m_opcion3Btn = null;
    [SerializeField] private Button m_opcion4Btn = null;
    [SerializeField] private Toggle m_opcionAToggle = null;
    [SerializeField] private Toggle m_opcionBToggle = null;
    [SerializeField] private Toggle m_opcionCToggle = null;
    [SerializeField] private Toggle m_opcionDToggle = null;
    [SerializeField] private GameObject m_barraProgreso = null;
    [SerializeField] private GameObject m_progreso = null;
    [SerializeField] private Text m_preguntasProgreso = null;
    [SerializeField] private Text m_nivelStatus = null;
    [SerializeField] private Text m_puntosStatus = null;
    private List<Pregunta> comboPreguntas = new List<Pregunta>();
    private int preguntasTotal = 0;
    private int[] niveles = { 10, 20, 30, 40, 50 };

    [Header("Respuesta")]
    [SerializeField] private Text m_respuestaTxt = null;
    [SerializeField] private Image m_respuestaImg = null;
    [SerializeField] private GameObject m_barraProgresoR = null;
    [SerializeField] private GameObject m_progresoR = null;
    [SerializeField] private Text m_preguntasProgresoR = null;
    private string respuestaCorrecta = "";
    private int contadorAciertos = 0;
    private int contadorErrores = 0;
    private string bitacoraDeResultados = "";

    [Header("Nivel")]
    [SerializeField] private Text m_nivelTxt = null;

    [Header("ResumenPartida")]
    [SerializeField] private Text m_aciertosTxt = null;
    [SerializeField] private Text m_erroresTxt = null;
    [SerializeField] private Text m_totalTxt = null;

    [Header("Perfil")]
    [SerializeField] private GameObject m_perfilUI = null;


    private NetworkManager m_networkManager = null;

    private void Awake()
    {
        m_networkManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {        
        ShowLogin();

        m_perfilUI.SetActive(false);
        LeanTween.scaleX(m_progreso, 0, 0);
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
        m_modalUI.SetActive(false);
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
        m_modalUI.SetActive(false);
        m_resultadosUI.SetActive(false);
    }

    public void ShowHome()
    {
        actualizarPreguntas();

        m_nombreUsuarioTxt.text = "Hola, " +  nombreUsuario;

        m_registerUI.SetActive(false);
        m_loginUI.SetActive(false);
        m_homeUI.SetActive(true);
        m_questionsUI.SetActive(false);
        m_modalUI.SetActive(false);
        m_resultadosUI.SetActive(false);

        m_rubrosUI.SetActive(true);
        m_subrubroCGUI.SetActive(false);
        m_subrubroCPCCUI.SetActive(false);
        m_questionsUI.SetActive(false);
        m_aciertoUI.SetActive(false);
        m_nivelUI.SetActive(false);
        m_statusUI.SetActive(false);
        m_buscarUI.SetActive(false);

        contadorAciertos = 0;
        contadorErrores = 0;
        bitacoraDeResultados = "";

        m_buscarInput.text = "";

        //saveResultadosSQLite("INICIO");
    }

    public void ShowSubrubroCG()
    {
        m_rubrosUI.SetActive(false);
        m_subrubroCGUI.SetActive(true);
        m_subrubroCPCCUI.SetActive(false);
        m_questionsUI.SetActive(false);
        m_aciertoUI.SetActive(false);
        m_resultadosUI.SetActive(false);
        m_nivelUI.SetActive(false);
        m_buscarUI.SetActive(false);
    }
    public void ShowSubrubroCPCC()
    {
        m_rubrosUI.SetActive(false);
        m_subrubroCGUI.SetActive(false);
        m_subrubroCPCCUI.SetActive(true);
        m_questionsUI.SetActive(false);
        m_aciertoUI.SetActive(false);
        m_resultadosUI.SetActive(false);
        m_nivelUI.SetActive(false);
        m_buscarUI.SetActive(false);
    }

    public void ShowPreguntas()
    {
        m_rubrosUI.SetActive(false);
        m_subrubroCGUI.SetActive(false);
        m_subrubroCPCCUI.SetActive(false);
        m_questionsUI.SetActive(true);
        m_aciertoUI.SetActive(false);
        m_resultadosUI.SetActive(false);
        m_nivelUI.SetActive(false);
        m_statusUI.SetActive(true);
        m_buscarUI.SetActive(false);

        asignarPregunta();
    }

    public void Resultados()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(false);
        m_questionsUI.SetActive(false);
        m_modalUI.SetActive(false);
        m_resultadosUI.SetActive(true);

        m_rubrosUI.SetActive(false);
        m_subrubroCGUI.SetActive(false);
        m_subrubroCPCCUI.SetActive(false);
        m_aciertoUI.SetActive(false);
        m_buscarUI.SetActive(false);

        m_aciertosTxt.text = "" + contadorAciertos;
        m_erroresTxt.text = "" + contadorErrores;
        m_totalTxt.text = "" + (contadorAciertos + contadorErrores);

        //registrarEnDB();

        //saveResultadosSQLite("FIN");
    }

    public void ShowProfile()
    {
        m_perfilUI.SetActive(true);
    }

    public void HideProfile()
    {
        m_perfilUI.SetActive(false);
    }

    public void HideModal()
    {
        m_modalUI.SetActive(false);
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
            municipio = textMunicipio;
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
        municipio, m_passwordInput.text,"0", "NO","1"));

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
            nombreUsuario = reader[1].ToString();
            score = int.Parse(reader[7].ToString());
            
        }
        reader.Close();
        if (credenciales)
        {
            nivelInicio();
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
                        nombreUsuario = response.nombre.ToString();
                        score = 0;
                        AppUser appUser = new AppUser(response.id.ToString(), response.nombre.ToString(), m_emailLoginInput.text, response.edad.ToString(),
                            response.sexo, response.municipio, m_passwordLoginInput.text, response.score.ToString() ,"SI", response.status.ToString());
                        mAppUserDB.addData(appUser);
                        mAppUserDB.close();
                        nivelInicio();
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
                m_DropOptions.Add(reader[0].ToString());
            }

            if (coincidencias)
            {
                m_municipioInput.AddOptions(m_DropOptions);
                m_municipioInput.interactable = true;
            }
            else
            {
                m_infoErrorTxt.text = "* No existen coincidencias";
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
                int score = int.Parse(reader[7].ToString());
                m_networkManager.CreateUserApp(reader[1].ToString(), reader[2].ToString(), int.Parse(reader[3].ToString()),
                    reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), int.Parse(reader[7].ToString()), delegate (Response response)
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

    public void actualizarPreguntas()
    {
        PreguntaDB mPreguntaDB = new PreguntaDB();

        IDataReader reader = mPreguntaDB.countPreguntas();

        int countPreguntas = int.Parse(reader[0].ToString());

        if (m_networkManager.verifyInternetAccess())
        {
            m_networkManager.allPreguntas(delegate (Preguntas preguntas)
            {
                if (countPreguntas == 0)
                {
                    for (int i = 0; i < preguntas.pregunta.Count; i++)
                    {
                        Pregunta pre = new Pregunta(preguntas.pregunta[i].id, preguntas.pregunta[i].pregunta,
                            preguntas.pregunta[i].opcion_a, preguntas.pregunta[i].opcion_b, preguntas.pregunta[i].opcion_c,
                            preguntas.pregunta[i].opcion_d, preguntas.pregunta[i].respuesta, preguntas.pregunta[i].rubro,
                            preguntas.pregunta[i].subrubro, preguntas.pregunta[i].etiquetas, preguntas.pregunta[i].version, 
                            preguntas.pregunta[i].numero_respuestas);
                        mPreguntaDB.addData(pre);
                    }
                    mPreguntaDB.close();
                }
                else
                {
                    reader = mPreguntaDB.versionPreguntas();

                    int versionApp = int.Parse(reader[0].ToString());
                    int versionDB = int.Parse(preguntas.pregunta[0].version);
                    reader.Close();

                    if (versionApp < versionDB)
                    {
                        InformeActualizacionPreguntas();
                        mPreguntaDB.deleteTable();
                        for (int i = 0; i < preguntas.pregunta.Count; i++)
                        {
                            Pregunta pre = new Pregunta(preguntas.pregunta[i].id, preguntas.pregunta[i].pregunta,
                                preguntas.pregunta[i].opcion_a, preguntas.pregunta[i].opcion_b, preguntas.pregunta[i].opcion_c,
                                preguntas.pregunta[i].opcion_d, preguntas.pregunta[i].respuesta, preguntas.pregunta[i].rubro,
                                preguntas.pregunta[i].subrubro, preguntas.pregunta[i].etiquetas, preguntas.pregunta[i].version,
                                preguntas.pregunta[i].numero_respuestas);
                            mPreguntaDB.addData(pre);
                        }
                    }
                    mPreguntaDB.close();
                }
            });
        }
        else
        {
            if (countPreguntas == 0)
            {
                string path = Path.Combine(Application.streamingAssetsPath, "Json");
                path = Path.Combine(path, "preguntasPredeterminadas.txt");

                //Debug.Log(path);

                string json = "";

                if (path.Contains("://") || path.Contains(":///"))
                {
                    UnityWebRequest file = UnityWebRequest.Get(path);
                    file.SendWebRequest();
                    while (!file.isDone) { }
                    json = file.downloadHandler.text;
                }
                else
                {
                    json = File.ReadAllText(path);
                }

                json = json.Remove(0, 1);
                json = json.Remove(json.Length - 1, 1);

                string[] preguntasPredeterminadasJSON = json.Split('{');

                for (int i = 0; i < preguntasPredeterminadasJSON.Length; i++)
                {
                    if (preguntasPredeterminadasJSON[i].Length > 0)
                    {
                        preguntasPredeterminadasJSON[i] = "{" + preguntasPredeterminadasJSON[i];

                        if (preguntasPredeterminadasJSON[i].Substring(preguntasPredeterminadasJSON[i].Length - 1, 1) == ",")
                        {
                            preguntasPredeterminadasJSON[i] = preguntasPredeterminadasJSON[i].Remove(preguntasPredeterminadasJSON[i].Length - 1, 1);
                        }

                        Pregunta pregunta = JsonUtility.FromJson<Pregunta>(preguntasPredeterminadasJSON[i]);

                        mPreguntaDB.addData(pregunta);

                    }
                }
            }
            mPreguntaDB.close();
        }
    }

    public void InformeActualizacionPreguntas()
    {
        m_modalUI.SetActive(true);
    }

    public void rubroUnoSubrubroUno()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos generales", "Constitución (CPEUM)");
    }

    public void rubroUnoSubrubroDos()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos generales", "LGIPE");
    }

    public void rubroUnoSubrubroTres()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos generales", "LGPP");
    }

    public void rubroUnoSubrubroCuatro()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos generales", "Constitución de Veracruz");
    }

    public void rubroUnoSubrubroCinco()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos generales", "Código Electoral del Estado de Veracruz");
    }

    public void rubroDosSubrubroUno()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos por cargos de consejos", "Presidenta(e) del Consejo Distrital");
    }

    public void rubroDosSubrubroDos()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos por cargos de consejos", "Consejera(o) del Consejo Distrital");
    }

    public void rubroDosSubrubroTres()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos por cargos de consejos", "Secretaria(o) del Consejo Distrital");
    }

    public void rubroDosSubrubroCuatro()
    {
        filtroPorRubroAndSubrubroPreguntas("Conocimientos por cargos de consejos", "Vocales");
    }

    public void filtroPorRubroAndSubrubroPreguntas(string rubro, string subrubro)
    {
        comboPreguntas.Clear();

        PreguntaDB preguntaDB = new PreguntaDB();

        IDataReader dataReader = preguntaDB.filtroPorRubroAndSubrubroPreguntas(rubro, subrubro);

        while (dataReader.Read())
        {
            Pregunta pregunta = new Pregunta(dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString(),
                dataReader[4].ToString(), dataReader[5].ToString(), dataReader[6].ToString(),"","","","", dataReader[7].ToString());
            comboPreguntas.Add(pregunta);
        }
        preguntasTotal = comboPreguntas.Count;

        ShowPreguntas();
    }

    public void asignarPregunta()
    {
        int numeroPregunta = NumPreguntaAleatorio(comboPreguntas.Count);

        m_preguntaTxt.text = "¿" + comboPreguntas[numeroPregunta].pregunta + "?";

        if (comboPreguntas[numeroPregunta].pregunta.Length >= 100)
        {
            m_preguntaTxt.resizeTextForBestFit = true;
        }
        else
        {
            m_preguntaTxt.resizeTextForBestFit = false;
            m_preguntaTxt.fontSize = 18;
        }

        string numeroRespuestas = comboPreguntas[numeroPregunta].numero_respuestas;



        bitacoraDeResultados += comboPreguntas[numeroPregunta].id;

        respuestaCorrecta = comboPreguntas[numeroPregunta].respuesta;

        if (respuestaCorrecta == "a")
        {
            respuestaCorrecta = comboPreguntas[numeroPregunta].opcion_a;
        }
        if (respuestaCorrecta == "b")
        {
            respuestaCorrecta = comboPreguntas[numeroPregunta].opcion_b;
        }

        if (numeroRespuestas == "3")
        {
            if (respuestaCorrecta == "c")
            {
                respuestaCorrecta = comboPreguntas[numeroPregunta].opcion_c;
            }
        }
        
        if (numeroRespuestas == "4")
        {
            if (respuestaCorrecta == "c")
            {
                respuestaCorrecta = comboPreguntas[numeroPregunta].opcion_c;
            }

            if (respuestaCorrecta == "d")
            {
                respuestaCorrecta = comboPreguntas[numeroPregunta].opcion_d;
            }
        }
        switch (numeroRespuestas)
        {
            case "2":
                string[] opc = { comboPreguntas[numeroPregunta].opcion_a,
                                comboPreguntas[numeroPregunta].opcion_b};

                OrdenarRespuestas(opc);

                m_opcionAToggle.GetComponentInChildren<Text>().text = opc[0];
                m_opcionBToggle.GetComponentInChildren<Text>().text = opc[1];

                m_opc3UI.SetActive(false);
                m_opc4UI.SetActive(false);

                break;
            case "3":
                string[] opc2 = { comboPreguntas[numeroPregunta].opcion_a,
                                comboPreguntas[numeroPregunta].opcion_b,
                                comboPreguntas[numeroPregunta].opcion_c};

                OrdenarRespuestas(opc2);

                m_opcionAToggle.GetComponentInChildren<Text>().text = opc2[0];
                m_opcionBToggle.GetComponentInChildren<Text>().text = opc2[1];
                m_opcionCToggle.GetComponentInChildren<Text>().text = opc2[2];

                m_opc3UI.SetActive(true);
                m_opc4UI.SetActive(false);

                break;
            case "4":
                string[] opc3 = { comboPreguntas[numeroPregunta].opcion_a,
                                comboPreguntas[numeroPregunta].opcion_b,
                                comboPreguntas[numeroPregunta].opcion_c,
                                comboPreguntas[numeroPregunta].opcion_d};

                OrdenarRespuestas(opc3);

                m_opcionAToggle.GetComponentInChildren<Text>().text = opc3[0];
                m_opcionBToggle.GetComponentInChildren<Text>().text = opc3[1];
                m_opcionCToggle.GetComponentInChildren<Text>().text = opc3[2];
                m_opcionDToggle.GetComponentInChildren<Text>().text = opc3[3];

                m_opc3UI.SetActive(true);
                m_opc4UI.SetActive(true);

                break;
        }

        comboPreguntas.Remove(comboPreguntas[numeroPregunta]);

        //SceneEventManager.Instance?.EnableButtons();

        m_preguntasProgreso.text = (preguntasTotal - comboPreguntas.Count) + "/" + preguntasTotal;

        float progressScale = 1 - ((float)comboPreguntas.Count / preguntasTotal);
        LeanTween.scaleX(m_progreso, progressScale, 0.25f).setEaseInOutBack();
        
    }

    public int NumPreguntaAleatorio(int rango)
    {
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

    public void enviarRespuesta()
    {
        m_preguntasProgresoR.text = (preguntasTotal - comboPreguntas.Count) + "/" + preguntasTotal;
        float progressScale = 1 - ((float)comboPreguntas.Count / preguntasTotal);
        LeanTween.scaleX(m_progresoR, progressScale, 0.25f).setEaseInOutBack();
        
        if (m_opcionAToggle.isOn)
        {
                      
            if (respuestaCorrecta == m_opcionAToggle.GetComponentInChildren<Text>().text)
            {
                m_respuestaTxt.text = "Respuesta Correcta";
                contadorAciertos++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
                bitacoraDeResultados += "C,";
                score += 2;
            }
            else
            {
                m_respuestaTxt.text = "Respuesta Incorrecta";
                contadorErrores++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
                bitacoraDeResultados += "I,";
            }
            m_questionsUI.SetActive(false);
            if (nivel())
            {
                m_nivelUI.SetActive(true);
            }
            else
            {
                m_nivelUI.SetActive(false);
                m_aciertoUI.SetActive(true);
            }
        }
        if (m_opcionBToggle.isOn)
        {
            
            if (respuestaCorrecta == m_opcionBToggle.GetComponentInChildren<Text>().text)
            {
                m_respuestaTxt.text = "Respuesta Correcta";
                contadorAciertos++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
                bitacoraDeResultados += "C,";
                score += 2;
            }
            else
            {
                m_respuestaTxt.text = "Respuesta Incorrecta";
                contadorErrores++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
                bitacoraDeResultados += "I,";
            }
            m_questionsUI.SetActive(false);
            if (nivel())
            {
                m_nivelUI.SetActive(true);
            }
            else
            {
                m_aciertoUI.SetActive(true);
            }
        }
        if (m_opcionCToggle.isOn)
        {
            
            if (respuestaCorrecta == m_opcionCToggle.GetComponentInChildren<Text>().text)
            {
                m_respuestaTxt.text = "Respuesta Correcta";
                contadorAciertos++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
                bitacoraDeResultados += "C,";
                score += 2;
            }
            else
            {
                m_respuestaTxt.text = "Respuesta Incorrecta";
                contadorErrores++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
                bitacoraDeResultados += "I,";
            }
            m_questionsUI.SetActive(false);
            if (nivel())
            {
                m_nivelUI.SetActive(true);
            }
            else
            {
                m_aciertoUI.SetActive(true);
            }
        }
        if (m_opcionDToggle.isOn)
        {
            
            if (respuestaCorrecta == m_opcionDToggle.GetComponentInChildren<Text>().text)
            {
                m_respuestaTxt.text = "Respuesta Correcta";
                contadorAciertos++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/correcto");
                bitacoraDeResultados += "C,";
                score += 2;
            }
            else
            {
                m_respuestaTxt.text = "Respuesta Incorrecta";
                contadorErrores++;
                m_respuestaImg.sprite = Resources.Load<Sprite>("Sprites/incorrecto");
                bitacoraDeResultados += "I,";
            }
            m_questionsUI.SetActive(false);
            if (nivel())
            {
                m_nivelUI.SetActive(true);
            }
            else
            {
                m_aciertoUI.SetActive(true);
            }
        }
        m_puntosStatus.text = score.ToString();
    }

    public void Siguiente()
    {
        m_opcionAToggle.isOn = false;
        m_opcionBToggle.isOn = false;
        m_opcionCToggle.isOn = false;
        m_opcionDToggle.isOn = false;

        m_aciertoUI.SetActive(false);
        //SceneEventManager.Instance?.DisableButtons();
        if (comboPreguntas.Count > 0)
        {
            //Preguntas();
            Invoke("ShowPreguntas", 1);
        }
        else
        {
            AppUserDB appUserDB = new AppUserDB();
            appUserDB.actualizarScore(""+score, ""+IDUser);
            //Resultados();
            Invoke("Resultados", 2);
            LeanTween.scaleX(m_progreso, 0, 2).setEaseInBack().setDelay(2);
            m_barraProgreso.GetComponent<SlideProgressBarBehaviour>().DisableProgressBar();
        }

    }

    public bool nivel()
    {
        if (score == niveles[0])
        {
            m_nivelStatus.text = "2";
            m_nivelTxt.text = "10";
            return true;
        }
        if (score == niveles[1])
        {
            m_nivelStatus.text = "3";
            m_nivelTxt.text = "20";
            return true;
        }
        if (score >= niveles[2])
        {
            m_nivelStatus.text = "4";
            m_nivelTxt.text = "30";
            return true;
        }
        if (score >= niveles[3])
        {
            m_nivelStatus.text = "5";
            m_nivelTxt.text = "40";
            return true;
        }
        return false;
    }

    public void nivelInicio()
    {
        m_puntosStatus.text = score.ToString();
        if (score >= niveles[0] && score < niveles[1])
        {
            m_nivelStatus.text = "2";
        }
        if (score >= niveles[1] && score < niveles[2])
        {
            m_nivelStatus.text = "3";
        }
        if (score >= niveles[2] && score < niveles[3])
        {
            m_nivelStatus.text = "4";
        }
        if (score >= niveles[3] && score < niveles[4])
        {
            m_nivelStatus.text = "5";
        }
    }

    public void buscar()
    {
        string filtro = m_buscarInput.text;
            
        if (filtro != "" && filtro != null)
        {
            m_rubrosUI.SetActive(false);
            m_subrubroCGUI.SetActive(false);
            m_subrubroCPCCUI.SetActive(false);
            m_questionsUI.SetActive(false);
            m_aciertoUI.SetActive(false);
            m_resultadosUI.SetActive(false);
            m_nivelUI.SetActive(false);
            m_buscarUI.SetActive(true);
            m_statusUI.SetActive(false);

            PreguntaDB preguntaDB = new PreguntaDB();

            IDataReader dataReader = preguntaDB.filtroPorTemaPreguntas(filtro);

            comboPreguntas.Clear();

            while (dataReader.Read())
            {
                Pregunta pregunta = new Pregunta(dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString(),
                    dataReader[4].ToString(), dataReader[5].ToString(), dataReader[6].ToString(), "", "", "", "", dataReader[7].ToString());
                comboPreguntas.Add(pregunta);
            }
            preguntasTotal = comboPreguntas.Count;

            m_numeroCoincidenciasTxt.text = comboPreguntas.Count.ToString();
        }


    }

    public void IniciarPartidaPorTema()
    {
        m_buscarInput.text = "";
        ShowPreguntas();
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
        MunicipioDB municipioDB = new MunicipioDB();

        IDataReader reader = municipioDB.countMunicipios();

        int registrosMunicipios = int.Parse(reader[0].ToString());

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
            
        }        
    }

    public void BackHome()
    {
        ShowHome();
    }

    
}