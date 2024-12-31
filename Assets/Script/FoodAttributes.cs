[System.Serializable]
public class FoodAttributes
{
    public string objectName; // 物體名稱（作為鍵值）
    // public string description; // 描述
    public int nutritionalValue; // 營養值
    public string specialEffects; // 特殊效果
    public int usageCount; // 特殊效果使用次數
    public bool isHeavy;
    public float spawnProbability; // 刷新概率
}

