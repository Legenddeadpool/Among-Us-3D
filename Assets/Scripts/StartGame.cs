using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartGameButton() {
        ClientSend.StartGame();
        UIManager.instance.hostMenu.SetActive(false);
        UIManager.instance.colourUI.SetActive(false);
        GameManager.instance.startedGame = true;
    }
}
