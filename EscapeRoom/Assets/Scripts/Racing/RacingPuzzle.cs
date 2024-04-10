using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RacingPuzzle : MonoBehaviour
{
    public Camera cam;
    public GameObject car;
    public GameObject key;
    public Text debugText;
    public AudioClip audioKeyReveal, audioCarDriving;

    private AudioSource keyAudioSource;
    private AudioSource carAudioSource;
    private bool gas = false;
    private int turn = 0;
    private bool isControllerTargetActive = false;
    private bool isCarTargetActive = false;
    private bool isKeyCollected = false;

    private const float PICKUP_DIST_THRESHOLD = 0.001f;
    private const float SPEED_GAS = 0.005f;
    private const float SPEED_TURN = 0.05f;

    void Start()
    {
        keyAudioSource = key.GetComponent<AudioSource>();
        keyAudioSource.clip = audioKeyReveal;

        carAudioSource = car.GetComponent<AudioSource>();
        carAudioSource.clip = audioCarDriving;
        carAudioSource.loop = true;
    }

    public void OnControllerTargetFound()
    {
        isControllerTargetActive = true;
    }

    public void OnControllerTargetLost()
    {
        isControllerTargetActive = false;
        // if the controller isn't detected, we want
        // the car to continue driving straight ahead
        turn = 0;
    }

    public void OnCarTargetFound()
    {
        isCarTargetActive = true;
    }

    public void OnCarTargetLost()
    {
        isCarTargetActive = false;
    }

    void Update()
    {
        UpdateTurn();
        //UpdateGas();

        Log("cam: " + (int) cam.transform.eulerAngles.z
            + " obj: " + (int) transform.eulerAngles.y + "; "
            + turn + " " + gas + "; key: " + isKeyCollected);
    }

    void FixedUpdate()
    {
        MoveCar();
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (!isKeyCollected &&
            (key.transform.position - car.transform.position).sqrMagnitude < PICKUP_DIST_THRESHOLD
        ) {
            key.transform.SetParent(car.transform);
            key.transform.localPosition = new Vector3(0.047f, 2.56f, 2.36f);
            key.transform.localEulerAngles = new Vector3(-90, 0, 0);

            isKeyCollected = true;
            keyAudioSource.Play();
        }
    }

    private void MoveCar()
    {
        if (!isCarTargetActive || !gas) return;

        car.transform.localEulerAngles += new Vector3(0, turn * SPEED_TURN, 0);

        float rotY = car.transform.localEulerAngles.y * Mathf.Deg2Rad;
        float deltaX = Mathf.Sin(rotY);
        float deltaZ = Mathf.Cos(rotY);

        car.transform.localPosition += new Vector3(deltaX * SPEED_GAS, 0, deltaZ * SPEED_GAS);
    }

    public void OnClickEnter()
    {
        gas = true;
        carAudioSource.Play();
    }

    public void OnClickExit()
    {
        gas = false;
        carAudioSource.Stop();
    }

    private void UpdateTurn()
    {
        if (!isControllerTargetActive) return;

        // our ImageTarget is rotated -90
        int wheelAngle = (int) transform.eulerAngles.y - 270;
        // since the camera points "downwards" towards the ImageTargets,
        // what we assume would be a rotation in y is actually in its z.
        int camAngle = (int) cam.transform.eulerAngles.z;
        // real-world rotation of tangible results in rotation of either
        // its ImageTarget or the camera, depending on the order of recognition.
        // to make this work, just use both.
        int netAngle = wheelAngle + camAngle;
        int netAngleNormalized = NormalizeAngle(netAngle);

        turn = Mathf.Clamp(netAngleNormalized, -45, 45);
    }

    private int NormalizeAngle(int angle)
    {
        while (angle >= 180) angle -= 360;
        while (angle < -180) angle += 360;

        return angle;
    }

    void Log(string msg)
    {
        debugText.text = msg;
    }
}
