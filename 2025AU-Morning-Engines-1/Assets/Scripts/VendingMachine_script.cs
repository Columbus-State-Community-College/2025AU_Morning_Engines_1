using UnityEngine;

/* Description:
    Depends on: 
        -GameController_script for starting
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
    private GameObject[] snacks; // Array with all of the snacks in it
    private int input; // input from the keypad UI

    void Start()
    {
        
    }

    void Update()
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
