using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayableDirector doorOpen, doorClose;
    private MeshCollider meshCollider;
    private bool isClosed = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (!isClosed)
                {
                    doorOpen.Play();
                }
                else
                {
                    doorClose.Play();
                }
            }
        }
    }
}
