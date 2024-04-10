using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VBTNReveal : MonoBehaviour
{
    public GameObject cube;
    public Animator cubeAni;
    public VirtualButtonBehaviour vb;

    void Start()
    {
        vb.RegisterOnButtonPressed(OnButtonPressed);
        vb.RegisterOnButtonReleased(OnButtonReleased);

        cube.SetActive(false);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        cube.SetActive(true);
        cubeAni.Play("cube-animation");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        cube.SetActive(false);
        cubeAni.Play("none");
    }
}
