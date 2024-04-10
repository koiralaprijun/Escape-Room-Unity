using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public GameObject key;
    public GameObject button;
    public GameObject code;

    private AudioSource audioSource;
    private Vector3 scaleChange, positionChange;
    private bool played = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        code.SetActive(false);
    }

    // Gets called at the start of the collision
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "rust_key" && other.gameObject != null)
        {
            Debug.Log("Colliding");
            if (!played) audioSource.Play();
            played = true;
            Invoke("Delayer", 4);
            Destroy(gameObject, 4);
            Destroy(other.gameObject, 4);
        }
    }

    void Delayer()
    {
        code.SetActive(true);
    }
}
