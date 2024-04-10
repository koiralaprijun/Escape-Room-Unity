using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockBehavior : MonoBehaviour
{
    public Text debugText;
    public GameObject key1;
    public GameObject cylinder;
    public Transform keyDestination1;
    public Material matCorrect;
    public AudioClip audioCorrect;
    public Door door;

    private AudioSource audioSource;
    private bool victory = false;

    private const int LOCK_NUM = 2;
    private const float DIST_THRESHOLD = 0.01f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioCorrect;
    }

    void Update()
    {
        if ((transform.position - key1.transform.position).sqrMagnitude <= DIST_THRESHOLD)
        {
            OnKeyRecognized();
        }
    }

    void OnKeyRecognized()
    {
        if (victory) return;

        key1.transform.parent = transform.parent;
        StartCoroutine(MoveKey());

        victory = true;
    }

    IEnumerator MoveKey()
    {
        Vector3 startPosition = key1.transform.position;
        Vector3 endPosition = keyDestination1.position;
        Quaternion startRotation = key1.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(-90, 90, 0);

        float moveDuration = 1f;
        float timer = 0f;

        while (timer < moveDuration)
        {
            // in case the imagetarget moves during anim
            endPosition = keyDestination1.position;

            key1.transform.position = Vector3.Lerp(startPosition, endPosition, timer / moveDuration);
            key1.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, timer / moveDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        key1.transform.position = endPosition;
        key1.transform.localRotation = endRotation;

        StartCoroutine(RotateKey());
        StartCoroutine(MoveCylinder());
        Unlock();
    }

    IEnumerator RotateKey()
    {
        Quaternion startRotation = key1.transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0);

        float moveDuration = 1f;
        float timer = 0f;

        while (timer < moveDuration)
        {
            key1.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, timer / moveDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        key1.transform.localRotation = endRotation;
    }

    IEnumerator MoveCylinder()
    {
        Vector3 startPosition = cylinder.transform.localPosition;
        Vector3 targetPosition = startPosition - Vector3.right * 1f;

        float moveDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            cylinder.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cylinder.transform.localPosition = targetPosition;
    }

    private void Unlock()
    {
        audioSource.Play();
        cylinder.GetComponent<Renderer>().material = matCorrect;
        door.SetLockStatus(LOCK_NUM, true);
    }
}
