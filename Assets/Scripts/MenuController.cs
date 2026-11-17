using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    // Menu Variables
    [Header("Menu Buttons")]
    public Button[] menuButtons;
    [Header("Selection")]
    public int selectedIndex = 0;
    [Header("Selection Highlight")]
    public RectTransform selectionHighlight;

    public void Start(){
        UpdateSelection(); // Positions outline around whichever button is initially selected
    }
    private void Update(){
        if (Keyboard.current == null)
            return;
        // Move selection around
        if (Keyboard.current.wKey.wasPressedThisFrame){
            MoveSelection(-1);
         }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            MoveSelection(1);
        }
        // Activate Selected button
        if (Keyboard.current.enterKey.wasPressedThisFrame ||
        Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SelectButton();
        }
    }
    /*
    Moves the selection up or down
    */
    private void MoveSelection(int direction){
        selectedIndex += direction;
        // Wrap around if we move past the first or last button
        if (selectedIndex < 0)
        {
            selectedIndex = menuButtons.Length-1;
        }
        else if (selectedIndex >= menuButtons.Length)
        {
            selectedIndex = 0;
        }
        UpdateSelection();
    }
    public void UpdateSelection()
    {
        if (menuButtons.Length == 0 || selectionHighlight == null)
        {
            return;
        }
        // move- selected highlight position will be the menu's selected index transform position
        selectionHighlight.position=menuButtons[selectedIndex].transform.position;
    }
    public void SelectButton()
    {
        if (menuButtons.Length == 0)
            return;
        // on click invoke - invokes a click without using mouse key

        menuButtons[selectedIndex].onClick.Invoke();
    }
    public void QuitGame()
    {
        # if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        # else
        Application.Quit();
        # endif
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
