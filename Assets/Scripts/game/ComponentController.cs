using mover;
using UnityEngine;

namespace game {
    public class ComponentController : MonoBehaviour {
        [SerializeField]
        private GameController gameController;
        [SerializeField]
        private Mover mover;

        private void Update() {
            if (gameController.gameState == GameState.Running) {
                mover.enabled = true;
            } else {
                mover.enabled = false;
            }
        }
    }
}