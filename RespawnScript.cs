using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnFloor;
    [SerializeField] private Transform cameraMinimap;
    void Start()
    {
        player.transform.position = respawnFloor.transform.position;   
    }

}
