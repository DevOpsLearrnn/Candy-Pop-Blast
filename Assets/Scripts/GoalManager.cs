public class GoalManager : MonoBehaviour {
    [System.Serializable]
    public struct Goal {
        public CandyType candyType;
        public int targetCount;
        [HideInInspector] public int currentCount;
    }

    // [Rest of the script...]
}
