using System.IO;
using UnityEngine;
using vjp;

namespace game {
    // save this loader
    public class SaveLoader : MonoBehaviour {
        [SerializeField]
        private GameController gameController;

        private string newGamePath;
        private string saveGamePath;

        private const string newGameFile = "/new.json";
        private const string saveGameFile = "/save.json";

        private void Awake() {
            newGamePath = Application.streamingAssetsPath + newGameFile;
            saveGamePath = Application.persistentDataPath + saveGameFile;
        }

        public void LoadNewGame() {
            LoadGameData(newGamePath);
        }

        public void LoadSaveGame() {
            LoadGameData(saveGamePath);
        }

        private void LoadGameData(string filePath) {
            string jsonData = LoadTextFromFile(filePath);
            if (string.IsNullOrWhiteSpace(jsonData)) {
                Debug.LogError("No data in json file");
                return;
            }

            var optionGameData = DeserializeGameData(jsonData);

            if (optionGameData.IsNone()) {
                Debug.LogError("Wrong json data in loaded file");
                return;
            }

            gameController.LoadGame(optionGameData.Peel());
        }

        public void SaveGame() {
            var gameData = gameController.GetGameData();

            if (gameData.tokenDatas == null) {
                Debug.LogError("Wrong data to save");
                return;
            }

            string jsonData = SerializeGameData(gameData);

            if (string.IsNullOrWhiteSpace(jsonData)) {
                Debug.LogError("Incorrect serialization");
                return;
            }

            SaveTextToFile(saveGamePath, jsonData);
        }

        public string SerializeGameData(GameData gameData) {

            if (gameData.tokenDatas == null) {
                Debug.LogError("Wrong data to serialize");
                return null;
            }

            try {
                return JsonUtility.ToJson(gameData);
            } catch (System.Exception ex) {
                Debug.LogError($"Serialization Error - {ex.Message}");
                return null;
            }
        }

        public Option<GameData> DeserializeGameData(string json) {

            if (string.IsNullOrWhiteSpace(json)) {
                Debug.LogError("No data to deserialize");
                return Option<GameData>.None();
            }

            try {
                var gameData = JsonUtility.FromJson<GameData>(json);
                return Option<GameData>.Some(gameData);
            } catch (System.Exception ex) {
                Debug.LogError($"Deserialization Error - {ex.Message}");
                return Option<GameData>.None();
            }
        }

        private string LoadTextFromFile(string path) {
            using (StreamReader reader = new StreamReader(path)) {

                if (!File.Exists(path)) {
                    Debug.LogError("File does not exist");
                    return null;
                }

                try {
                    string json = reader.ReadToEnd();
                    return json;
                } catch (System.Exception ex) {
                    Debug.LogError(ex.Message);
                    return null;
                }
            }
        }

        private void SaveTextToFile(string path, string text) {
            using (StreamWriter streamWriter = new StreamWriter(path)) {
                try {
                    streamWriter.Write(text);
                } catch (System.Exception ex) {
                    Debug.LogError(ex.Message);
                    return;
                }
            }
        }
    }
}
