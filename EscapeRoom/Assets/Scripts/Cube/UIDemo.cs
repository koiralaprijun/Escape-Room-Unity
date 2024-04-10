using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using TMPro;

public class UIDemo : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField userName;

    public void UnlockButton()
    {
        // Check if the input matches "e=mc2"
        if (userName.text.ToLower() == "e=mc2")
        {
            output.text = "correct. Your Code is 734";
        }
        else
        {
            output.text = "incorrect. Try Again to unlock Code.";
        }
    }
}
