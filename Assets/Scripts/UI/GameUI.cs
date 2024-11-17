using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    
    GameObject Server;
    private ServerController serverController;

    [Header("Scenes")]
    public GameObject mainMenuScene;
    public GameObject showDataScene;
    public GameObject updateDataScene;

    [Header("Show Datas")]
    public Text idTxt;
    public Text nameTxt;
    public Text passwordTxt;
    public Text levelTxt;
    public Text gold_amountTxt;
    public Text experience_levelTxt;

    [Header("Imput Fields")]
    public TMP_InputField nameInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField levelInputField;
    public TMP_InputField goldInputField;
    public TMP_InputField xpInputField;



    
    private void Start()
    {
        BackMainMenuBtn();
        Server = GameObject.FindGameObjectWithTag("Server");
        serverController = Server.GetComponent<ServerController>();
        StartCoroutine(GetDataAndLoad());

    }
    public void DisableAllScene()
    {
        mainMenuScene.SetActive(false);
        showDataScene.SetActive(false);
        updateDataScene.SetActive(false);
    }
    public void ShowDataSceneBtn()
    {
        DisableAllScene();
        showDataScene.SetActive(true);
    }

    public void PlayBtnClick()
    {
        SceneManager.LoadScene("Game");
    }
    public void UpdateDataSceneBtn()
    {
        DisableAllScene() ;
        updateDataScene.SetActive(true);
    }

    public void BackMainMenuBtn()
    {
        DisableAllScene();
        mainMenuScene.SetActive(true);
    }

    private IEnumerator GetDataAndLoad()
    {
        yield return StartCoroutine(serverController.GetDataWithName());
        LoadData();
    }
    public void LoadData()
    {
        Debug.Log("Data Loading");
        idTxt.text = DataBase.id;
        nameTxt.text = DataBase.name;
        passwordTxt.text = DataBase.password;
        levelTxt.text = DataBase.level;
        gold_amountTxt.text = DataBase.gold;
        experience_levelTxt.text = DataBase.xp;

        nameInputField.text = DataBase.name;
        passwordInputField.text = DataBase.password;
        levelInputField.text = DataBase.level;
        goldInputField.text = DataBase.gold;
        xpInputField.text = DataBase.xp;
    }
    public void LoadLeaderBoard()
    {
        Debug.Log("Leader Board Loading");
        for (int i = 0; i < DataBase.leaderBoard.Length; i--)
        {
            DataBase.leaderBoard[i, i] = DataBase.leaderBoard[i, i];
            DataBase.leaderBoard[i, i] = DataBase.leaderBoard[i, i];

            Debug.Log($"{i}. Username: {DataBase.name}, Level: {DataBase.level}");
        }
    }

    private IEnumerator GetLeaderBoard()
    {
        yield return StartCoroutine(serverController.GetLeaderboard());
        LoadData();
    }

    private IEnumerator UpdateDataAndLoad()
    {
        yield return StartCoroutine(serverController.UpdateData(
            int.Parse(DataBase.id),
            nameInputField.text,
            passwordInputField.text,
            int.Parse(levelInputField.text),
            int.Parse(goldInputField.text),
            int.Parse(xpInputField.text)));
        LoadData();
    }

    public void UpdateDataBtn()
    {
        Debug.Log("Update Btn Clicked");
        StartCoroutine(UpdateDataAndLoad());
    }

  




}
