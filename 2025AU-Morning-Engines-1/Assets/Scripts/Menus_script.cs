using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Description:
 * Used for storing methods that may be useful for the main menu
 */

public class Menus_script : MonoBehaviour
{
    [SerializeField] private Button startButton;

    public void Start()
    {
        startButton.Select();
    }

    public void SelectButton(Button button)
    {
        button.Select();
    }

    public void exitgame()
    {
        Debug.Log("NOTE: Exit game only works during a build, not in the editor!");
        Application.Quit();
    }
}
