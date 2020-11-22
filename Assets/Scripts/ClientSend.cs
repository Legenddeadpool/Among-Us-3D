using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{/// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {   
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerKill(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerKill))
        {
            _packet.Write(_facing);
            SendTCPData(_packet);
            Debug.Log("Command Received");
        }
    }

    public static void PlayerSelectedColour(float _colourId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerSelectedColour))
        {
            _packet.Write(_colourId);

            SendTCPData(_packet);
        }

        Debug.Log($"Selected Colour {_colourId}");
    }

    public static void StartGame() {
        using (Packet _packet = new Packet((int)ClientPackets.startGame)) 
        {
            SendTCPData(_packet);
        }
    }

    public static void StartMeeting()
    {
        using (Packet _packet = new Packet((int)ClientPackets.startMeeting))
        {
            SendTCPData(_packet);
        }
    }

    public static void Vote(int _id)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerVoted))
        {
            _packet.Write(_id);
            SendTCPData(_packet);
        }
    }
#endregion
}
