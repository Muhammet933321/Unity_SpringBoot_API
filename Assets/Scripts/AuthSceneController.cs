using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthSceneController : MonoBehaviour
{
    GameObject Server;
    ServerController serverController;
    private void Start()
    {
        Server = GameObject.FindGameObjectWithTag("Server");
        serverController = Server.GetComponent<ServerController>();
        StartCoroutine(serverController.GetWeapons());

    }

    void Update()
    {
        
    }
}
