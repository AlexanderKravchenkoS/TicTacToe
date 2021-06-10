using System.Collections.Generic;
using token;

namespace game {

    [System.Serializable]
    public struct GameData {
        public List<TokenData> tokenDatas;
        public bool isRedTurn;
    }

    public enum GameState {
        Pause,
        Running,
        RedWin,
        BlueWin,
        Draw
    }
}
