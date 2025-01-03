using System.IO;
using TMPro;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;
    public TMP_InputField inputField;
    public TextMeshProUGUI currentPlayerText;

    public string playerName;

    public string BestPlayerName { get; set; } = "No Player";
    public int BestScore { get; set; } = 0;

    private string saveFilePath;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        saveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        LoadPlayerData();
    }

    void Start()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnEndEdit);
        }
        UpdateCurrentPlayerText();
    }

    private void OnEndEdit(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            playerName = newName;
            SavePlayerData();
            UpdateCurrentPlayerText();
        }
    }

    private void UpdateCurrentPlayerText()
    {
        if (currentPlayerText != null)
        {
            currentPlayerText.text = $"Current Player: {playerName}";
        }
    }

    // Make the SavePlayerData public to allow access from other classes
    public void SavePlayerData()
    {
        PlayerData data = new PlayerData { playerName = this.playerName, bestPlayerName = BestPlayerName, bestScore = BestScore };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            playerName = data.playerName;
            BestPlayerName = data.bestPlayerName;
            BestScore = data.bestScore;
        }
        else
        {
            playerName = "Anonymous"; // Default name
        }
    }

    // Implement GetName to return the player's current name
    public string GetName()
    {
        return playerName;
    }

    [System.Serializable]
    private class PlayerData
    {
        public string playerName;
        public string bestPlayerName;
        public int bestScore;
    }
}
