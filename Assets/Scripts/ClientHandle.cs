using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        int _colourID = _packet.ReadInt();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation, _colourID);
        GameManager.players[_id].colourID = _colourID;
        if (Client.instance.myId == 1) {
            HostMenu.instance.InitializeHostMenu();
        }
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].SetPosition(_position);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerAnimation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _isMoving = _packet.ReadBool();
        Debug.Log($"Player: {GameManager.players[_id].username} Moving : {_isMoving}");
        GameManager.players[_id].AnimateMovement(_isMoving);
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
        Debug.Log($"Player {_id} has disconnected.");
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerDied(Packet _packet)
    {

        int _id = _packet.ReadInt();
        GameManager.players[_id].Die();
    }

    public static void PlayerAppliedColour(Packet _packet)
    {
        float _colourid = _packet.ReadFloat();
        int _id = _packet.ReadInt();
        GameManager.players[_id].GetComponentInChildren<MeshRenderer>().material = GameManager.instance.colours[(int)_colourid];
        GameManager.players[_id].colourID = (int)_colourid;
        Debug.Log($"Set Player {Client.instance.myId}'s colour to {_colourid}");
        Debug.Log($"Set Player {Client.instance.myId}'s colour to {GameManager.players[Client.instance.myId].colourID}");

    }

    public static void ColourList(Packet _packet)
    {
        Debug.Log("Received ColourList Packet.");
        bool[] _isTaken = new bool[_packet.ReadInt()];
        for (int i = 0; i < _isTaken.Length; i++)
        {
            _isTaken[i] = _packet.ReadBool();
        }
        GameManager.instance.takenList = _isTaken;
        Debug.Log("Received new colour list:");
        for (int i = 0; i < _isTaken.Length; i++)
        {
            Debug.Log(_isTaken[i]);
        }
    }

    public static void MeetingStarted(Packet _packet) 
    {
        int _meetingHost = _packet.ReadInt();
        GameManager.instance.currentMeetingHost = _meetingHost;
        Debug.Log("Emergency Meeting Started.");
        GameManager.instance.meetingDisplay = Instantiate(UIManager.instance.meetingScreen);
        UIManager.instance.inMeeting = true;
        GameManager.instance.taskActive = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public static void MeetingEnded(Packet _packet)
    {
        string victimName = _packet.ReadString();
        if (victimName != "Skip")
        {
            Debug.Log($"{victimName} was voted off.");
            foreach (PlayerManager _player in GameManager.players.Values)
            {
                if (_player.username == victimName && GameManager.players[Client.instance.myId].username != victimName)
                {
                    //Ensures they don't spawn a corpse
                    _player.wasVoted = true;
                    GameManager.instance.StartEject(_player);

                    //Turns them into a ghost.
                    _player.player.layer = 6;
                    for (int i = 0; i < _player.player.transform.childCount; i++)
                    {
                        _player.player.transform.GetChild(i).gameObject.layer = 6;
                    }
                    _player.isDead = true;
                }

                else if (_player.username == victimName && GameManager.players[Client.instance.myId].username == victimName)
                {
                    _player.wasVoted = true;
                    GameManager.players[Client.instance.myId].Die();
                    GameManager.instance.StartEject(_player);
                }
                
            }
        }
        else
        {
            Debug.Log("No players were voted off.");
        }

        Debug.Log("Meeting ended.");
        Destroy(GameManager.instance.meetingDisplay);
        UIManager.instance.inMeeting = false;
        GameManager.instance.taskActive = false;
        if (Cursor.lockState != CursorLockMode.Locked)
        {    
            GameManager.players[Client.instance.myId].GetComponent<CameraController>().ToggleCursorMode();
        }
    }

    public static void PlayerRoles(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _role = _packet.ReadString();
        GameManager.players[_id].role = _role;
        //Updating UI Elements to in-game states.
        UIManager.instance.mainUI.SetActive(true);
        UIManager.instance.lobbyUI.SetActive(false);
        if (_id == Client.instance.myId)
        {
            UIManager.instance.ShowRole(_role);
        }
    }
}