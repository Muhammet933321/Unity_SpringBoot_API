using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerUI : MonoBehaviour
{
    GameObject Server;
    public TMP_InputField nameTxt;
    public TMP_InputField passwordTxt;
    public Text errorTxt;
    private void Start()
    {
        Server = GameObject.FindGameObjectWithTag("Server");
    }

    public void LogInBtn()
    {
        StartCoroutine(LogIn());
    }

    public void SignUpBtn()
    {
        StartCoroutine(SignUp());
    }

    private IEnumerator SignUp()
    {
        yield return StartCoroutine(Server.GetComponent<ServerController>().SignUp(nameTxt.text, passwordTxt.text));
        if (!string.IsNullOrEmpty(DataBase.errorTXT))
        {
            errorTxt.text = DataBase.errorTXT;
        }
        else
        {
            DataBase.errorTXT = null;
        }
    }

    private IEnumerator LogIn()
    {
        yield return StartCoroutine(Server.GetComponent<ServerController>().Authentication(nameTxt.text, passwordTxt.text));
        if (!string.IsNullOrEmpty(DataBase.errorTXT))
        {
            errorTxt.text = DataBase.errorTXT;
        }
        else
        {
            DataBase.errorTXT = null;
        }
    }

}
