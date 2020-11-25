using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public GameObject UIObject;
    public bool aliveOnly = false;
    public bool crewOnly = false;

    public void InteractStart() 
    {
        if(!GameManager.instance.taskActive)
        {
            Instantiate(UIObject);
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                GameManager.players[Client.instance.myId].GetComponentInChildren<CameraController>().ToggleCursorMode();
            }
            GameManager.instance.taskActive = true;
        }
    }
}
