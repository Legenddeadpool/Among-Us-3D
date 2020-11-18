using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSelect : MonoBehaviour
{
    public float colourId;  // Value specified in Inspector which is unique to each button.
    public void SelectColour()
    {
        ClientSend.PlayerSelectedColour(colourId);
        UIManager.instance.colourUI.SetActive(false);
    }
}
