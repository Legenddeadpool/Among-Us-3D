using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmergencyMeeting : MonoBehaviour
{
    public int playerCount;

    public GameObject[] playerCards;
    public Image[] playerIcons;
    public Button[] voteButtons;

    public GameObject skipButton;
    public Color hostColor;

    void Awake()
    {
        playerCount = GameManager.players.Count;
        for (int i = 0; i < playerCards.Length; i++)
        {
            playerCards[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < playerCount; i++)
        {
            playerCards[i].gameObject.SetActive(true);
            playerCards[i].gameObject.GetComponentInChildren<TMP_Text>().text = GameManager.players[i + 1].username;
            if (GameManager.players[i + 1].id == GameManager.instance.currentMeetingHost)
            {
                playerCards[i].GetComponent<Image>().color = hostColor;
            }
            playerIcons[i].color = GameManager.instance.colours[GameManager.players[i + 1].colourID].color;
            //playerIcons[i].material = GameManager.instance.colours[GameManager.players[i + 1].colourID];

        }
        foreach (Button b in voteButtons)
        {
            if(b.GetComponent<VoteButton>().id == Client.instance.myId)
            {
                Destroy(b.gameObject);
            }
        }
    }

    public void Vote(int _id)
    {
        //Vote for target
        ClientSend.Vote(_id);
        //Checks if the target was a player or the skip button
        if (_id != 0)
        {
            Debug.Log($"Voting for Player {_id}");
        }
        else
        {
            Debug.Log("Skipped Voting");
        }

        foreach (Button b in voteButtons)
        {
            //The null check is necessary as the button corresponding to the player will be null
            if(b != null)
            {
                Destroy(b.gameObject);
            }
        }

        skipButton.SetActive(false);
    }
}
