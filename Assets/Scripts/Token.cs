using UnityEngine;

public class Token : MonoBehaviour {
    public TokenData data;
}

[System.Serializable]
public struct TokenData {
    public float x;
    public float y;
    public bool isRed;
    public TokenType type;
    public bool isTurned;
}

public enum TokenType {
    Small,
    Maduim,
    Big
}