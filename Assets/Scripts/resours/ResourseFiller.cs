using System.Collections.Generic;
using UnityEngine;
using game;
using token;

namespace resourse {
    // ResourceFiller - значит он заполняет ресурсы, верно?
    public class ResourseFiller : MonoBehaviour {
        public Resourses resourses;
        public GameController gameController;

        private void Awake() {

            Dictionary<TokenType, GameObject> redTokens = new Dictionary<TokenType, GameObject> {
                { TokenType.Small, resourses.redSmallToken },
                { TokenType.Medium, resourses.redMediumToken },
                { TokenType.Big, resourses.redBigToken },
            };

            Dictionary<TokenType, GameObject> blueTokens = new Dictionary<TokenType, GameObject> {
                { TokenType.Small, resourses.blueSmallToken },
                { TokenType.Medium, resourses.blueMediumToken },
                { TokenType.Big, resourses.blueBigToken },
            };

            // что-то не похоже на ресурсы
            gameController.UpdateResourses(redTokens, blueTokens, resourses.winConditions);
        }
    }
}
