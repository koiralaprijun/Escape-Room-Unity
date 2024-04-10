using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleInfo : MonoBehaviour
{
    public Text title;
    public Text hint;
    public List<Transform> imageTargets;
    public GameObject gyroPuzzleUI;
    public GameObject racingPuzzleUI;
    public GameObject cubePuzzleUI;
    public Transform cam;
    public Door door;

    void Start()
    {
        gyroPuzzleUI.SetActive(false);
        racingPuzzleUI.SetActive(false);
        cubePuzzleUI.SetActive(false);
    }

    void Update()
    {
        DisplayActivePuzzleInfo();
    }

    private void DisplayActivePuzzleInfo()
    {
        // sort imagetargets by distance to camera
        imageTargets.Sort((a, b) =>
        {
            return (cam.position - a.position).sqrMagnitude
                .CompareTo((cam.position - b.position).sqrMagnitude);
        });

        if (door.isOpened)
        {
            title.text = "You win!";
            hint.text = "Thank you for playing our game!";
            return;
        }

        switch (imageTargets[0].name)
        {
            case "LockTarget":
                gyroPuzzleUI.SetActive(false);
                racingPuzzleUI.SetActive(false);
                cubePuzzleUI.SetActive(false);

                title.text = "Lock Puzzle";
                hint.text = "To unlock the door and escape, you need two keys and two codes. Solve puzzles to find them!";
                break;

            case "GyroTarget":
                gyroPuzzleUI.SetActive(true);
                racingPuzzleUI.SetActive(false);
                cubePuzzleUI.SetActive(false);

                title.text = "Gyro Puzzle";
                hint.text = "Tilt the thing to roll the ball. NOTE: To get this to work, you should keep another puzzle in the background.";
                break;

            case "WheelTarget":
            case "CarTarget":
                gyroPuzzleUI.SetActive(false);
                racingPuzzleUI.SetActive(true);
                cubePuzzleUI.SetActive(false);

                title.text = "Racing Puzzle";
                hint.text = "Control the remote car by turning the steering wheel and pressing gas. Collect the key and insert it into the lock!";
                break;

            case "CubeTarget":
                gyroPuzzleUI.SetActive(false);
                racingPuzzleUI.SetActive(false);
                cubePuzzleUI.SetActive(true);

                title.text = "Revealing Cube Puzzle";
                hint.text = "Figure out formula from the revolving cube, enter below and unlock the code.";
                break;

            case "BoxTarget":
            case "ButtonTarget":
            case "ChestTarget":
            case "KeyTarget":
                gyroPuzzleUI.SetActive(false);
                racingPuzzleUI.SetActive(false);
                cubePuzzleUI.SetActive(false);

                title.text = "Chest Puzzle";
                hint.text = "Find the invisible key to unlock the chest.";
                break;
        }
    }
}
