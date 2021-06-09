using System.IO;
using UnityEngine;

namespace game {
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
            if (!File.Exists(filePath)) {
                return;
            }

            using StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            var gameData = JsonUtility.FromJson<GameData>(json);

            gameController.LoadGame(gameData);
        }

        public void SaveGame() {
            var gameData = gameController.SaveGame();

            string json = JsonUtility.ToJson(gameData);

            using (StreamWriter streamWriter = new StreamWriter(saveGamePath)) {
                streamWriter.Write(json);
            }
        }
    }
}
