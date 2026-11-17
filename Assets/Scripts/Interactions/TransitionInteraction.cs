
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Interaction interaction;
    public string promptMessage = "Press SPACE to Enter";
    public string sceneToLoad = "Playground";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interaction.Show(promptMessage, EnterScene);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interaction.Hide();
        }
    }

    private void EnterScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}