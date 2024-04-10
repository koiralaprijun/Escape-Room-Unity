using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;
using UnityEngine.UI;

public class WheelLock : MonoBehaviour
{
    public Text result;
    public AudioClip audioCorrect;
    public Renderer windowframe;
    public Material matNeutral, matCorrect;
    public Door door;

    private int[] currentCombination = new int[]{0, 0, 0};
    private int[] correctCombination = new int[]{2, 1, 9};
    private AudioSource resultAudioSource;

    private const int LOCK_NUM = 1;

    private void Start()
    {
        resultAudioSource = GetComponent<AudioSource>();
        resultAudioSource.clip = audioCorrect;
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "WheelKey")
            {
                // rotate the specific wheel
                hit.transform.parent.GetComponent<WheelLockRotation>().OnHit();
            }
        }
    }

    public void CheckResult(string wheelName)
    {
        switch (wheelName)
        {
            case "Wheel1":
                currentCombination[0] = (currentCombination[0] + 1) % 10;
                break;

            case "Wheel2":
                currentCombination[1] = (currentCombination[1] + 1) % 10;
                break;

            case "Wheel3":
                currentCombination[2] = (currentCombination[2] + 1) % 10;
                break;
        }

        if (currentCombination[0] == correctCombination[0] &&
            currentCombination[1] == correctCombination[1] &&
            currentCombination[2] == correctCombination[2]
        ) {
            result.text = "Winner";
            windowframe.material = matCorrect;
            resultAudioSource.Play();
            door.SetLockStatus(LOCK_NUM, true);
        }
        else {
            result.text = currentCombination[0] + "" + currentCombination[1] + "" + currentCombination[2];
            windowframe.material = matNeutral;
            door.SetLockStatus(LOCK_NUM, false);
        }
    }
}
