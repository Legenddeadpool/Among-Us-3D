using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    public static TaskUI instance;

    void Awake() 
    {
        instance = this;
    }
    void Start() 
    {
    }

    void Update() 
    {

    }

    public void InteractExit() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.instance.taskActive = false;
        Destroy(this.gameObject);
    }

}