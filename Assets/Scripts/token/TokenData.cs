namespace token {
    [System.Serializable]
    public class TokenData {
        public float x;
        public float y;
        public bool isRed;
        public TokenType type;
        public bool isTurned;
    }

    public enum TokenType {
        Small,
        Medium,
        Big
    }
}