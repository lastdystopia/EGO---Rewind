using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Menu Variables
    private static string previousSceneName;

    [Header("Menu Options")]
    public MenuOption[] menuOptions;
    
    private  bool horizontalSelectionActive = false;

    [Header("Selection")]
    public int selectedIndex = 0;
    public int selectedHorizontalIndex = 0;

    [Header("Selection Highlight")]
    public RectTransform selectionHighlight;

    // End Menu Variables
    public void Start(){
        UpdateSelection(); // Positions outline around whichever button is initially selected
    }
    private void Update(){
        Vector2 input = GameInputManager.Instance.MoveInput.ReadValue<Vector2>();
        if (GameInputManager.Instance.MoveInput.WasPressedThisFrame())
        {
        int horizontal = -Convert.ToInt32(input.x);
        int vertical = -Convert.ToInt32(input.y);
         if (horizontal != 0)
        MoveHorizontal(horizontal);

        if (vertical != 0)
        MoveVertical(vertical);
        }
        bool selectInput = GameInputManager.Instance.InteractInput.WasCompletedThisFrame();
        // Activate Selected button
        if (selectInput)
            SelectButton();
        
    }
    /*
    Moves the selection up or down
    */
    private void MoveVertical(int direction){
        if (menuOptions == null || menuOptions.Length == 0)
        {
            return;
        }
        selectedIndex += direction;
        // Wrap around if we move past the first or last button
        if (selectedIndex < 0)
        {
            selectedIndex = menuOptions.Length-1;
        }
        else if (selectedIndex >= menuOptions.Length)
        {
            selectedIndex = 0;
        }
        horizontalSelectionActive = false;
        UpdateSelection();
    }
    private void MoveHorizontal(int direction)
    {
        if (menuOptions == null || menuOptions.Length ==0)
            return;
        Button[] horizontalButtons = menuOptions[selectedIndex].horizontalButtons;
        // If no horizontal buttons, return
        if (horizontalButtons == null || horizontalButtons.Length ==0)
            return;
        // Else, move between them
        horizontalSelectionActive =true;
        selectedHorizontalIndex += direction;
        if (selectedHorizontalIndex < 0)
        {
         selectedHorizontalIndex = horizontalButtons.Length-1;   
        }
        else if (selectedHorizontalIndex >= horizontalButtons.Length)
        {
            selectedHorizontalIndex = 0;
        }
        UpdateSelection();
    }
    public void UpdateSelection()
    {
        if (menuOptions.Length == 0 || menuOptions == null)
            return;
        Button selectedButton;
        if (horizontalSelectionActive)
        {
            Button[] horizontalButtons = menuOptions[selectedIndex].horizontalButtons;
            if (horizontalButtons == null || horizontalButtons.Length == 0)
                return;
            selectedButton = horizontalButtons[selectedHorizontalIndex];
        }
        else
        {
            selectedButton = menuOptions[selectedIndex].verticalButton;
        }
        if (selectedButton==null)
            return;
RectTransform buttonRect =
    (RectTransform)selectedButton.transform;

    selectionHighlight.position = buttonRect.position;        // move- selected highlight position will be the menu's selected index transform position
    }
    public void SelectButton()
    {
        if (menuOptions.Length == 0 || menuOptions ==null)
            return;
        // on click invoke - invokes a click without using mouse key
        Button selectedButton;
        if (horizontalSelectionActive)
        {
            // Get the horizontal buttons
            Button[] horizontalButtons = menuOptions[selectedIndex].horizontalButtons;
            if (horizontalButtons.Length == 0 || horizontalButtons == null)
                return;
            // selected button = the button selected in those
            selectedButton = horizontalButtons[selectedHorizontalIndex];
        }
        else
        {
            selectedButton = menuOptions[selectedIndex].verticalButton;
        }
        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
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
        // Remember previous scene
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
