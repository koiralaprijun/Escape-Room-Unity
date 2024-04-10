using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip audioOpen;
    public AudioClip audioClose;
    public bool isOpened = false;

    private AudioSource audioSource;
    private bool[] locksUnlocked = new bool[]{false, false, false, false};

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetLockStatus(int lockNum, bool unlocked)
    {
        locksUnlocked[lockNum] = unlocked;
        CheckVictory();
    }

    private void CheckVictory()
    {
        for (int i = 0; i < locksUnlocked.Length; i++)
        {
            if (!locksUnlocked[i])
            {
                // close if any are locked
                Close();
                return;
            }
        }

        // open only if all are unlocked
        Open();
    }

    private void Open()
    {
        if (isOpened) return;

        isOpened = true;
        audioSource.clip = audioOpen;
        audioSource.Play();
        StartCoroutine(Rotate(-1));
    }

    private void Close()
    {
        if (!isOpened) return;

        isOpened = false;
        audioSource.clip = audioClose;
        audioSource.Play();
        StartCoroutine(Rotate(1));
    }

    private IEnumerator Rotate(int direction)
    {
        for (int i = 0; i < 10; i++)
        {
            transform.Rotate(0, 9 * direction, 0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
