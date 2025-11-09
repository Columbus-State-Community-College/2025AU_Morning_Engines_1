using UnityEngine;

/* Description:
    Depends on: 
        +GameController_script for starting
        -SnackController_script for organization of snacks
        -Keypad_script for selection of snacks
    This script will be mostly used to cycle through every snack object for varying reasons like:
        -Setting the positions of the snacks for each level
        -Checking status of snacks
        -Taking keypad input in order to check which snack should be selected
        -To call SnackController.TryDropSnack() on each snack when a number input is inputted 
 */
public class VendingMachine_script : MonoBehaviour
{
    [SerializeField] private GameObject[] snacks; // Array with all of the snacks in it
    private int input; // input from the keypad UI
    private string[] positionIDs = {"A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4", "C1", "C2", "C3", "C4"};
    private Vector3[] positionVectors = { 
        new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(2, 1, 0), new Vector3(3, 1, 0), 
        new Vector3(0, 2, 0), new Vector3(1, 2, 0), new Vector3(2, 2, 0), new Vector3(3, 2, 0),
        new Vector3(0, 3, 0), new Vector3(1, 3, 0), new Vector3(2, 3, 0), new Vector3(3, 3, 0),
        new Vector3(0, 4, 0), new Vector3(1, 4, 0), new Vector3(2, 4, 0), new Vector3(3, 4, 0)}; // Possible positions for snacks


    private void Start()
    {
        // for each snack, put them in their spot, and assign them their positionID (A1, A2, etc.)
        for (int i = 0; i < 4 /*snacks.Length*/; i++)
        {

            GameObject indexedSnack = Instantiate(snacks[i], positionVectors[i] + transform.position, Quaternion.identity, transform);
        }

    }

    private void Update()
    {
        
    }

    public void SetSnacks() // Positions all of the snacks for the level, called by GameController
    {
        
        return;
    }
    private void CheckSnacks() // Makes sure all of the snacks are accounted for in their price and status
    {

        return;
    }

}
