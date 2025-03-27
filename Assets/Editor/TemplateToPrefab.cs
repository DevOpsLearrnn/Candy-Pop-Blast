using UnityEditor;
using UnityEngine;

public class TemplateToPrefab : EditorWindow
{
    [MenuItem("Tools/Convert Templates to Prefabs")]
    static void Convert()
    {
        // Candies
        CreateCandyPrefab("Blue");
        CreateCandyPrefab("Red");
        CreateCandyPrefab("Green");
        
        // UI
        CreateButtonPrefab("Play");
        
        Debug.Log("Prefabs generated from templates!");
    }

    static void CreateCandyPrefab(string color)
    {
        GameObject candy = new GameObject($"Candy_{color}");
        candy.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"{color}_candy");
        candy.AddComponent<BoxCollider2D>().size = Vector2.one;
        candy.AddComponent<CandyData>().candyType = color;
        
        PrefabUtility.SaveAsPrefabAsset(candy, $"Assets/Prefabs/Candies/Candy_{color}.prefab");
        DestroyImmediate(candy);
    }

    static void CreateButtonPrefab(string buttonType)
    {
        GameObject btn = new GameObject($"Button_{buttonType}");
        btn.AddComponent<RectTransform>();
        btn.AddComponent<Image>().sprite = Resources.Load<Sprite>($"btn_{buttonType.ToLower()}");
        btn.AddComponent<Button>();
        
        PrefabUtility.SaveAsPrefabAsset(btn, $"Assets/Prefabs/UI/Button_{buttonType}.prefab");
        DestroyImmediate(btn);
    }
}
