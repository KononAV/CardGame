using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Basic gameMode;
    public Texture2D[] saveMaterial;

    public static SaveManager Instance { get; set; }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameMode = GameMode.GameModeSelector(0);


    }
}
