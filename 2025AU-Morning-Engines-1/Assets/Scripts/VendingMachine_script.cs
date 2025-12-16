using System;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private bool logStuff; // set to false for avoiding misc info in the console
    [SerializeField] private GameObject[] levels;
    public GameObject[] snacks; // Array with all of the snacks in it for this level
    [SerializeField] private Transform vendingUI;
    [SerializeField] private GameObject snackPriceUI;
    [SerializeField] private GameObject snackLocationUI;
    public int width = 4; // Amount of snack positions in each row
    public static event Action<VendingMachine_script> OnGameWin;
    public static event Action<VendingMachine_script> OnGameLose;
    public static bool playerRanOutOfCash = false;

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
    }

    public void SetSnacks(int levelNumber = 0) // Positions all of the snacks for the level, called by GameController and Start()
    {
        // for each snack, put them in their spot, and assign them their positionID (A1, A2, etc.)
        
        levels[levelNumber].SetActive(true);
        if (levelNumber != 0)
        {
            //RemovePreviousSnackUIs(levelNumber); 
        }

        for (int i = 0; i < levels[levelNumber].transform.childCount; i++)
        {
            snacks[i] = levels[levelNumber].transform.GetChild(i).gameObject;
            SnackController_script currentSnack = snacks[i].GetComponent<SnackController_script>();

            try // The code here attempts to access each index of snacks[], so if there isn’t a snack at one of the indexes, then it will catch that and move to the next iteration
            {
                snacks[i].SetActive(true);
                currentSnack.snackPosID = positionIDs[i];
                currentSnack.snackIndex = i;
            }
            catch
            {
                //Debug.LogWarning("There was no snackIndex at index: " + i);
                continue; // Move on to next iteration
            }


            SetSnackLocationUIs(snacks[i], levelNumber);
            SetSnackPriceUIs(snacks[i], levelNumber);
               
        }
    }
    private void CheckSnacks(SnackController_script snackController = null) // Makes sure all of the snacks are accounted for in their price and status
    {
        float minimumPriceLeft = 0.0f; // Minimum amount of money needed for the game to end
        float highestPriceLeft = 0.0f; // Highest price currently in the vending machine
        int maxStatus = 0; // Used as a sum of the maximum possible status amount for the level before the level is beat
        int currentStatus = 0; // Actual status sum at the moment of checking

        if (logStuff == true) { Debug.Log("----------- CheckSnacks() Start -----------"); }
        for (int i = 0; i < levels[GameController_script.levelNum].transform.childCount; i++)
        {
            SnackController_script currentSnack = snacks[i].GetComponent<SnackController_script>();

            if (logStuff == true) // Only log this misc info if you want the console filled with this stuff
            {
                try
                {
                    Debug.Log("- - Snack: " + snacks[i] + " - -");
                }
                catch
                {
                    Debug.LogWarning("snacks[" + i + "] is not defined");
                    Debug.Log("     Skipping to the next snack in snacks[]");
                    // Put code here for handling empty spaces in the vending machine
                    continue; // Move on to the next snack, skipping the rest of this iteration
                }

                try
                {
                    Debug.Log("  Position: " + currentSnack.snackPosID);
                }
                catch
                {
                    Debug.LogWarning("snacks[" + i + "].snackPos is not defined");
                    continue;
                }

                try
                {
                    Debug.Log("  Price: " + currentSnack.snackCost);
                }
                catch
                {
                    Debug.LogWarning("snacks[" + i + "].snackCost is not defined");
                    continue;
                }
            }

            try
            {
                if (logStuff == true) { Debug.Log("Status: " + currentSnack.snackStatus); }

                maxStatus += 1;
                currentStatus += currentSnack.snackStatus;
                if (currentSnack.snackStatus == 0) // if snack needs bought in order to fall
                {
                    minimumPriceLeft += currentSnack.snackCost;
                }
            }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackStatus is not defined");
            }

            try
            {
                // Debug.Log("  WillGetStuck: " + snacks[i].GetComponent<SnackController_script>().willGetStuck);

                if (currentSnack.willGetStuck > 0)
                {
                    maxStatus += 1; // if a snack can get stuck, the maximum status for it is larger
                }
            }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].willGetStuck is not defined");
            }

            if ((currentSnack.snackCost > highestPriceLeft) && (currentSnack.snackStatus < 2)) // Finds the highest price of snack that is still in the machine
            {
                if ((currentSnack.snackStatus == 1) && (currentSnack.willGetStuck == 1))
                {
                    highestPriceLeft = currentSnack.snackCost;
                    if (logStuff == true) { Debug.Log("New Highest Price: " + currentSnack.snackCost); }
                }
                else if ((currentSnack.snackStatus == 0) && (currentSnack.willGetStuck == 0))
                {
                    highestPriceLeft = currentSnack.snackCost;
                    if (logStuff == true) { Debug.Log("New Highest Price: " + currentSnack.snackCost); }
                }
            }
        }
        // Debug.Log("maxStatus:   " + maxStatus);
        // Debug.Log("currentStatus:    " + currentStatus);
        // Debug.Log("minimumPriceLeft: " + minimumPriceLeft);
        // Debug.Log("highestPriceLeft: " + highestPriceLeft);
        // Debug.Log("playerMoney:      " + PlayerController_script.playerMoney);

        if (currentStatus >= maxStatus)
        {
            OnGameWin?.Invoke(this); // Used in GameController_script
            return;
        }
        else
        {
            // Debug.Log("currentStatus is less than maxStatus");
        }

        if ((playerRanOutOfCash == true) && (PlayerController_script.playerMoney < highestPriceLeft)) // Lose Conditions
        {
            OnGameLose?.Invoke(this);
        }
        if (logStuff == true) { Debug.Log("----------- CheckSnacks() End -----------"); }
    }

    private void GetInputFromKeypad(Keypad_script keypadScript)
    {
        Debug.Log("Input inputted: " + keypadScript.inputString);

        input = keypadScript.inputString;
        for (int i = 0; i < snacks.Length - 1; i++) // for each snack
        {
            if (snacks[i] != null)
            {
                SnackController_script currentSnackScript = snacks[i].transform.GetComponent<SnackController_script>();
                try
                {
                    if (currentSnackScript.snackPosID == input)
                    {
                        currentSnackScript.TryDropSnack();
                    }
                }
                catch { Debug.LogWarning("snacks[" + i + "] has no posID"); }
            }
            else
            {
                //Debug.Log("snacks[" + i + "] == null, out of snacks[" + (snacks.Length - 1) + "]");
                return;
            }
        }
    }

    private void SetSnackPriceUIs(GameObject currentSnack, int lvlNum = -1) // Spawns in the price UI for the vending machine snacks
    {
        //Debug.Log("SettingPrices(" + lvlNum + ")");
        Vector3 snackPriceOffset = new Vector3(0, 0.21f, 0.02f); // Change this when the Vending machine model is put in!
        GameObject currentPrice = Instantiate(snackPriceUI, currentSnack.transform.position - snackPriceOffset, Quaternion.identity, vendingUI);
        currentPrice.transform.GetComponent<TextMeshProUGUI>().text = currentSnack.transform.GetComponent<SnackController_script>().snackCost.ToString();
    }

    private void SetSnackLocationUIs(GameObject currentSnack, int lvlNum = -1) // Spawns in the snack location UI for the vending machine snacks
    {
        //Debug.Log("SettingLocations(" + lvlNum + ")");
        Vector3 snackLocationUIOffset = new Vector3(0, 0.01f, 0.15f); // Change this when the Vending machine model is put in!
        GameObject currentLocationUI = Instantiate(snackLocationUI, currentSnack.transform.position - snackLocationUIOffset, Quaternion.identity, vendingUI);
        currentLocationUI.transform.GetComponent<TextMeshProUGUI>().text = currentSnack.transform.GetComponent<SnackController_script>().snackPosID.ToString();
    }

    private void RemovePreviousSnackUIs(int lvlNum = -1)
    {
        //Debug.Log("RemovingSnackUIs(" + lvlNum + ")");
        for (int i = 0; i < vendingUI.childCount; i++)
        {
            //Debug.Log("Removing:" + vendingUI.GetChild(i).name);

            vendingUI.GetChild(i).gameObject.SetActive(false);
        }
    }

    public GameObject[] GetLevels()
    {
        return levels;
    }

    private void OnDisable()
    {
        Keypad_script.OnEnterInput -= GetInputFromKeypad;
        SnackController_script.OnSnackBought -= CheckSnacks;
    }
}