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
        UIManager.instance.ejectText.text = $"{GameManager.instance.latestEject} has been ejected.";
        UIManager.instance.ejectText.enabled = true;
    }

    public void EjectEnd()
    {
        GameManager.instance.StopEject();
        UIManager.instance.ejectText.enabled = false;
    }
}
