using System.Collections.Generic;
using token;

namespace game {

    [System.Serializable]
    public struct GameData {
        public List<TokenData> tokenDatas;
        public bool isRedTurn;
        // в джсоне эта штука конечно будет иметь много смысла: "gameState": 1 - стоит поменять местами типы в GameState и приятного дебагинга после загрузки старого джсона
        public GameState gameState;
    }

    public enum GameState {
        Pause,
        Running,
        RedWin,
        BlueWin,
        Draw
    }
}
