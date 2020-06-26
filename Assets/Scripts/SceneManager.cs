using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;
    [SerializeField] private GameObject m_homeUI = null;

    [Header("Login")]
    [SerializeField] private Text m_infoLoginTxt = null;
    [SerializeField] private InputField m_emailLoginInput = null;
    [SerializeField] private InputField m_passwordLoginInput = null;


    [Header("Register")]   

    [SerializeField] private InputField m_nameInput = null;
    [SerializeField] private InputField m_emailInput = null;
    [SerializeField] private InputField m_edadInput = null;
    [SerializeField] private Dropdown m_sexoInput = null;
    [SerializeField] private InputField m_municipioInput = null;
    [SerializeField] private InputField m_passwordInput = null;
    [SerializeField] private Toggle m_privacidadToggle = null;

    [SerializeField] private Text m_infoErrorTxt = null;    

    private NetworkManager m_networkManager = null;

    private void Awake()
    {
        m_networkManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        ShowLogin();
    }

    public void ShowLogin()
    {
        m_infoLoginTxt.text = "";
        m_emailLoginInput.text = "";
        m_passwordLoginInput.text = "";

        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);
        m_homeUI.SetActive(false);
    }

    public void ShowRegister()
    {
        m_nameInput.text = "";
        m_emailInput.text = "";
        m_edadInput.text = "";
        m_municipioInput.text = "";
        m_passwordInput.text = "";
        m_infoErrorTxt.text = "";
        m_sexoInput.value = 0;
        m_privacidadToggle.isOn = false;
        m_infoLoginTxt.text = "";

        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
        m_homeUI.SetActive(false);
    }

    public void SubmitRegister()
    {
        string sexo = "";

        char[] charsToTrim = { '*', ' ', '\'' };
        m_nameInput.text = m_nameInput.text.Trim(charsToTrim);
        m_emailInput.text = m_emailInput.text.Trim(charsToTrim);
        m_edadInput.text = m_edadInput.text.Trim(charsToTrim);
        m_municipioInput.text = m_municipioInput.text.Trim(charsToTrim);
        m_passwordInput.text = m_passwordInput.text.Trim(charsToTrim);
        if (m_nameInput.text == "")
        {
            m_infoErrorTxt.text = "Ingrese su nombre";
            return;
        }
        if (m_emailInput.text == "")
        {
            m_infoErrorTxt.text = "Ingrese su e-mail";
            return;
        }
        if (m_edadInput.text == "")
        {
            m_infoErrorTxt.text = "Ingrese su edad";
            return;
        }
        if (m_sexoInput.value == 0)
        {
            m_infoErrorTxt.text = "Ingrese su sexo";
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
        if (m_municipioInput.text == "")
        {
            m_infoErrorTxt.text = "Ingrese su municipio";
            return;
        }
        if (m_passwordInput.text == "")
        {
            m_infoErrorTxt.text = "Ingrese su contraseña";
            return;
        }
        if (!m_privacidadToggle.isOn)
        {
            m_infoErrorTxt.text = "Aceptar el aviso de privacidad";
            return;
        }

        m_infoErrorTxt.text = "Procesando...";

        m_networkManager.CreateUserApp(m_nameInput.text, m_emailInput.text, Int32.Parse(m_edadInput.text), sexo,
            m_municipioInput.text, m_passwordInput.text, delegate (Response response)
            {
                if (response.message == "Usuario Registrado")
                {
                    m_infoErrorTxt.text = response.message;
                    m_infoLoginTxt.text = response.message + ", ya puedes iniciar sesión";
                    ShowLogin();
                }
                else
                {
                    m_infoErrorTxt.text = response.message;
                }
                
            });
    }

    public void SubmitLogin()
    {
        char[] charsToTrim = { '*', ' ', '\'' };
        m_emailLoginInput.text = m_emailLoginInput.text.Trim(charsToTrim);
        m_passwordLoginInput.text = m_passwordLoginInput.text.Trim(charsToTrim);

        if (m_emailLoginInput.text == "")
        {
            m_infoLoginTxt.text = "Ingrese su e-mail";
            return;
        }
        if (m_passwordLoginInput.text == "")
        {
            m_infoLoginTxt.text = "Ingrese su contraseña";
            return;
        }

        m_infoLoginTxt.text = "Procesando...";

        m_networkManager.LoginUserApp(m_emailLoginInput.text, m_passwordLoginInput.text, delegate (Response response)
            {
                m_infoLoginTxt.text = response.message;
                if (response.message == "Logueado")
                {
                    m_homeUI.SetActive(true);
                    m_registerUI.SetActive(false);
                    m_loginUI.SetActive(false);
                }
                else
                {
                    m_infoLoginTxt.text = response.message;
                }
            });
    }

    public void salirSesion()
    {
        ShowLogin();
    }
}
