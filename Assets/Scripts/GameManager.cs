using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool[] takenList = new bool[11];

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public int currentMeetingHost;
    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject corpsePrefab;
    public GameObject killAnimationPrefab;
    public GameObject[] lights = new GameObject[16];
    public Light ghostLight;
    public GameObject map;

    public Material[] colours;
    public Button[] buttons;

    public GameObject meetingDisplay;
    public bool taskActive;
    public bool startedGame = false;

    private void Awake()
    {
        for (int i = 0; i < takenList.Length; i++)
        {
            takenList[i] = false;
        }
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        ghostLight.enabled = false;
    }

    private void Update()
    {

        for (int i = 0; i < buttons.Length; i++)
        {

            if (GameManager.instance.takenList[i] == true)
            {
                if (buttons[i] == null)
                {
                    return;
                }
                buttons[i].interactable = false;
                buttons[i].image.enabled = false;
            }
            else if (GameManager.instance.takenList[i] == false)
            {
                if (buttons[i] == null)
                {
                    return;
                }
                buttons[i].interactable = true;
                buttons[i].image.enabled = true;
            }
        }
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation, int _colourID)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
            _player.GetComponent<PlayerManager>().colourID = _colourID;
            _player.GetComponentInChildren<MeshRenderer>().material = GameManager.instance.colours[_colourID];
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
        
    public void SpawnCorpse(Vector3 _position, Quaternion _rotation, int _colourID)
    {
        GameObject _corpse = Instantiate(corpsePrefab, _position, _rotation);
        _corpse.GetComponent<MeshRenderer>().material = GameManager.instance.colours[_colourID];
    }

    public static void ColourTaken(int _colourId)
    {
        instance.buttons[_colourId].gameObject.SetActive(false);
    }
}