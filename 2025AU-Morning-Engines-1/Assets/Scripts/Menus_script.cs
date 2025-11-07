using UnityEngine;
using UnityEngine.EventSystems;

/* Description:
 * Used for storing methods that may be useful for the main menu
 */

public class Menus_script : MonoBehaviour
{
    public void exitgame()
    {
        Debug.Log("NOTE: Exit game only works during a build, not in the editor!");
        Application.Quit();
    }
}
