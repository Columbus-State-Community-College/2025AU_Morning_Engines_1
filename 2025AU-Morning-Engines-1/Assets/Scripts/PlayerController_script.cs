using UnityEngine;

/* Description:
    +Will take all input from the player that isn’t through any UI elements like the keypad
        -Pausing, scrolling, etc.
    +Will move the camera when scrolling with the mouse (if the camera will be that close to the vending machine)

*/
public class PlayerController_script : MonoBehaviour
{
    public float playerMoney;

    private float scrollSpeed = 0.05f;
    void Start()
    {
        
    }

    void Update()
    {
        if ((Input.mouseScrollDelta.y > 0) && (transform.position.y < 0.7f))
        {
            transform.Translate(Vector3.up * scrollSpeed);
        }
        else if ((Input.mouseScrollDelta.y < 0) && (transform.position.y > -0.5f))
        {
            transform.Translate(Vector3.down * scrollSpeed);
        }


    }
}
