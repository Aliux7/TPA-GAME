using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public Camera MainCam;

    // Update is called once per frame
    void Update()
    {
        Ray ray = MainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        transform.position = ray.GetPoint(1000);
    }
}
