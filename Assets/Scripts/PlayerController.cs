using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public Camera playerCamera = new Camera();
    public LayerMask aliveMask = new LayerMask();
    public LayerMask deadMask = new LayerMask();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClientSend.PlayerKill(camTransform.forward);
        }
        
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Interact(camTransform.forward);
        }
    }
    private void FixedUpdate()
    {
        SendInputToServer();

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseMenu.instance.gameObject.SetActive(true);
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                GetComponentInChildren<CameraController>().ToggleCursorMode();
            }
        }
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };


        ClientSend.PlayerMovement(_inputs);
    }

    public void Interact(Vector3 _viewDirection) 
    {
        Debug.DrawRay(camTransform.position, _viewDirection * 2f);
        if (Physics.Raycast(camTransform.position, _viewDirection, out RaycastHit _hit, 2f)) 
        {
            if (_hit.collider.CompareTag("Interactable")) 
            {
                if(GameManager.players[Client.instance.myId].isDead)
                {
                    if(_hit.collider.GetComponent<InteractManager>().aliveOnly)
                    {
                        return;
                    }
                }
                _hit.collider.GetComponent<InteractManager>().InteractStart();
                Debug.Log("Interacting with task.");
            }
        }
    }
}