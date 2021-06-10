using UnityEngine;
using vjp;
using token;
using game;

namespace mover {
    public class Mover : MonoBehaviour {
        [SerializeField]
        private GameController gameController;
        private Option<Token> selectedToken = Option<Token>.None();

        // может TOKEN_HEIGHT? NEW_Y - название привязанное к одному контексту, не позволяет понять что дают эти данные без логики
        private const float NEW_Y = 1f;
        // смещение чего? куда? кого? Данные должны пониматься без кода.
        private const float OFFSET = 0.5f;

        private void Update() {

            // зависимость скрипта передвижения от состояния игры - неверное решение, кто-то другой должен просто выключать этот скрипт, если нужно отключить эту логику
            if (gameController.gameState != GameState.Running) {
                return;
            }

            // какой смысл рейкастить пока не нажата кнопка мыши?
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {

                if (Input.GetMouseButtonDown(0)) {
                    var token = hit.transform.GetComponent<Token>();

                    if (token == null) {
                        return;
                    }

                    selectedToken = Option<Token>.Some(token);
                }

                if (selectedToken.IsNone()) {
                    return;
                }

                var newPos = new Vector3(hit.point.x, NEW_Y, hit.point.z);
                selectedToken.Peel().transform.position = newPos;

                if (Input.GetMouseButtonUp(0)) {
                    int mouseX = (int)(hit.point.x + OFFSET);
                    int mouseY = (int)(hit.point.z + OFFSET);

                    gameController.MoveToken(mouseX, mouseY, selectedToken.Peel());

                    selectedToken = Option<Token>.None();
                }
            }
        }
    }
}
