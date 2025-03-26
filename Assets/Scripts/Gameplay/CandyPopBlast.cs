using UnityEngine;

public class CandyPopBlast : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;
    }

    void OnMatch()
    {
        audioManager?.PlayPop();
    }
}
