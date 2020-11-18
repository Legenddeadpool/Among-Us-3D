using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        instance.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMenu() 
    {
        instance.gameObject.SetActive(false);
        GameManager.players[Client.instance.myId].GetComponentInChildren<CameraController>().ToggleCursorMode();
    }

    public void Leave() 
    {
        Application.Quit();
    }
}
