using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstUIController : MonoBehaviour
{
    GameObject Server;
    private ServerController serverController;

    public GameObject LoadingObj;
    public GameObject TryAgainBtnOby;
    public Text errorTXT;

    private void Start()
    {
        Server = GameObject.FindGameObjectWithTag("Server");
        serverController = Server.GetComponent<ServerController>();

        LoadingObj.SetActive(true);
        TryAgainBtnOby.SetActive(false);

        StartCoroutine( TryConnactin());

    }

    public void TryAgainBtn()
    {

        StartCoroutine(TryConnactin());
    }

    private IEnumerator TryConnactin()
    {
        errorTXT.text = "";
        DataBase.errorTXT = null;
        LoadingObj.SetActive(true);
        TryAgainBtnOby.SetActive(false);

        yield return StartCoroutine(serverController.GetDataTest());

        if (!string.IsNullOrEmpty(DataBase.errorTXT))
        {
            errorTXT.text = DataBase.errorTXT;
            DataBase.errorTXT = null;

            LoadingObj.SetActive(false);
            TryAgainBtnOby.SetActive(true);
        }

    }


}
