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
        
        if (levelNum == vendingMachine.GetLevels().Length - 1)
        {
            Debug.Log("LevelNum: " + levelNum + " / levels.length: " + (vendingMachine.GetLevels().Length - 1));
            Debug.Log("You Win!");
            winScreen.SetActive(true);
            return;
        }
        
        levelNum++;
        Debug.Log("LevelNum: " + levelNum + " / levels.length: " + (vendingMachine.GetLevels().Length - 1));
        vendingScript.SetSnacks(levelNum);                     // Swaps the level                   
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
