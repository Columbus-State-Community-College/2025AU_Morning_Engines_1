using UnityEngine;


/* Description:
    -This script controls which level is active and interacts with the VendingMachine_script in order to load the levels
    -Spawns the player camera after the menu UI’s start button is pressed

*/
public class GameController_script : MonoBehaviour
{
    public int levelNum;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject vendingMachine;

    private Vector3 playerPos = new Vector3(0, 0, 0);
    private Vector3 vendingMachinePos = new Vector3(0.0f, 1.0f, 2.25f);

    void OnEnable()
    {
        GameObject currentPlayer = Instantiate(player, playerPos, Quaternion.identity);
        currentPlayer.GetComponent<PlayerController_script>().playerMoney = levelNum*10; // Placeholder value for cash amount for each level
        
        GameObject currentVendingMachine = Instantiate(vendingMachine, vendingMachinePos, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
