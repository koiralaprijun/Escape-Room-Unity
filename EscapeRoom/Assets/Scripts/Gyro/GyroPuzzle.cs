using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroPuzzle : MonoBehaviour
{
    public Transform spawn;
    public Transform finish;
    public Transform keyDestination;
    public GameObject key;
    public AudioClip audioBallRoll, audioBallFall, audioKeyReveal;
    public AudioSource boardAudioSource;
    public Text debugText;

    private bool isGameActive = false;
    private Rigidbody rb;
    private AudioSource ballAudioSource, keyAudioSource;
    private Vector3 lastPosition = Vector3.zero;
    private bool isFalling = false;
    private float timer = 0f;

    private const float WIN_DIST_THRESHOLD = 0.00005f;
    private const float FALLING_Y = 0.025f;
    private const float OOB_Y = -6f;
    private const float SPEED_MAX = 1f;
    private const float PITCH_MIN = 0.75f;
    private const float PITCH_MAX = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        key.SetActive(false);
        lastPosition = transform.localPosition;

        ballAudioSource = GetComponent<AudioSource>();
        ballAudioSource.clip = audioBallRoll;
        ballAudioSource.loop = true;

        boardAudioSource.clip = audioBallFall;

        keyAudioSource = key.GetComponent<AudioSource>();
        keyAudioSource.clip = audioKeyReveal;
    }

    public void OnTargetFound()
    {
    }

    public void OnTargetLost()
    {
        ResetGame();
    }

    void Update()
    {
        HandleMovement();
        HandlePosition();
    }

    public void OnStartButtonClick()
    {
        if (!isGameActive)
        {
            StartGame();
        }
        else
        {
            ResetGame();
        }
    }

    private void HandleMovement()
    {
        // rolling audio
        if (!isGameActive) return;

        timer += Time.deltaTime;

        if (timer > 0.1f)
        {
            float dist = (transform.localPosition - lastPosition).magnitude;
            float speed = dist / timer;
            float speedNormalized = Mathf.Clamp(speed / SPEED_MAX, 0f, 1f);

            ballAudioSource.volume = Mathf.Log(9 * speedNormalized + 1);
            ballAudioSource.pitch = (speedNormalized * (PITCH_MAX - PITCH_MIN)) + PITCH_MIN;

            debugText.text = speedNormalized + "";

            lastPosition = transform.localPosition;
            timer = 0f;
        }
    }

    private void HandlePosition()
    {
        bool isWithinGoal = (finish.position - transform.position).sqrMagnitude < WIN_DIST_THRESHOLD;

        // falling
        if (!isFalling && !isWithinGoal && transform.localPosition.y < FALLING_Y)
        {
            isFalling = true;

            // don't play rolling sound while falling
            ballAudioSource.Stop();
            boardAudioSource.Play();
        }

        // out of bounds
        if (transform.localPosition.y < OOB_Y)
        {
            Log("Out of bounds");
            ResetGame();
        }

        // win condition
        if (isWithinGoal && transform.position != finish.position)
        {
            transform.position = finish.position;
            WinGame();
        }
    }

    private IEnumerator MoveKey()
    {
        Vector3 startScale = key.transform.localScale;
        Vector3 endScale = startScale * 2.5f;
        Vector3 startPosition = key.transform.position;
        Vector3 endPosition = keyDestination.position;
        Quaternion startRotation = key.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(180, 0, 90);

        float moveDuration = 2f;
        float timer = 0f;

        while (timer < moveDuration)
        {
            // in case the imagetarget moves during anim
            endPosition = keyDestination.position;

            key.transform.localScale = Vector3.Lerp(startScale, endScale, timer / moveDuration);
            key.transform.position = Vector3.Lerp(startPosition, endPosition, timer / moveDuration);
            key.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, timer / moveDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        key.transform.localScale = endScale;
        key.transform.position = endPosition;
        key.transform.localRotation = endRotation;
    }

    private void StartGame()
    {
        isGameActive = true;
        rb.useGravity = true;
        ballAudioSource.Play();

        Log("");
    }

    private void ResetGame()
    {
        Handheld.Vibrate();

        isFalling = false;
        isGameActive = false;
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        ballAudioSource.Stop();
        transform.position = spawn.position;
    }

    private void WinGame()
    {
        Log("Win!");

        StartCoroutine(MoveKey());
        keyAudioSource.Play();
        rb.useGravity = false;
        rb.isKinematic = true; // disable physics
        key.SetActive(true);
    }

    void Log(string msg)
    {
        debugText.text = msg;
    }
}
