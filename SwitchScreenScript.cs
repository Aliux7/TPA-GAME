using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScreenScript : MonoBehaviour
{
    public Transform cameraTarget1;
    public Transform cameraTarget2;
    public float switchSpeed = 10.0f;
    public Vector3 dist;
    public Transform lookTarget;

    private int currentTarget;
    private Transform cameraTarget;

    public void SetCameraTarget(int num)
    {
        switch (num)
        {
            case 1:
                cameraTarget = cameraTarget1.transform;
                break;
            case 2:
                cameraTarget = cameraTarget2.transform;
                break;
        }
    }
    void Start()
    {
        currentTarget = 1;
        SetCameraTarget(currentTarget);
        
    }

    private void FixedUpdate()
    {
        Vector3 dPos = cameraTarget.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, switchSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }

    public void SwitchCamera()
    {
        if(currentTarget < 2)
        {
            currentTarget++;
        }
        else
        {
            currentTarget = 1;
        }
        SetCameraTarget(currentTarget);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
