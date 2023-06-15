using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightScript : MonoBehaviour
{
    public Light Spotlight;

    // Start is called before the first frame update
    void Start()
    {
        Spotlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        Spotlight.enabled = true;
    }

    void OnMouseExit()
    {
        Spotlight.enabled = false;
    }
}