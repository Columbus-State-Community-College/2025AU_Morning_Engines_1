using UnityEngine;


/* Description:
    -This script controls which level is active and interacts with the VendingMachine_script in order to load the levels
    +Enables the player camera after the menu UI’s start button is pressed

*/
public class GameController_script : MonoBehaviour
{
    public static int levelNum;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] levels;

    private Vector3 playerPos = new Vector3(0, 0, 0);
    private Vector3 vendingMachinePos = new Vector3(0.0f, 1.0f, 2.25f);

    void OnEnable()
    {
        player.SetActive(true);

        levels[levelNum].SetActive(true);
    }

    void Update()
    {
        
    }
}
