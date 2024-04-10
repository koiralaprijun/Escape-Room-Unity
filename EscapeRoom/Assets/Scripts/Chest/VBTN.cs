using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VBTN : MonoBehaviour
{
    public GameObject button;
    public VirtualButtonBehaviour vb;
    public GameObject key;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = button.GetComponent<AudioSource>();
        vb.RegisterOnButtonPressed(OnButtonPressed);
        vb.RegisterOnButtonReleased(OnButtonReleased);
        key.SetActive(false);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb) {
        button.transform.localScale = new Vector3(0.5f, 0.05f, 0.5f);
        //Debug.Log("Pressing button");
        audioSource.Play();
        if (key != null) key.SetActive(true);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        button.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
        //Debug.Log("Pressing button");
        audioSource.Play();
        if (key != null) key.SetActive(false);
    }
}
