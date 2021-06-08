using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vjp;

public class Manager : MonoBehaviour
{
    private Option<Token>[,] board;
    private List<Vector2Int[]> winConditions;

    private bool isRunning;
    private bool isRedTurn;

    private const int MIN_BOUND = 0;
    private const int MAX_BOUND = 2;

    private const float TOKEN_Y = 0.15f;

    private void Start() {
        board = new Option<Token>[3,3];
        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {
                board[i,j] = Option<Token>.None();
            }
        }

        isRunning = true;
        isRedTurn = true;

        winConditions = new List<Vector2Int[]> {
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2) },
            new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2) },
            new Vector2Int[] { new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2) },
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) },
            new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1) },
            new Vector2Int[] { new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2) },
            new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(2, 2) },
            new Vector2Int[] { new Vector2Int(2, 0), new Vector2Int(1, 1), new Vector2Int(0, 2) }
        };
    }

    public void MoveToken(int newX, int newY, Token selectedToken) {
        if (!isRunning) {
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
            Destroy(board[newX, newY].Peel().gameObject);
        }

        selectedToken.gameObject.transform.position = newPosition;
        selectedToken.data = newData;
        board[newX, newY] = Option<Token>.Some(selectedToken);

        if (!IsWin(board, isRedTurn)) {
            isRedTurn = !isRedTurn;
            return;
        }

        if (isRedTurn) {
            Debug.Log("Red Win");
        } else {
            Debug.Log("Blue Win");
        }

        isRunning = false;
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

    private bool IsWin(Option<Token>[,] board, bool isRedTurn) {
        foreach (Vector2Int[] array in winConditions) {

            int counter = 0;

            foreach (Vector2Int vector in array) {

                if (board[vector.x, vector.y].IsNone()) {
                    break;
                }

                if (board[vector.x, vector.y].Peel().data.isRed != isRedTurn) {
                    break;
                }

                counter++;
            }

            if (counter == array.Length) {
                return true;
            }
        }

        return false;
    }
}
