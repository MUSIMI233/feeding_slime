using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    public string objectName; // 用於匹配字典中的食材名稱
    private FoodAttributes attributes;

    void Start()
    {
        // 從靜態字典中加載屬性
        attributes = FoodDatabase.GetFoodAttributes(objectName);

        if (attributes != null)
        {
            Debug.Log($"Loaded attributes for {attributes.objectName}: {attributes.nutritionalValue}");
        }
        else
        {
            Debug.LogWarning($"Attributes for {objectName} not found.");
        }
    }

    public void SetAttributes(FoodAttributes foodAttributes)
    {
        attributes = foodAttributes;

        if (attributes != null)
        {
            // 初始化行為，例如顯示屬性或設置其他屬性
            Debug.Log(
                $"Food {attributes.objectName} initialized with nutritional value: {attributes.nutritionalValue}");
        }
        else
        {
            Debug.LogWarning("Attributes not found for this food.");
        }
    }

    public int GetNutritionalValue()
    {
        return attributes.nutritionalValue;
    }
}
