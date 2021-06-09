using UnityEngine;
using UnityEngine.UI;
using game;
using TMPro;

namespace ui {
    public class UIController : MonoBehaviour {
        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private Canvas mainCanvas;
        [SerializeField]
        private Canvas endCanvas;
        [SerializeField]
        private Canvas playCanvas;

        [SerializeField]
        private Slider slider;
        [SerializeField]
        private float time;

        [SerializeField]
        private TextMeshProUGUI endGameText;

        private const string BLUE_WIN = "Blue Win";
        private const string RED_WIN = "Red Win";
        private const string DRAW = "Draw";

        private void Update() {
            switch (gameController.gameState) {
                case GameState.Pause:
                    mainCanvas.enabled = true;
                    endCanvas.enabled = false;
                    playCanvas.enabled = false;
                    break;
                case GameState.Running:
                    mainCanvas.enabled = false;
                    endCanvas.enabled = false;
                    playCanvas.enabled = true;

                    if (gameController.isRedTurn) {
                        slider.value = Mathf.Lerp(slider.value, 0f, time * Time.deltaTime);
                    } else {
                        slider.value = Mathf.Lerp(slider.value, 1f, time * Time.deltaTime);
                    }

                    break;
                case GameState.RedWin:
                    endCanvas.enabled = true;
                    playCanvas.enabled = false;

                    endGameText.text = RED_WIN;
                    break;
                case GameState.BlueWin:
                    endCanvas.enabled = true;
                    playCanvas.enabled = false;

                    endGameText.text = BLUE_WIN;
                    break;
                case GameState.Draw:
                    endCanvas.enabled = true;
                    playCanvas.enabled = false;

                    endGameText.text = DRAW;
                    break;
                default:
                    break;
            }
        }
    }
}
