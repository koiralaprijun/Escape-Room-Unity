using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAndButton : MonoBehaviour
{
    public GameObject button;
    public GameObject key;

    private AudioSource audioSource;
    private Vector3 btnScaleOriginal, btnScalePressed;

    void Start()
    {
        audioSource = button.GetComponent<AudioSource>();
        key.SetActive(false);

        btnScaleOriginal = button.transform.localScale;
        btnScalePressed = new Vector3(btnScaleOriginal.x, btnScaleOriginal.y / 2, btnScaleOriginal.z);
    }

    // Gets called at the start of the collision
    void OnTriggerEnter(Collider other)
    {
        if (other is BoxCollider)
        {
            button.transform.localScale = btnScalePressed;
            //Debug.Log(key.activeSelf);
            audioSource.Play();
            if (key != null) key.SetActive(true);
        }
    }

    // Gets called when the object exits the collision
    void OnTriggerExit(Collider other)
    {
        if (other is BoxCollider)
        {
            button.transform.localScale = btnScaleOriginal;
            //Debug.Log("No longer colliding with cube");
            audioSource.Play();
            if (key != null) key.SetActive(false);
        }
    }
}
