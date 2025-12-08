using System;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;

/* Description:
    Depends on: 
        +GameController_script for starting
        -SnackController_script for creation of snacks
        -Keypad_script for selection of snacks
    This script will be mostly used to cycle through every snack object for varying reasons like:
        +Setting the positions of the snacks for each level
        -Checking status of snacks
        -Taking keypad input in order to check which snack should be selected
        -To call SnackController.TryDropSnack() on each snack when an input is inputted 
 */
public class VendingMachine_script : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;
    public GameObject[] snacks; // Array with all of the snacks in it for this level
    [SerializeField] private Transform vendingUI;
    [SerializeField] private GameObject snackPriceUI;
    [SerializeField] private GameObject snackLocationUI;
    public int width = 4; // Amount of snack positions in each row
    public static event Action<VendingMachine_script> OnGameWin;
    public static event Action<VendingMachine_script> OnGameLose;

    private string input; // input from the keypad UI
    private string[] positionIDs = {
        "A1", "A2", "A3", "A4",
        "B1", "B2", "B3", "B4",
        "C1", "C2", "C3", "C4"};
    private void Start()
    {
        Keypad_script.OnEnterInput += GetInputFromKeypad;
        SnackController_script.OnSnackBought += CheckSnacks;

        SetSnacks();
        // CheckSnacks();
    }

    public void SetSnacks(int levelNumber = 0) // Positions all of the snacks for the level, called by GameController and Start()
    {
        // for each snack, put them in their spot, and assign them their positionID (A1, A2, etc.)
        levels[levelNumber].SetActive(true);

        for (int i = 0; i < levels[levelNumber].transform.childCount; i++)
        {
            snacks[i] = levels[levelNumber].transform.GetChild(i).gameObject;

            try // The code here attempts to access each index of snacks[], so if there isn’t a snack at one of the indexes, then it will catch that and move to the next iteration
            {
                snacks[i].SetActive(true);
                snacks[i].GetComponent<SnackController_script>().snackPosID = positionIDs[i];
                snacks[i].GetComponent<SnackController_script>().snackIndex = i;
            }
            catch
            {
                Debug.LogWarning("There was no snack at index: " + i);
                continue; // Move on to next iteration
            }

            SetSnackPriceUIs(snacks[i]);
            SetSnackLocationUIs(snacks[i]);
        }
        return;
    }
    private void CheckSnacks(SnackController_script snackController = null) // Makes sure all of the snacks are accounted for in their price and status
    {
        float minimumPriceLeft = 0; // Minimum amount of money needed for the game to end
        int maxStatus = 0; // Used as a sum of the maximum possible status amount for the level before the level is beat
        int currentStatus = 0; // Actual status sum at the moment of checking
        Debug.Log("----------- CheckSnacks() Start -----------");
        for (int i = 0; i < levels[GameController_script.levelNum].transform.childCount; i++)
        {

            /*try { Debug.Log("- - Snack: " + snacks[i] + " - -"); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "] is not defined");
                Debug.Log("     Skipping to the next snack in snacks[]");
                // Put code here for handling empty spaces in the vending machine
                continue; // Move on to the next snack, skipping the rest of this iteration
            }

            try { Debug.Log("  Position: " + snacks[i].GetComponent<SnackController_script>().snackPosID); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackPos is not defined");
            }

            try 
            {
                Debug.Log("  Price: " + snacks[i].GetComponent<SnackController_script>().snackCost); 
            }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackCost is not defined");
            }*/

            try
            {
                // Debug.Log("Status: " + snacks[i].GetComponent<SnackController_script>().snackStatus);

                maxStatus += 1;
                currentStatus += snacks[i].GetComponent<SnackController_script>().snackStatus;
                if (snacks[i].GetComponent<SnackController_script>().snackStatus == 0) // if snack needs bought in order to fall
                {
                    minimumPriceLeft += snacks[i].GetComponent<SnackController_script>().snackCost;
                }
            }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackStatus is not defined");
            }

            try
            {
                // Debug.Log("  WillGetStuck: " + snacks[i].GetComponent<SnackController_script>().willGetStuck);

                if (snacks[i].GetComponent<SnackController_script>().willGetStuck > 0)
                {
                    maxStatus += 1; // if a snack can get stuck, the maximum status for it is larger
                }
            }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].willGetStuck is not defined");
            }
        }
        // Debug.Log("maxStatus:   " + maxStatus);
        // Debug.Log("currentStatus:    " + currentStatus);
        // Debug.Log("minimumPriceLeft: " + minimumPriceLeft);

        if (currentStatus >= maxStatus)
        {
            OnGameWin?.Invoke(this); // Used in GameController_script

            
            return;
        }
        else
        {
            // Debug.Log("currentStatus is less than maxStatus");

            Debug.Log("----------- CheckSnacks() End -----------");
        }
        if (PlayerController_script.playerMoney < minimumPriceLeft)
        {
            OnGameLose?.Invoke(this);
        }
    }

    private void GetInputFromKeypad(Keypad_script keypadScript)
    {
        Debug.Log("Input inputted: " + keypadScript.inputString);
        input = keypadScript.inputString;
        for (int i = 0; i < snacks.Length; i++) // for each snack
        {
            if (snacks[i] != null)
            {
                SnackController_script currentSnackScript = snacks[i].transform.GetComponent<SnackController_script>();
                if (currentSnackScript.snackPosID == input)
                {
                    currentSnackScript.TryDropSnack();
                }
            }
            else
            {
                return;
            }
        }
    }

    private void SetSnackPriceUIs(GameObject currentSnack) // Spawns in the price UI for the vending machine snacks
    {
        Vector3 snackPriceOffset = new Vector3(0, 0.21f, 0.02f); // Change this when the Vending machine model is put in!
        GameObject currentPrice = Instantiate(snackPriceUI, currentSnack.transform.position - snackPriceOffset, Quaternion.identity, vendingUI);
        currentPrice.transform.GetComponent<TextMeshProUGUI>().text = currentSnack.transform.GetComponent<SnackController_script>().snackCost.ToString();
    }

    private void SetSnackLocationUIs(GameObject currentSnack) // Spawns in the snack location UI for the vending machine snacks
    {
        Vector3 snackLocationUIOffset = new Vector3(0, 0.01f, 0.1f); // Change this when the Vending machine model is put in!
        GameObject currentLocationUI = Instantiate(snackLocationUI, currentSnack.transform.position - snackLocationUIOffset, Quaternion.identity, vendingUI);
        currentLocationUI.transform.GetComponent<TextMeshProUGUI>().text = currentSnack.transform.GetComponent<SnackController_script>().snackPosID.ToString();
    }
    private void OnDisable()
    {
        Keypad_script.OnEnterInput -= GetInputFromKeypad;
        SnackController_script.OnSnackBought -= CheckSnacks;
    }
}