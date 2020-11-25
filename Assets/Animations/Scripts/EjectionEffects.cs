using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectionEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EjectReveal()
    {
        if (GameManager.instance.latestEject.role == "Impostor")
        {
            UIManager.instance.ejectText.text = $"{GameManager.instance.latestEject.username} was the Impostor.";
        }
        else if (GameManager.instance.latestEject.role == "Crewmate")
        {
            UIManager.instance.ejectText.text = $"{GameManager.instance.latestEject.username} was not the Impostor.";
        }
        UIManager.instance.ejectText.enabled = true;
    }

    public void EjectEnd()
    {
        GameManager.instance.StopEject();
        UIManager.instance.ejectText.enabled = false;
    }
}
