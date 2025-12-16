using System;
using System.Collections;
using TMPro;
using UnityEngine;

/* Description:
    +Will take all input from the player that isn�t through any UI elements like the keypad
        +Pausing, scrolling, etc.
    +Will move the camera when scrolling with the mouse (if the camera will be that close to the vending machine)

*/
public class PlayerController_script : MonoBehaviour
{
    public static float playerMoney;
    [SerializeField] TextMeshProUGUI cashTextElement;
    public float[] levelStartingMoney;
    private float originalY;
    private float lastSnackCost = 0.00f;
    private float scrollSpeed = 0.05f;
    public Animator billAnimator;
    public Animator coinAnimator;

    private void Start()
    {
        if (billAnimator == null) //makes sure the animator is logged
        {
            billAnimator = GameObject.Find("OneDollarModel").GetComponent<Animator>();
            if (billAnimator == null)
            {
                Debug.LogError("Animator not found.", gameObject);
            }
        }
        if (coinAnimator == null) //makes sure the animator is logged
        {
            coinAnimator = GameObject.Find("CoinModel").GetComponent<Animator>();
            if (coinAnimator == null)
            {
                Debug.LogError("Animator not found.", gameObject);
            }
        }
        DetermineStartingCash();
        this.gameObject.SetActive(false);
        originalY = transform.position.y;
        
    }
    void OnEnable()
    {
        cashTextElement.text = playerMoney.ToString();
        SnackController_script.OnSnackBought += ChangePlayerMoney;
    }

    void Update()
    {
        if ((Input.mouseScrollDelta.y > 0) && (transform.position.y < originalY + 0.7f))
        {
            transform.Translate(Vector3.up * scrollSpeed);
        }
        else if ((Input.mouseScrollDelta.y < 0) && (transform.position.y > originalY - 0.5f))
        {
            transform.Translate(Vector3.down * scrollSpeed);
        }
    }

    public void DetermineStartingCash()
    {
        playerMoney = levelStartingMoney[GameController_script.levelNum];
        playerMoney += lastSnackCost;
    }

    private void ChangePlayerMoney(SnackController_script snackController)
    {
        lastSnackCost = snackController.snackCost;
        playerMoney -= snackController.snackCost;
        Pay_Animation(lastSnackCost);
        
        cashTextElement.text = playerMoney.ToString("F2");
    }

    private void OnDisable()
    {
        SnackController_script.OnSnackBought -= ChangePlayerMoney;
    }

    public void Pay_Animation(float cost)
    {
        float CurrentCost = cost; 
        if (CurrentCost >= 0.99f)
        {
            coinAnimator.SetTrigger("CoinInserted");
            
            
        }
        else
        {
            coinAnimator.SetTrigger("CoinInserted");
            billAnimator.SetTrigger("BillInserted");
        }
       
    }
}
