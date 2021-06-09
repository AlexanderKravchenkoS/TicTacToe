using System.Collections.Generic;
using UnityEngine;
using vjp;
using token;
using resourse;

namespace game {
    public class GameController : MonoBehaviour {
        [SerializeField]
        private GameObject playground;

        private Option<Token>[,] board = new Option<Token>[3, 3];
        private List<Token> tokens = new List<Token>();

        public bool isRedTurn;

        private const int MIN_BOUND = 0;
        private const int MAX_BOUND = 2;

        private const float TOKEN_Y = 0.15f;

        private Dictionary<TokenType, GameObject> redTokens;
        private Dictionary<TokenType, GameObject> blueTokens;

        private List<WinCondition> winConditions;

        public GameState gameState = GameState.Pause;

        public void MoveToken(int newX, int newY, Token selectedToken) {
            if (gameState != GameState.Running) {
                return;
            }

            Vector3 oldPosition = new Vector3(selectedToken.data.x, TOKEN_Y, selectedToken.data.y);
            Vector3 newPosition = new Vector3(newX, TOKEN_Y, newY);

            TokenData newData = new TokenData() {
                isRed = selectedToken.data.isRed,
                isTurned = true,
                type = selectedToken.data.type,
                x = newX,
                y = newY
            };

            if (!IsCorrectMove(newData, selectedToken.data.isTurned, board)) {
                selectedToken.gameObject.transform.position = oldPosition;
                return;
            }

            if (board[newX, newY].IsSome()) {
                tokens.Remove(board[newX, newY].Peel());
                Destroy(board[newX, newY].Peel().gameObject);
            }

            selectedToken.gameObject.transform.position = newPosition;
            selectedToken.data = newData;
            board[newX, newY] = Option<Token>.Some(selectedToken);

            gameState = UpdateState(tokens, board, isRedTurn);
            isRedTurn = !isRedTurn;
        }

        private bool IsCorrectMove(TokenData newData, bool isTurned, Option<Token>[,] board) {

            int x = (int)newData.x;
            int y = (int)newData.y;

            if (isTurned) {
                return false;
            }

            if (x < MIN_BOUND || x > MAX_BOUND || y < MIN_BOUND || y > MAX_BOUND) {
                return false;
            }

            if (isRedTurn != newData.isRed) {
                return false;
            }

            if (board[x, y].IsSome()) {

                if (board[x, y].Peel().data.isRed == newData.isRed) {
                    return false;
                }

                if (board[x, y].Peel().data.type >= newData.type) {
                    return false;
                }
            }

            return true;
        }

        private GameState UpdateState(List<Token> tokens, Option<Token>[,] board, bool isRedTurn) {
            if (IsWin(board, isRedTurn)) {
                if (isRedTurn) {
                    return GameState.RedWin;
                } else {
                    return GameState.BlueWin;
                }
            }

            if (IsDraw(tokens, board, isRedTurn)) {
                return GameState.Draw;
            }

            return GameState.Running;
        }

        private bool IsDraw(List<Token> tokens, Option<Token>[,] board, bool isRedTurn) {
            foreach (var token in tokens) {
                if (token.data.isRed != isRedTurn) {
                    continue;
                }

                if (token.data.isTurned) {
                    continue;
                }

                for (int i = 0; i < board.GetLength(0); i++) {
                    for (int j = 0; j < board.GetLength(1); j++) {
                        var data = new TokenData {
                            isRed = token.data.isRed,
                            isTurned = token.data.isTurned,
                            type = token.data.type,
                            x = i,
                            y = j
                        };
                        if (IsCorrectMove(data, false, board)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsWin(Option<Token>[,] board, bool isRedTurn) {
            foreach (WinCondition condition in winConditions) {

                int counter = 0;

                foreach (Vector2Int vector in condition.positions) {

                    if (board[vector.x, vector.y].IsNone()) {
                        break;
                    }

                    if (board[vector.x, vector.y].Peel().data.isRed != isRedTurn) {
                        break;
                    }

                    counter++;
                }

                if (counter == condition.positions.Length) {
                    return true;
                }
            }

            return false;
        }

        public GameData SaveGame() {
            List<TokenData> tokenDatasTemp = new List<TokenData>();
            foreach (var token in tokens) {
                tokenDatasTemp.Add(token.data);
            }

            var gameData = new GameData {
                gameState = gameState,
                isRedTurn = isRedTurn,
                tokenDatas = tokenDatasTemp
            };

            return gameData;
        }

        public void LoadGame(GameData gameData) {
            if (tokens.Count != 0) {
                for (int i = 0; i < tokens.Count; i++) {
                    Destroy(tokens[i].gameObject);
                }
            }
            tokens = new List<Token>();

            board = new Option<Token>[3, 3];
            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    board[i, j] = Option<Token>.None();
                }
            }

            isRedTurn = gameData.isRedTurn;
            gameState = gameData.gameState;

            foreach (var item in gameData.tokenDatas) {
                var x = item.x;
                var y = item.y;
                var isRed = item.isRed;
                var isTurned = item.isTurned;
                var type = item.type;

                var vector = new Vector3(x, TOKEN_Y, y);

                GameObject tokenModel;

                if (isRed) {
                    tokenModel = redTokens[type];
                } else {
                    tokenModel = blueTokens[type];
                }

                var rotation = tokenModel.transform.rotation;

                var token = Instantiate(tokenModel, vector, rotation, playground.transform);
                var tokenData = new TokenData {
                    type = type,
                    isRed = isRed,
                    isTurned = isTurned,
                    x = x,
                    y = y
                };
                token.GetComponent<Token>().data = tokenData;
                tokens.Add(token.GetComponent<Token>());
                if (x >= MIN_BOUND && x <= MAX_BOUND && y >= MIN_BOUND && y <= MAX_BOUND) {
                    board[(int)x, (int)y] = Option<Token>.Some(token.GetComponent<Token>());
                }
            }
        }

        public void UpdateResourses(
            Dictionary<TokenType, GameObject> redTokens,
            Dictionary<TokenType, GameObject> blueTokens,
            List<WinCondition> winConditions
            ) {

            this.redTokens = redTokens;
            this.blueTokens = blueTokens;
            this.winConditions = winConditions;
        }
    }
}