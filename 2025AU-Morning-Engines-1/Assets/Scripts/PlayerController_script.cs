using TMPro;
using UnityEngine;

/* Description:
    +Will take all input from the player that isn’t through any UI elements like the keypad
        -Pausing, scrolling, etc.
    +Will move the camera when scrolling with the mouse (if the camera will be that close to the vending machine)

*/
public class PlayerController_script : MonoBehaviour
{
    public static float playerMoney;
    [SerializeField] TextMeshProUGUI cashTextElement;

    private float scrollSpeed = 0.05f;
    private void Start()
    {
        DetermineStartingCash();
        this.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        cashTextElement.text = playerMoney.ToString();
        SnackController_script.OnSnackBought += ChangePlayerMoney;
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

    private void DetermineStartingCash()
    {
        switch (GameController_script.levelNum)
        {
            case 0:
                playerMoney = 12.23f;
                break;
            case 1:
                playerMoney = 15.34f;
                break;
        }
    }
    private void ChangePlayerMoney(SnackController_script snackController)
    {
        playerMoney -= snackController.snackCost;
        cashTextElement.text = playerMoney.ToString();
    }

    private void OnDisable()
    {
        SnackController_script.OnSnackBought -= ChangePlayerMoney;
    }
}
