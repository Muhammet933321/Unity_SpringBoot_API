using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowCameraSC : MonoBehaviour
{
    public Vector3 CameraOffSet;
    public GameObject playerOBj;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOBj != null)
        {
            transform.position = Vector3.Lerp(transform.position, CameraOffSet + playerOBj.transform.position, 0.03f);
        }
    }
}
