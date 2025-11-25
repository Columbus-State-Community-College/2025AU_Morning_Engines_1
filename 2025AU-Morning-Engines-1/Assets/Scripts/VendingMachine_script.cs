using TMPro;
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
    [SerializeField] private GameObject[] snacks; // Array with all of the snacks in it for this level
    [SerializeField] private Transform vendingUI;
    [SerializeField] private GameObject snackPrice;
    

    private int levelNumber = 0; // fix later

    private string input; // input from the keypad UI
    private string[] positionIDs = {
        "A1", "A2", "A3", "A4",
        "B1", "B2", "B3", "B4",
        "C1", "C2", "C3", "C4"};
    private void Start()
    {
        Keypad_script.OnEnterInput += GetInputFromKeypad;
        
        SetSnacks();
        //CheckSnacks();
    }

    public void SetSnacks() // Positions all of the snacks for the level, called by GameController
    {
        // for each snack, put them in their spot, and assign them their positionID (A1, A2, etc.)

        for (int i = 0; i < levels[levelNumber].transform.childCount; i++)
        {
            snacks[i] = levels[levelNumber].transform.GetChild(i).gameObject;

            try // The code here attempts to access each index of snacks[], so if there isn’t a snack at one of the indexes, then it will catch that and move to the next iteration
            {
                snacks[i].SetActive(true);
                snacks[i].GetComponent<SnackController_script>().snackPosID = positionIDs[i];
            }
            catch
            {
                Debug.Log("There was no snack at index: " + i);
                continue; // Move on to next iteration
            }

            SetSnackPrices(snacks[i]);
        }
        return;
    }
    private void CheckSnacks() // Makes sure all of the snacks are accounted for in their price and status
    {
        Debug.Log("----------- CheckSnacks() Start -----------");
        for (int i = 0; i < levels[levelNumber].transform.childCount; i++)
        {
            
            try { Debug.Log("Snack " + i + ": "); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "] is not defined");
            }

            try { Debug.Log("  Position: " + snacks[i].GetComponent<SnackController_script>().snackPosID); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackPos is not defined");
            }

            try { Debug.Log("  Price: " + snacks[i].GetComponent<SnackController_script>().snackCost); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackCost is not defined");
            }

            try { Debug.Log("  Status: " + snacks[i].GetComponent<SnackController_script>().snackStatus); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].snackStatus is not defined");
            }

            try { Debug.Log("  WillGetStuck: " + snacks[i].GetComponent<SnackController_script>().willGetStuck); }
            catch
            {
                Debug.LogWarning("snacks[" + i + "].willGetStuck is not defined");
            }
        }
        Debug.Log("----------- CheckSnacks() End -----------");
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

    private void SetSnackPrices(GameObject currentSnack)
    {
        Vector3 snackPriceOffset = new Vector3(0, 0.21f, 0.02f);
        GameObject currentPrice = Instantiate(snackPrice, currentSnack.transform.position - snackPriceOffset, Quaternion.identity, vendingUI);
        currentPrice.transform.GetComponent<TextMeshProUGUI>().text = currentSnack.transform.GetComponent<SnackController_script>().snackCost.ToString();
    }
    private void OnDisable()
    {
        Keypad_script.OnEnterInput -= GetInputFromKeypad;
    }
}