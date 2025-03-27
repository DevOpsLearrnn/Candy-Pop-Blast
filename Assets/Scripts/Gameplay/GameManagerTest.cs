using NUnit.Framework;
using UnityEngine;
using TMPro;

public class GameManagerTest
{
    private GameManager _gameManager;
    private TextMeshProUGUI _mockScoreText;
    private TextMeshProUGUI _mockMovesText;
    private RectTransform _mockSugarRushMeter;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and attach the GameManager script
        GameObject gameObject = new GameObject();
        _gameManager = gameObject.AddComponent<GameManager>();

        // Mock UI elements
        _mockScoreText = new GameObject().AddComponent<TextMeshProUGUI>();
        _mockMovesText = new GameObject().AddComponent<TextMeshProUGUI>();
        _mockSugarRushMeter = new GameObject().AddComponent<RectTransform>();

        // Assign mock UI elements to the GameManager
        _gameManager.scoreText = _mockScoreText;
        _gameManager.movesText = _mockMovesText;
        _gameManager.sugarRushMeter = _mockSugarRushMeter;

        // Initialize private fields
        typeof(GameManager).GetField("_score", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_gameManager, 100);
        typeof(GameManager).GetField("_movesLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_gameManager, 10);
        typeof(GameManager).GetField("_sugarCharge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_gameManager, 0.5f);
    }

    [Test]
    public void UpdateUI_SetsScoreTextCorrectly()
    {
        _gameManager.UpdateUI();
        Assert.AreEqual("SCORE: 100", _mockScoreText.text);
    }

    [Test]
    public void UpdateUI_SetsMovesTextCorrectly()
    {
        _gameManager.UpdateUI();
        Assert.AreEqual("MOVES: 10", _mockMovesText.text);
    }

    [Test]
    public void UpdateUI_SetsSugarRushMeterScaleCorrectly()
    {
        _gameManager.UpdateUI();
        Assert.AreEqual(new Vector3(0.5f, 1, 1), _mockSugarRushMeter.localScale);
    }
}