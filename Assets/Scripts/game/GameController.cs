using System.Collections.Generic;
using UnityEngine;
using vjp;
using token;
using resourse;

namespace game {
    public class GameController : MonoBehaviour {
        [SerializeField]
        private GameObject playground;
        [SerializeField]
        private Resourses resourses;

        private Option<Token>[,] board;

        public bool isRedTurn;

        private const int MIN_BOUND = 0;
        private const int MAX_BOUND = 2;

        private const float TOKEN_Y = 0.15f;

        public GameState gameState = GameState.Pause;

        public void ProcessTurn(Token token, int newX, int newY) {
            MoveToken(token, newX, newY, out bool cancelTurn);

            if (cancelTurn) {
                return;
            }

            if (board[newX, newY].IsSome()) {
                Destroy(board[newX, newY].Peel().gameObject);
            }

            board[newX, newY] = Option<Token>.Some(token);

            gameState = CalculateState(FindObjectsOfType<Token>(), board, isRedTurn);

            isRedTurn = !isRedTurn;
        }

        private void MoveToken(Token token, int newX, int newY, out bool cancelTurn) {
            cancelTurn = false;
            if (!IsCorrectMove(token.data, newX, newY)) {
                var position = new Vector3(token.data.x, TOKEN_Y, token.data.y);
                token.gameObject.transform.position = position;
                cancelTurn = true;
                return;
            }

            token.gameObject.transform.position = new Vector3(newX, TOKEN_Y, newY);
            TokenData newData = new TokenData() {
                isRed = token.data.isRed,
                isTurned = true,
                size = token.data.size,
                x = newX,
                y = newY
            };
            token.data = newData;
        }

        private bool IsCorrectMove(TokenData tokenData, int newX, int newY) {

            if (tokenData.isTurned) {
                return false;
            }

            if (newX < MIN_BOUND || newX > MAX_BOUND || newY < MIN_BOUND || newY > MAX_BOUND) {
                return false;
            }

            if (isRedTurn != tokenData.isRed) {
                return false;
            }

            if (board[newX, newY].IsSome()) {

                if (board[newX, newY].Peel().data.isRed == tokenData.isRed) {
                    return false;
                }

                if (board[newX, newY].Peel().data.size >= tokenData.size) {
                    return false;
                }
            }

            return true;
        }

        private GameState CalculateState(Token[] tokens, Option<Token>[,] board, bool isRedTurn) {
            if (IsWin(board)) {
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

        private bool IsWin(Option<Token>[,] board) {
            int boardSize = board.GetLength(0);
            int counter;
            TokenData firstData;
            TokenData secondData;


            for (int i = 0; i < boardSize; i++) {
                counter = 1;
                for (int j = 0; j < boardSize - 1; j++) {
                    if (board[i, j].IsNone() || board[i, j + 1].IsNone()) {
                        break;
                    }
                    firstData = board[i, j].Peel().data;
                    secondData = board[i, j + 1].Peel().data;
                    if (firstData.isRed != secondData.isRed) {
                        break;
                    }
                    counter++;
                }
                if (counter == boardSize) {
                    return true;
                }
            }

            for (int i = 0; i < boardSize; i++) {
                counter = 1;
                for (int j = 0; j < boardSize - 1; j++) {
                    if (board[j, i].IsNone() || board[j + 1, i].IsNone()) {
                        break;
                    }
                    firstData = board[j, i].Peel().data;
                    secondData = board[j + 1, i].Peel().data;
                    if (firstData.isRed != secondData.isRed) {
                        break;
                    }
                    counter++;
                }
                if (counter == boardSize) {
                    return true;
                }
            }

            counter = 1;
            for (int i = 0; i < boardSize - 1; i++) {
                if (board[i, i].IsNone() || board[i + 1, i + 1].IsNone()) {
                    break;
                }
                firstData = board[i, i].Peel().data;
                secondData = board[i + 1, i + 1].Peel().data;
                if (firstData.isRed != secondData.isRed) {
                    break;
                }
                counter++;
            }
            if (counter == boardSize) {
                return true;
            }

            counter = 1;
            for (int i = 0; i < boardSize - 1; i++) {
                if (board[boardSize - (i + 1), i].IsNone() ||
                    board[boardSize - (i + 2), i + 1].IsNone()) {
                    break;
                }
                firstData = board[boardSize - (i + 1), i].Peel().data;
                secondData = board[boardSize - (i + 2), i + 1].Peel().data;
                if (firstData.isRed != secondData.isRed) {
                    break;
                }
                counter++;
            }
            if (counter == boardSize) {
                return true;
            }

            return false;
        }

        private bool IsDraw(Token[] tokens, Option<Token>[,] board, bool isRedTurn) {
            foreach (var token in tokens) {

                if (token.data.isRed != isRedTurn) {
                    continue;
                }

                if (token.data.isTurned) {
                    continue;
                }

                for (int i = 0; i < board.GetLength(0); i++) {
                    for (int j = 0; j < board.GetLength(1); j++) {
                        if (IsCorrectMove(token.data, i, j)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public GameData GetGameData() {
            var tokens = FindObjectsOfType<Token>();
            List<TokenData> tokenDatasTemp = new List<TokenData>();
            foreach (var token in tokens) {
                tokenDatasTemp.Add(token.data);
            }

            var gameData = new GameData {
                isRedTurn = isRedTurn,
                tokenDatas = tokenDatasTemp
            };

            return gameData;
        }

        public void LoadGame(GameData gameData) {
            ResetGame();
            InitializeGame(gameData);
        }

        public void ResetGame() {
            var tokens = FindObjectsOfType<Token>();
            if (tokens.Length != 0) {
                for (int i = 0; i < tokens.Length; i++) {
                    Destroy(tokens[i].gameObject);
                }
            }

            board = new Option<Token>[3, 3];
        }

        public void InitializeGame(GameData gameData) {
            isRedTurn = gameData.isRedTurn;
            gameState = GameState.Running;

            foreach (var item in gameData.tokenDatas) {
                var x = item.x;
                var y = item.y;
                var isRed = item.isRed;
                var isTurned = item.isTurned;
                var type = item.size;

                var vector = new Vector3(x, TOKEN_Y, y);

                GameObject tokenModel;

                if (isRed) {
                    tokenModel = resourses.redTokensPrefabs[type];
                } else {
                    tokenModel = resourses.blueTokensPrefabs[type];
                }

                var rotation = tokenModel.transform.rotation;

                var token = Instantiate(tokenModel, vector, rotation, playground.transform);
                var tokenData = new TokenData {
                    size = type,
                    isRed = isRed,
                    isTurned = isTurned,
                    x = x,
                    y = y
                };
                token.GetComponent<Token>().data = tokenData;
                if (x >= MIN_BOUND && x <= MAX_BOUND && y >= MIN_BOUND && y <= MAX_BOUND) {
                    board[(int)x, (int)y] = Option<Token>.Some(token.GetComponent<Token>());
                }
            }
        }
    }
}
