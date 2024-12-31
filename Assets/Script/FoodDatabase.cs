using System.Collections.Generic;

public static class FoodDatabase
{
    private static readonly float COMMON = 0.5f;
    private static readonly float RARE = 0.15f;
    private static readonly float UNCOMMON = 0.35f;
    // 靜態字典存儲所有食材屬性
    public static readonly Dictionary<string, FoodAttributes> foodDictionary = new Dictionary<string, FoodAttributes>
    {
        { "Ice Cube", new FoodAttributes
            {
                objectName = "Ice Cube",
                nutritionalValue = 2,
                specialEffects = "skin",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Grape", new FoodAttributes
            {
                objectName = "Grape",
                nutritionalValue = 4,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Berry", new FoodAttributes
            {
                objectName = "Berry",
                nutritionalValue = 4,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
 
        { "Oatmeal", new FoodAttributes
            {
                objectName = "Oatmeal",
                nutritionalValue = 4,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = UNCOMMON
            }
        },
        { "Spinach", new FoodAttributes
            {
                objectName = "Spinach",
                nutritionalValue = 4,
                specialEffects = "super strength:throwing 什么都能扔的动",
                usageCount = 30,
                isHeavy = false,
                spawnProbability = UNCOMMON
            }
        },
        { "Screw", new FoodAttributes
            {
                objectName = "Screw",
                nutritionalValue = 4,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = UNCOMMON
            }
        },
        { "Battery", new FoodAttributes
            {
                objectName = "Battery",
                nutritionalValue = 4,
                specialEffects = "触发slime特殊动画（Art）",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = UNCOMMON
            }
        },

        { "Spring", new FoodAttributes
            {
                objectName = "Spring",
                nutritionalValue = 6,
                specialEffects = "Hold",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = UNCOMMON
            }
        },
        
        { "Pill", new FoodAttributes
            {
                objectName = "Pill",
                nutritionalValue = 4,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = UNCOMMON
            }
        },
        { "Sugar Cube", new FoodAttributes
            {
                objectName = "Sugar Cube",
                nutritionalValue = 6,
                specialEffects = "Rush / Speed boost",
                usageCount = 30,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Peanut ", new FoodAttributes
            {
                objectName = "Peanut",
                nutritionalValue = 6,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Seafood Shell", new FoodAttributes
            {
                objectName = "Seafood Shell",
                nutritionalValue = 6,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Bread Crumbs", new FoodAttributes
            {
                objectName = "Bread Crumbs",
                nutritionalValue = 6,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Cheese Crumbs", new FoodAttributes
            {
                objectName = "Cheese Crumbs",
                nutritionalValue = 6,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = false,
                spawnProbability = COMMON
            }
        },
        { "Apple Core", new FoodAttributes
            {
                objectName = "Apple Core",
                nutritionalValue = 8,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = COMMON
            }
        },
        { "Strawberry", new FoodAttributes
            {
                objectName = "Strawberry",
                nutritionalValue = 8,
                specialEffects = "skin",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = COMMON
            }
        },
        { "Rubber Duck", new FoodAttributes
            {
                objectName = "Rubber Duck",
                nutritionalValue = 8,
                specialEffects = "Hold",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = UNCOMMON
            }
        },
        { "Egg", new FoodAttributes
            {
                objectName = "Egg",
                nutritionalValue = 15,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = UNCOMMON
            }
        },
        
        { "Flower Bud", new FoodAttributes
            {
                objectName = "Flower Bud",
                nutritionalValue = 20,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = UNCOMMON
            }
        },

        { "Watermelon", new FoodAttributes
            {
                objectName = "Watermelon",
                nutritionalValue = 40,
                specialEffects = "N",
                usageCount = 0,
                isHeavy = true,
                spawnProbability = RARE
            }
        }
    };

    /// <summary>
    /// 獲取食材屬性
    /// </summary>
    public static FoodAttributes GetFoodAttributes(string name)
    {
        if (foodDictionary.TryGetValue(name, out FoodAttributes attributes))
        {
            return attributes;
        }
        else
        {
            return null;
        }
    }
}