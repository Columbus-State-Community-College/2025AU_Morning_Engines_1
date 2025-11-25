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

    // change to an int value. 
    // 0 is that it won't be 
    // 1 is that it needs to be bought twice because it's a row back
    // 2 is it will get stuck
    // then we could use this variable for the visual part maybe
    // also maybe add a thing were Ints 3 and over

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
        
        if (snackStatus < 2)
        {
            snackStatus += 1; // snackStatus cannot equal zero from now on
        }
        else
        {
            Debug.Log("That snack has already been bought!");
        }


        if (snackStatus == maxStatus)
        {
            Debug.Log("Snack should be falling");
            StartCoroutine(DispenseSnack());
        }
        else
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
