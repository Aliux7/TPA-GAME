using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Animator _animator;
    private float timeLight = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeLight >= 20f)
        {
            if (!_animator.GetBool("goToCycle"))
            {
                _animator.SetBool("goToCycle", true);
                timeLight = Time.time;
            }
            if (_animator.GetBool("isNight"))
            {
                _animator.SetBool("isNight", false);
                timeLight = Time.time;
            }
            else
            {
                _animator.SetBool("isNight", true);
                timeLight = Time.time;
            }

        }
    }
}
