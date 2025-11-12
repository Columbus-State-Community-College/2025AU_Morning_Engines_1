using UnityEngine;

/* Description:
    -Goes on each type of snack prefab
    -
*/

public class SnackController_script : MonoBehaviour
{
    public string snackPos = null;   // Number string for snack position (not to be confused with an int!)
    public float snackCost;   // Cost of the snack
    public int snackStatus;   // Whether or not the snack has been emptied from the machine or not (0 is the starting value, 1 is when the snack is stuck, and 2 is when the snack has been dropped)
    public bool willGetStuck; // Determines if the snack will get stuck or not
    // From Pin, change to an int value. 
    // 0 is that it won't be 
    // 1 is that it needs to be bought twice because it's a row back
    // 2 is it will get stuck
    // then we could use this variable for the visual part maybe
    // also maybe add a thing were Ints 3 and over
    public string positionID = null;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TryDropSnack() // Used to try to drop a snack, it does this by adding to the snackStatus by either 1 or 2 based on if willGetStuck == true
    {

        return;
    }
}
