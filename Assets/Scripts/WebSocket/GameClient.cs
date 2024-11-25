using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using WebSocketSharp;
using SimpleJSON;

public class GameWebSocketClient : MonoBehaviour
{
    private WebSocket ws;
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
    public GameObject characterPrefab;

    private Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();
    private string characterId;

    void Start()
    {
        characterId = DataBase.id;
        ws = new WebSocket("ws://localhost:8080/game");

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Sunucudan gelen mesaj: " + e.Data);
            messageQueue.Enqueue(e.Data);
        };

        ws.Connect();
        Invoke("SendInitialPosition", 1f);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SendInputData("move_forward");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            SendInputData("move_backward");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SendInputData("move_left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SendInputData("move_right");
        }

        while (messageQueue.TryDequeue(out string message))
        {
            ProcessServerResponse(message);
        }
    }

    void SendInputData(string action)
    {
        string data = JsonUtility.ToJson(new PlayerInput
        {
            id = characterId,
            action = action
        });

        ws.Send(data);
        Debug.Log("G�nderilen veri: " + data);
    }

    void SendInitialPosition()
    {
        string data = JsonUtility.ToJson(new PlayerInput
        {
            id = characterId,
            action = "initialize"
        });

        ws.Send(data);
        Debug.Log("Ba�lang�� pozisyonu g�nderildi: " + data);
    }

    private void ProcessServerResponse(string message)
    {
        try
        {
            Debug.Log("Sunucu yan�t� al�nd�: " + message);

            var response = JSON.Parse(message);

            if (response["status"] != "success")
            {
                Debug.LogError("Ge�ersiz sunucu yan�t�.");
                return;
            }

            var allPositions = response["allPositions"];
            if (allPositions == null)
            {
                Debug.LogError("allPositions null!");
                return;
            }

            foreach (var positionData in allPositions)
            {
                string id = positionData.Key;
                var pos = positionData.Value;

                if (!characters.ContainsKey(id))
                {
                    GameObject newCharacter = Instantiate(characterPrefab);
                    characters.Add(id, newCharacter);
                }

                Vector3 newPosition = new Vector3(
                    pos["x"].AsFloat,
                    pos["y"].AsFloat,
                    pos["z"].AsFloat
                );

                characters[id].transform.position = newPosition;

                // E�er gelen ID benim karakterimse, kameray� bu nesneyi takip ettir
                if (id == characterId)
                {
                    Camera.main.transform.position = new Vector3(
                        newPosition.x,
                        newPosition.y + 5f, // Kameray� biraz yukar� al
                        newPosition.z - 10f // Kameray� biraz geriye �ek
                    );

                    Camera.main.transform.LookAt(characters[id].transform);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Sunucu yan�t�n� i�lerken hata olu�tu: " + e.Message);
        }
    }



    [System.Serializable]
    public class PlayerInput
    {
        public string id;
        public string action;
    }

    [System.Serializable]
    public class ServerResponse
    {
        public string status;
        public string id; // Sunucudan gelen "id" alan�
        public Dictionary<string, Position> allPositions; // Karakter pozisyonlar�
    }

    [System.Serializable]
    public class Position
    {
        public float x, y, z; // Pozisyon koordinatlar�
    }

    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }
}
