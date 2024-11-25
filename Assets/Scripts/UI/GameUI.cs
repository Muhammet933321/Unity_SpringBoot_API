using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public GameObject leaderBoardScene;
    public GameObject showDataScene;
    public GameObject updateDataScene;

    [Header("Show Datas")]
    public Text idTxt;
    public Text nameTxt;
    public Text passwordTxt;
    public Text levelTxt;
    public Text gold_amountTxt;
    public Text experience_levelTxt;
    public Text[] LeaderBoardStatic;
    public Text[] LeaderBoardNames;
    public Text[] LeaderBoardLevels;
    



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
        StartCoroutine(serverController.GetLeaderboard());

    }
    public void DisableAllScene()
    {
        mainMenuScene.SetActive(false);
        showDataScene.SetActive(false);
        updateDataScene.SetActive(false);
        leaderBoardScene.SetActive(false);
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

    public void LeaderBoardBtn()
    {
        LoadLeaderBoard();
        DisableAllScene();
        leaderBoardScene.SetActive(true);
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

    private IEnumerator GetLeaderBoardAndLoad()
    {
        yield return StartCoroutine(serverController.GetLeaderboard());
        LoadLeaderBoard();
    }
    public void LoadData() // Show User Data
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

        int lengt; 
        if (DataBase.leaderBoard.Length < LeaderBoardNames.Length)
        {
            lengt = DataBase.leaderBoard.Length;
        }
        else
        {
            lengt = LeaderBoardNames.Length;
        }
        

        for (int i = 0; i < lengt; i++)
        {
            LeaderBoardNames[i].text = DataBase.leaderBoard[i, 0];
            LeaderBoardLevels[i].text = DataBase.leaderBoard[i, 1];
            if (DataBase.leaderBoard[i, 0] == DataBase.name)
            {
                LeaderBoardStatic[i].color = Color.red;
                LeaderBoardLevels[i].color = Color.red;
                LeaderBoardNames[i].color = Color.red;
            }
            
            Debug.Log($"{i}. Username: {DataBase.leaderBoard[i, 0]}, Level: {DataBase.leaderBoard[i, 1]}");
        }
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
