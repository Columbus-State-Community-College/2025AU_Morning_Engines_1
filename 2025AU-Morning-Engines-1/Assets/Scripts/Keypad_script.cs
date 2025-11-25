using System;
using TMPro;
using UnityEngine;

/* Description:
    Depends on:
        -VendingMachine_script for sending events for which snack should be dropped

    -Takes input from the keypad UI.
    -This script will get the input from the UI and create a string to show visually what number is being inputted.
    -This script will also send out a message with the inputted string to VendingMachine_script so it can search for it.
 */
public class Keypad_script : MonoBehaviour
{
    public string inputString = "";
    [SerializeField] private TMPro.TMP_Text displayText;
    
    public static event Action<Keypad_script> OnEnterInput;

    void Start()
    {

    }
    void Update()
    {

    }

    public void ConcatenateInputString(string input)
    {
        if (inputString.Length >= 2)
        {
            Debug.LogWarning("Input string is too long, press reset to retry.");
            return;
        }
        else if ((input == "A" || input == "B" || input == "C") && (inputString.Length == 1))
        {
            Debug.LogWarning("Only one letter can be entered, and only in the first position.");
            return;
        }
        else if ((input == "1" || input == "2" || input == "3" || input == "4") && (inputString.Length == 0))
        {
            Debug.LogWarning("Only one number can be entered, and only in the last position.");
            return;
        }

        inputString += input;
        UpdateDisplay();
    }
    public void ResetInputString()
    {
        inputString = "";
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayText.text = inputString;
    }

    public void EnterInput()
    {
        OnEnterInput?.Invoke(this); // Sends an event to VendingMachine_script to try the input, can also be used for SFX for entering a combination
        ResetInputString();
    }
}
