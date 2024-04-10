using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ButtonLock : MonoBehaviour
{
    public Text debugText;
    public Renderer[] asterisks;
    public Material matDisabled, matEnabled, matIncorrect, matCorrect;
    public AudioSource resultAudioSource;
    public AudioClip audioButton, audioIncorrect, audioCorrect;
    public Door door;

    private AudioSource buttonAudioSource;
    private Transform pressedButton = null;
    private string attemptedCode = "";
    private string correctCode = "734";

    private const int LOCK_NUM = 0;
    private const int REACH_RANGE = 100;
    private const float BUTTON_PRESSED_SPATIAL_FEEDBACK = 0.05f;

    void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
        buttonAudioSource.clip = audioButton;
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (pressedButton == null && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, REACH_RANGE) && hit.transform.tag == "ButtonKey")
            {
                pressedButton = hit.transform;

                string value = pressedButton.name;
                SetValue(value);

                // feedback
                pressedButton.localPosition -= new Vector3(0, 0, BUTTON_PRESSED_SPATIAL_FEEDBACK);
                buttonAudioSource.Play();
                //Handheld.Vibrate();
            }
        }

        if (pressedButton != null && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // feedback
            pressedButton.localPosition += new Vector3(0, 0, BUTTON_PRESSED_SPATIAL_FEEDBACK);
            //Handheld.Vibrate();

            // needs to be last for some reason, otherwise what comes after won't run
            pressedButton = null;
        }
    }

    void SetValue(string value)
    {
        attemptedCode += value;
        debugText.text = attemptedCode;

        if (attemptedCode.Length == correctCode.Length)
        {
            CheckCode();
            attemptedCode = "";
        }
        else if (attemptedCode.Length == 1)
        {
            asterisks[0].material = matEnabled;
            asterisks[1].material = matDisabled;
            asterisks[2].material = matDisabled;
        }
        else if (attemptedCode.Length == 2)
        {
            asterisks[1].material = matEnabled;
        }
    }

    void CheckCode()
    {
        if (attemptedCode == correctCode)
        {
            debugText.text += " Winner";
            resultAudioSource.clip = audioCorrect;
            resultAudioSource.Play();

            asterisks[0].material = matCorrect;
            asterisks[1].material = matCorrect;
            asterisks[2].material = matCorrect;

            door.SetLockStatus(LOCK_NUM, true);
        }
        else
        {
            debugText.text += " Loser";
            resultAudioSource.clip = audioIncorrect;
            resultAudioSource.Play();

            asterisks[0].material = matIncorrect;
            asterisks[1].material = matIncorrect;
            asterisks[2].material = matIncorrect;

            door.SetLockStatus(LOCK_NUM, false);
        }
    }
}
