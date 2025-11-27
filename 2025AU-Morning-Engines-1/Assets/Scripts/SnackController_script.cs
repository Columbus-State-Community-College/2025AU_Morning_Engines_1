using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

/* Description:
    -Goes on each type of snack prefab
    -
*/

public class SnackController_script : MonoBehaviour
{
    public string snackPosID;   // Number string for snack position (not to be confused with an int!)
    public float snackCost;   // Cost of the snack
    public int snackStatus;   // Whether or not the snack has been emptied from the machine or not (0 is the starting value, 1 is when the snack is stuck, and 2 is when the snack has been dropped)
    public int willGetStuck; // Determines if the snack will get stuck or not
    private Animator animator;
    private Vector3 dispenseLocation;
    public static event Action<SnackController_script> OnSnackBought;

    public enum SnackType 
    {
        Soda_Can,
        Soda_Bottle,
        Water_Bottle,
        Chip_Bag,
        Candy_Bar,
        Candy_Packet
    }

    public string ThisSnack; // this variable is assigned in the inspector


    private void Start()
    {
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>(); // Gets the animator from the visual child object of each snack
        if (animator == null) // This is used for cans since they have a more complicated structure for their animations
        {
            animator = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>();
        }
        else if (animator == null)
        {
            Debug.LogWarning("Child object Animator component was not found on " + this.gameObject.name);
        }
        dispenseLocation = GameObject.Find("DispenseSpot").transform.position;

        
    }

    private void Update()
    {
        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.normalizedTime >= 1.0f && animatorState.IsName("SnackFall"))
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            animator.Play("Idle");
        }
    }

    public void TryDropSnack() // Used to try to drop a snack, it does this by adding to the snackStatus
    {
        int maxStatus;
        Debug.Log("dropping snack: " + snackPosID);
        if (willGetStuck == 2)
        {
            maxStatus = 2;
        }
        else
        {
            maxStatus = 1;
        }

        if (snackStatus >= 2) // snack spot empty
        {
            Debug.Log("That snack has already been bought!");
        }
        else if (PlayerController_script.playerMoney > snackCost) // if less than 2, then the snack can still be bought and the price has to be checked here
        {
            snackStatus += 1; // snackStatus cannot equal zero from now on
            OnSnackBought?.Invoke(this); // Sends an event to PlayerController_script to edit the player money, can also be used for SFX
        }
        else
        {
            Debug.Log("Not enough cash!");
        }


        if (snackStatus == maxStatus)
        {
            Debug.Log("Snack should be falling");
            StartCoroutine(DispenseSnack());
        }
        else if (snackStatus == 1)
        {
            Debug.Log("Snack should be stuck");
            //animator.Play("StuckAnimation");
        }
    }

    private IEnumerator DispenseSnack()
    {
        animator.Play("SnackFall");

        yield return new WaitForSeconds(2.0f); // Wait for the snack to fall
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;

        yield return new WaitForSeconds(1.0f); // Wait to dispense snack
        BoxCollider snackCollider;
        if (transform.GetChild(0).gameObject.GetComponent<BoxCollider>() != null)
        {
            snackCollider = transform.GetChild(0).gameObject.GetComponent<BoxCollider>();
        }
        else if (transform.GetChild(0).GetChild(0).gameObject.GetComponent<BoxCollider>()) // This is used for cans since they have a more complicated structure for their animations
        {
            snackCollider = transform.GetChild(0).GetChild(0).gameObject.GetComponent<BoxCollider>();
        }
        else 
        { 
            Debug.LogWarning("Child object BoxCollider component was not found on " + this.gameObject.name);
            snackCollider = null;
        }

        snackCollider.enabled = false;
        transform.position = dispenseLocation;
        transform.rotation = Quaternion.Euler(-180, 0, 0);
        animator.Play("SnackDispense");

        yield return new WaitForSeconds(1.0f); // Wait for the dispense animation
        snackCollider.enabled = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return null;
    }
}
