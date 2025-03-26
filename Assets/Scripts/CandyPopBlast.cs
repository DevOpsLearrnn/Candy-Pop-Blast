// Add this variable
private AudioManager audioManager;

// In Start():
audioManager = FindObjectOfType<AudioManager>();

// When matches occur:
if (audioManager != null) 
    audioManager.PlayPop();
