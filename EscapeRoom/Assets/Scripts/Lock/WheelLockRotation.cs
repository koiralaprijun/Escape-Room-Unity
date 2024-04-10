using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

public class WheelLockRotation : MonoBehaviour
{
    public AudioClip audioRotation;

    private AudioSource audioSource;
    private bool coroutineAllowed = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioRotation;
    }

    public void OnHit()
    {
        if (coroutineAllowed)
        {
            audioSource.Play();
            StartCoroutine("RotateWheel");
        }
    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;

        for (int i = 0; i <= 11; i++)
        {
            transform.Rotate(-3f, 0, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;

        // update value in WheelLock
        transform.parent.GetComponent<WheelLock>().CheckResult(gameObject.name);
    }
}
