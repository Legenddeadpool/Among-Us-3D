using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public GameObject UIObject;
    public bool aliveOnly = false;

    void Awake () 
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractStart() 
    {
        if(!GameManager.instance.taskActive)
        {
            Instantiate(UIObject);
            GameManager.players[Client.instance.myId].GetComponentInChildren<CameraController>().ToggleCursorMode();
            GameManager.instance.taskActive = true;
        }
    }
}
