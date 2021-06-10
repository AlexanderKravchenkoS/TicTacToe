namespace token {
    [System.Serializable]
    public class TokenData {
        public float x;
        public float y;
        // почему не enum?
        public bool isRed;
        public TokenSize size;
        // в кого превратился?
        public bool isTurned;
    }

    public enum TokenSize {
        Small,
        Medium,
        Big
    }
}
