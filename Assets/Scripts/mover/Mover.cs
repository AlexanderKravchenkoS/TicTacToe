using UnityEngine;
using vjp;
using token;
using game;

namespace mover {
    public class Mover : MonoBehaviour {
        [SerializeField]
        private GameController gameController;
        private Option<Token> selectedToken = Option<Token>.None();
        RaycastHit hit;

        private const float TOKEN_HEIGHT = 1f;
        private const float BOARD_OFFSET = 0.5f;

        private void Update() {
            if (Input.GetMouseButton(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {

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

                    var newPos = new Vector3(hit.point.x, TOKEN_HEIGHT, hit.point.z);
                    selectedToken.Peel().transform.position = newPos;
                }
            }

            if (selectedToken.IsNone()) {
                return;
            }

            if (Input.GetMouseButtonUp(0)) {
                int mouseX = (int)(hit.point.x + BOARD_OFFSET);
                int mouseY = (int)(hit.point.z + BOARD_OFFSET);

                gameController.ProcessTurn(selectedToken.Peel(), mouseX, mouseY);

                selectedToken = Option<Token>.None();
            }
        }
    }
}
