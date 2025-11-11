using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/* Description:
    Depends on: 
        +GameController_script for starting
        -SnackController_script for creation of snacks
        -Keypad_script for selection of snacks
    This script will be mostly used to cycle through every snack object for varying reasons like:
        -Setting the positions of the snacks for each level
        -Checking status of snacks
        -Taking keypad input in order to check which snack should be selected
        -To call SnackController.TryDropSnack() on each snack when a number input is inputted 
 */
public class VendingMachine_script : MonoBehaviour
{
    [SerializeField] private GameObject[] snacks; // Array with all of the snacks in it for this level
    private int input; // input from the keypad UI
    private string[] positionIDs = {
        "A1", "A2", "A3", "A4",
        "B1", "B2", "B3", "B4",
        "C1", "C2", "C3", "C4"};
    private Vector3[] positionVectors = {
        new Vector3(1, 1, 0), new Vector3(2, 1, 0), new Vector3(3, 1, 0), new Vector3(4, 1, 0),
        new Vector3(1, 2, 0), new Vector3(2, 2, 0), new Vector3(3, 2, 0), new Vector3(4, 2, 0),
        new Vector3(1, 3, 0), new Vector3(2, 3, 0), new Vector3(3, 3, 0), new Vector3(4, 3, 0),
        new Vector3(1, 4, 0), new Vector3(2, 4, 0), new Vector3(3, 4, 0), new Vector3(4, 4, 0)}; // Possible positions for snacks, will need to be scaled

    public Vector3 snackPosScale = new Vector3(0.23f, 0.43f, 1); // Scale for the position vectors
    private void Start()
    {
        SetSnacks();
        CheckSnacks();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
                { SetSnacks(); }
        
    }

    public void SetSnacks() // Positions all of the snacks for the level, called by GameController
    {
        // for each snack, put them in their spot, and assign them their positionID (A1, A2, etc.)

        Vector3 snackPosOffset = new Vector3(
            transform.position.x - 0.83f, 
            transform.position.y - 0.83f, 
            transform.position.z - 0.55f); // Position offset for the position vectors after scaling
        for (int i = 0; i < snacks.Length; i++)
        {
            try // The code here attempts to access each index of snacks[], so if there isn’t a snack at one of the indexes, then it will catch that and move to the next iteration
            {
                Vector3 scaledPosition = new Vector3(positionVectors[i].x * snackPosScale.x, 
                                                     positionVectors[i].y * snackPosScale.y,
                                                     positionVectors[i].z * snackPosScale.z);
                Vector3 finalizedPosition = scaledPosition + snackPosOffset;
                GameObject indexedSnack = Instantiate(snacks[i], finalizedPosition, Quaternion.Euler(0, 270, 90)); // Creates a snack with the finalized position and the vending machine as a parent
                indexedSnack.GetComponent<SnackController_script>().positionID = positionIDs[i]; // Sets the positionID of the snack
            }
            catch
            {
                Debug.Log("There was no snack at index: " + i);
                continue; // Move on to next iteration
            } 
        }
        return;
    }
    private void CheckSnacks() // Makes sure all of the snacks are accounted for in their price and status
    {
        for (int i = 0; i < snacks.Length; i++)
        {
            try { Debug.Log("Snack " + i + ": "); }
            catch
            {
                Debug.LogError("snacks[" + i + "] is not defined");
                //throw;
            }

            try { Debug.Log("  Position: " + snacks[i].GetComponent<SnackController_script>().snackPos); }
            catch
            {
                Debug.LogError("snacks[" + i + "].snackPos is not defined");
                //throw;
            }

            try { Debug.Log("  Price: " + snacks[i].GetComponent<SnackController_script>().snackCost); }
            catch
            {
                Debug.LogError("snacks[" + i + "].snackCost is not defined");
                //throw;
            }

            try { Debug.Log("  Status: " +snacks[i].GetComponent<SnackController_script>().snackStatus); }
            catch
            {
                Debug.LogError("snacks[" + i + "].snackStatus is not defined");
                //throw;
            }

            try { Debug.Log("  WillGetStuck: " +snacks[i].GetComponent<SnackController_script>().willGetStuck); }
            catch
            {
                Debug.LogError("snacks[" + i + "].willGetStuck is not defined");
                //throw;
            }
        }
    }
}

// change the initializations of snack variables to be null at first in snackcontroller
