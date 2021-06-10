using System.IO;
using UnityEngine;

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
            // ну допустим вот здесь файл существует
            if (!File.Exists(filePath)) {
                // и никакого сообщения
                return;
            }

            // а вот здесь ОС переключила процесс игры на процесс программы, которая удалила файл по пути filePath

            // упс, эксепшн?
            using StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            // кидает ли JSONUtility эксепшны? что если json невалидный?
            var gameData = JsonUtility.FromJson<GameData>(json);

            gameController.LoadGame(gameData);
        }

        public void SaveGame() {
            var gameData = gameController.SaveGame();

            string json = JsonUtility.ToJson(gameData);

            // а тут видимо saveGamePath даже не нужно пытаться проверить
            using (StreamWriter streamWriter = new StreamWriter(saveGamePath)) {
                streamWriter.Write(json);
            }
        }
    }
}
