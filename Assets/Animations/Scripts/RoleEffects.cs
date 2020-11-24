using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopAnimation()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("StopAnimation");
        gameObject.SetActive(false);
    }
}
