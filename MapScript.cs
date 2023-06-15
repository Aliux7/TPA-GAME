using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject playerStatus, map;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerStatus.SetActive(false);
            map.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            playerStatus.SetActive(true);
            map.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerStatus.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            playerStatus.SetActive(true);
        }
    }
}
