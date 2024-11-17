using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ServerController : MonoBehaviour
{
    private string AuthURL = "http://localhost:8080/api/users/authenticate";
    private string GetDataURL = "http://localhost:8080/api/users/get";
    private string UpdateDataURL = "http://localhost:8080/api/users/update";
    private string addUserURL = "http://localhost:8080/api/users/add";
    private string weaponGetURL = "http://localhost:8080/api/weapons";

    private string URLTest = "http://localhost:8080/test";



    private void Start()
    {
        
    }

    [System.Serializable]
    public class UserData
    {
        public string username;
        public string password;
    }
    public IEnumerator Authentication(string username, string password)
    {
        
        UserData userData = new UserData { username = username, password = password };
        string jsonString = JsonUtility.ToJson(userData);

        using (UnityWebRequest request = new UnityWebRequest(AuthURL, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Ýsteði gönder
            yield return request.SendWebRequest();

            // Hata durumunu kontrol et
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (request.responseCode == 401)
                    {
                        DataBase.errorTXT = "Invalid Password";
                        Debug.LogError("Invalid Password");
                        

                    }
                    else if (request.responseCode == 404)
                    {
                        Debug.LogError("User Nor Found");
                        DataBase.errorTXT = "User Nor Found";

                    }
                    else
                    {
                        Debug.LogError("Error: " + request.error);
                        DataBase.errorTXT = "Error: " + request.error;

                    }
                    
                }
                else
                {
                    DataBase.name = username;
                    DataBase.password = password;
                    Debug.Log("Giriþ baþarýlý: " + request.downloadHandler.text);
                    
                    SceneManager.LoadScene("GameScene");
                }
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                DataBase.name = username;
                DataBase.password = password;
                
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    public IEnumerator SignUp(string username, string password)
    {
        // Kullanýcý verilerini JSON formatýnda hazýrlayýn
        string jsonData = JsonUtility.ToJson(new User(username, password));

        // POST isteði oluþtur
        using (UnityWebRequest request = new UnityWebRequest(addUserURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Ýsteði gönder
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User added successfully!");

                DataBase.name = username;
                DataBase.password = password;

                SceneManager.LoadScene("GameScene");

            }
            else
            {
                Debug.LogError("Error adding user: " + request.error);
                if (request.responseCode == 400)
                {
                    DataBase.errorTXT = "This Name Already Using";
                }
                else
                {
                    DataBase.errorTXT = request.error;
                }
                
            }
        }
    }


    public IEnumerator GetWeapons()
    {
        Debug.Log("Fetching weapons from API...");

        using (UnityWebRequest request = UnityWebRequest.Get(weaponGetURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Weapons data fetched successfully!");

                string jsonResponse = request.downloadHandler.text;

                // JSON'u SimpleJSON ile parse et ve WeaponList'e aktar
                JSONNode weaponsNode = SimpleJSON.JSONNode.Parse(jsonResponse);

                if (weaponsNode.IsArray)
                {
                    // WeaponList sýnýfýný API verileriyle doldur
                    DataBase.weapons = new Weapon[weaponsNode.Count];
                    for (int i = 0; i < weaponsNode.Count; i++)
                    {
                        Weapon weapon = new Weapon
                        {
                            id = weaponsNode[i]["id"].AsLong,
                            name = weaponsNode[i]["name"],
                            damage = weaponsNode[i]["damage"].AsInt,
                            cooldown = weaponsNode[i]["cooldown"].AsDouble
                        };

                        DataBase.weapons[i] = weapon;
                    }

                    Debug.Log($"Fetched {DataBase.weapons.Length} weapons.");
                    foreach (var weapon in DataBase.weapons)
                    {
                        Debug.Log($"ID: {weapon.id}, Name: {weapon.name}, Damage: {weapon.damage}, Cooldown: {weapon.cooldown}");
                    }
                }
                else
                {
                    Debug.LogError("Unexpected JSON format: " + jsonResponse);
                }
            }
        }
    }


    public IEnumerator UpdateData(int id, string username, string password, int level, int goldAmount, int experienceLevel)
    {
        // JSON nesnesi oluþturuyoruz
        var userData = new SimpleJSON.JSONObject();
        userData["id"] = id;
        userData["username"] = username;
        userData["password"] = password;
        userData["level"] = level;
        userData["goldAmount"] = goldAmount;
        userData["experienceLevel"] = experienceLevel;

        Debug.Log("Data Updating");

        string jsonData = userData.ToString();

        using (UnityWebRequest request = new UnityWebRequest(UpdateDataURL+"/"+DataBase.id, "PUT"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Data successfully updated: " + request.downloadHandler.text);

                DataBase.id = id.ToString();
                DataBase.name = username;
                DataBase.password = password;
                DataBase.level = level.ToString();
                DataBase.gold = goldAmount.ToString();
                DataBase.xp = experienceLevel.ToString();

            }
        }
    }


    public IEnumerator GetData()
    {
        Debug.Log("Datas Getting");
        using (UnityWebRequest request = UnityWebRequest.Get(GetDataURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                DataBase.errorTXT = request.error;
            }
            else
            {
                Debug.Log("Data Getting Success");
                string json = request.downloadHandler.text;

                SimpleJSON.JSONNode stats = SimpleJSON.JSONNode.Parse(json);

                // Eðer JSON dizisi ise
                if (stats.IsArray && stats.Count > 0)
                {
                    DataBase.id = stats[0]["id"].ToString();
                    DataBase.level = stats[0]["level"].ToString();
                    DataBase.gold = stats[0]["goldAmount"].ToString();
                    DataBase.xp = stats[0]["experienceLevel"].ToString();
                }
                else if (stats.IsObject) // JSON tek bir nesne ise
                {
                    DataBase.id = stats["id"].ToString();
                    DataBase.level = stats["level"].ToString();
                    DataBase.gold = stats["goldAmount"].ToString();
                    DataBase.xp = stats["experienceLevel"].ToString();
                }
                else
                {
                    Debug.LogError("Unexpected JSON format: " + json);
                }
            }
        }
    }

    public IEnumerator GetDataWithName()
    {
        Debug.Log("Datas Getting");
        using (UnityWebRequest request = UnityWebRequest.Get(GetDataURL+ "/by-username/" + DataBase.name))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                DataBase.errorTXT = request.error;
            }
            else
            {
                Debug.Log("Data Getting Success");
                string json = request.downloadHandler.text;

                SimpleJSON.JSONNode stats = SimpleJSON.JSONNode.Parse(json);

                // Eðer JSON dizisi ise
                if (stats.IsArray && stats.Count > 0)
                {
                    DataBase.id = stats[0]["id"].ToString();
                    DataBase.level = stats[0]["level"].ToString();
                    DataBase.gold = stats[0]["goldAmount"].ToString();
                    DataBase.xp = stats[0]["experienceLevel"].ToString();
                }
                else if (stats.IsObject) // JSON tek bir nesne ise
                {
                    DataBase.id = stats["id"].ToString();
                    DataBase.level = stats["level"].ToString();
                    DataBase.gold = stats["goldAmount"].ToString();
                    DataBase.xp = stats["experienceLevel"].ToString();
                }
                else
                {
                    Debug.LogError("Unexpected JSON format: " + json);
                }
            }
        }
    }

    public IEnumerator GetDataWithId()
    {
        Debug.Log("Fetching data by ID");
        using (UnityWebRequest request = UnityWebRequest.Get(GetDataURL + "/by-id/" + DataBase.id))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                DataBase.errorTXT = request.error;
            }
            else
            {
                Debug.Log("Data fetched successfully");
                string json = request.downloadHandler.text;

                SimpleJSON.JSONNode stats = SimpleJSON.JSONNode.Parse(json);

                // Eðer JSON dizisi deðil de tek bir nesne dönüyorsa
                if (stats.IsObject)
                {
                    DataBase.id = stats["id"].ToString();
                    DataBase.name = stats["username"].ToString();
                    DataBase.password = stats["password"].ToString();
                    DataBase.level = stats["level"].ToString();
                    DataBase.gold = stats["goldAmount"].ToString();
                    DataBase.xp = stats["experienceLevel"].ToString();
                }
                else
                {
                    Debug.LogError("Unexpected JSON format: " + json);
                }
            }
        }
    }


    public IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/api/users/leaderboard"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode users = SimpleJSON.JSONNode.Parse(json);

                for (int i = 0; i< users.Count; i--)
                {
                    DataBase.leaderBoard[i,i] = users["username"].ToString();
                    DataBase.leaderBoard[i, i] = users["level"].ToString();
                
                    Debug.Log($"{i}. Username: {DataBase.name}, Level: {DataBase.level}");
                }
            }
        }
    }




    public IEnumerator GetDataTest()
    {
        
        using (UnityWebRequest request = UnityWebRequest.Get(URLTest))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
                DataBase.errorTXT = request.error;
            }
            else
            {
                SceneManager.LoadScene("AuthScene");
            }
        }
    }


    IEnumerator PostData(string username, string email, string password, int level, int goldAmount, int experienceLevel)
    {

        // Create JSON using SimpleJSON
        var userData = new SimpleJSON.JSONObject();
        userData["username"] = username;
        userData["password"] = password;
        userData["level"] = level;
        userData["gold_amount"] = goldAmount;
        userData["experience_level"] = experienceLevel;

        string jsonData = userData.ToString();

        using (UnityWebRequest request = new UnityWebRequest(AuthURL, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Data successfully posted: " + request.downloadHandler.text);
            }
        }
    }

    

    [System.Serializable]
    public class User
    {
        public string username;
        public string password;

        public User(string name, string password)
        {
            this.username = name;
            this.password = password;
        }
    }

}
