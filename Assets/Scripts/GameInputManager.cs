using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameInputManager : MonoBehaviour
{
    // allows other scripts to access the manager without needing separate Inspector reference
    public static GameInputManager Instance {get; private set;} // singleton

    [UnitHeaderInspectable("Input Actions")]
    [SerializeField] private InputActionAsset gameControls;

    private InputActionMap gameplayMap;
    //player movement
    public InputAction MoveInput{get; private set;}
    //camera movement
    public InputAction CameraInput{get; private set;}
    // general interaction
    public InputAction InteractInput{get; private set;}
    //menu controls
    public InputAction GoBackInput{get; private set;}
    public InputAction OpenMenuInput{get; private set;}

    // Awake
    private void Awake()
    {
        Debug.Log("Game input manager initialized");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameplayMap = gameControls.FindActionMap("Gameplay");

        MoveInput = gameplayMap.FindAction("Move");
        CameraInput = gameplayMap.FindAction("Camera");
        InteractInput = gameplayMap.FindAction("Interact");
        GoBackInput = gameplayMap.FindAction("GoBack");
        OpenMenuInput = gameplayMap.FindAction("OpenMenu");
    }
private void OnEnable()
    {
        if (gameplayMap != null)
        {
            gameplayMap.Enable();
        }
    }
    private void OnDisable()
    {
        if (gameplayMap != null)
        {
            gameplayMap.Disable();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
