using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;
    public RectTransform sugarRushMeter;
    
    [Header("Game Settings")]
    public int maxMoves = 20;
    public float sugarRushDuration = 10f;
    
    private int _score;
    private int _movesLeft;
    private float _sugarCharge;
    private bool _isSugarRushActive;

    // [Rest of the script from earlier...]
}
