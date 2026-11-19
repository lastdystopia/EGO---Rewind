
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    private TMP_Text promptText;
    private Action interactionAction;

    private void Awake()
    {
        promptText = GetComponentInChildren<TMP_Text>(true);

        if (promptText == null)
        {
            Debug.LogError("Interaction: No TextMeshPro component found!");
        }
    }

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        // Game input manager
        bool isEntered = GameInputManager.Instance.InteractInput.WasCompletedThisFrame();
        if (interactionAction == null)
            return;

        if (isEntered)
        {
            Debug.Log("Interaction Detected");
            interactionAction.Invoke();
        }
    }

    public void Show(string message, Action action)
    {
        if (promptText == null)
            return;

        promptText.text = message;
        interactionAction = action;

        promptText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        interactionAction = null;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}
