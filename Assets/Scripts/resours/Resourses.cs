using System.Collections.Generic;
using UnityEngine;

namespace resourse {
    public class Resourses : MonoBehaviour {
        // зачем тут всюду [SerializeField]?
        [SerializeField]
        public GameObject redSmallToken;
        [SerializeField]
        public GameObject redMediumToken;
        [SerializeField]
        public GameObject redBigToken;
        [SerializeField]
        public GameObject blueSmallToken;
        [SerializeField]
        public GameObject blueMediumToken;
        [SerializeField]
        public GameObject blueBigToken;

        [SerializeField]
        public List<WinCondition> winConditions;
    }
}
