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
        SoundEffectManager_script.Instance.Play(SoundEffectManager_script.SoundType.UI); // plays sound effect
    }

    public void SelectButton(Button button)
    {
        button.Select();
        SoundEffectManager_script.Instance.Play(SoundEffectManager_script.SoundType.UI); // plays sound effect
    }

    public void exitgame()
    {
        Debug.Log("NOTE: Exit game only works during a build, not in the editor!");
        Application.Quit();
    }
}
