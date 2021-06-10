namespace token {
    [System.Serializable]
    public class TokenData {
        public float x;
        public float y;
        // почему не enum?
        public bool isRed;
        public TokenType type;
        // в кого превратился?
        public bool isTurned;
    }

    public enum TokenType {
        Small,
        Medium,
        Big
    }
}
