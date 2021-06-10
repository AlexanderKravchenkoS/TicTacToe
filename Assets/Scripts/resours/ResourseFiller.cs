using System.Collections.Generic;
using UnityEngine;
using game;
using token;

namespace resourse {
    public class ResourseFiller : MonoBehaviour {
        public Resourses resourses;

        public GameObject redSmallToken;
        public GameObject redMediumToken;
        public GameObject redBigToken;
        public GameObject blueSmallToken;
        public GameObject blueMediumToken;
        public GameObject blueBigToken;

        private void Awake() {

            var redTokensPrefabs = new Dictionary<TokenSize, GameObject> {
                { TokenSize.Small, redSmallToken },
                { TokenSize.Medium, redMediumToken },
                { TokenSize.Big, redBigToken },
            };

            var blueTokensPrefabs = new Dictionary<TokenSize, GameObject> {
                { TokenSize.Small, blueSmallToken },
                { TokenSize.Medium, blueMediumToken },
                { TokenSize.Big, blueBigToken },
            };

            resourses.blueTokensPrefabs = blueTokensPrefabs;
            resourses.redTokensPrefabs = redTokensPrefabs;
        }
    }
}
