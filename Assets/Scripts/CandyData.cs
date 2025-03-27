public enum CandyType
{
    Normal,
    Striped,
    Wrapped,
    ColorBomb
}

public class CandyData : MonoBehaviour
{
    public CandyType type;
    public int colorIndex;
}
