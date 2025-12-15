using UnityEngine;
using UnityEngine.SceneManagement;


/* Description:
    +This script controls which level is active and interacts with the VendingMachine_script in order to load the levels
    +Enables the player camera after the menu UI’s start button is pressed

*/
public class GameController_script : MonoBehaviour
{
    public static int levelNum;
    [SerializeField] private GameObject player;
    [SerializeField] private VendingMachine_script vendingScript;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private void OnEnable()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        player.SetActive(true);

        
        VendingMachine_script.OnGameLose += GameLoss;
        VendingMachine_script.OnGameWin += GameWin;
    }

    void GameLoss(VendingMachine_script vendingMachine)
    {
        Debug.Log("You Lose!");
        loseScreen.SetActive(true);
    }

    void GameWin(VendingMachine_script vendingMachine)
    {
        Debug.Log("You Win Round " + levelNum + "!");
        winScreen.SetActive(true);
        levelNum++;
        vendingScript.SetSnacks(levelNum);                                        // Will be used later for level swappage
        player.GetComponent<PlayerController_script>().DetermineStartingCash();
    }

    private void OnDisable()
    {
        VendingMachine_script.OnGameLose -= GameLoss;
        VendingMachine_script.OnGameWin -= GameWin;
    }

    public void Restart()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
