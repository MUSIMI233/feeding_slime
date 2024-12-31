using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FoodSpawner : MonoBehaviour
{
    [Header("Table Settings")]
    public Transform tableParent; // 指向 table 的父物件
    private List<Collider> tableColliders; // 子組件的所有 Colliders
    
    [Header("Spawner Settings")]
    public int maxSpawnCount = 20; // 最大生成量
    public float spawnInterval = 5f; // 刷新間隔（秒）
    public Vector3 spawnArea = new Vector3(10, 0, 10); // 生成範圍

    [Header("Prefab Settings")]
    public string foodFolderPath = "prefabs"; // Prefab 文件夾路徑
    public Transform spawnParent; // Food 的父節點，用於管理子物件

    private Dictionary<string, GameObject> foodPrefabs; // 加載的 Food Prefabs

    void Start()
    {
        // 收集所有子物件的 Collider
        tableColliders = new List<Collider>();
        foreach (var child in tableParent.GetComponentsInChildren<Collider>())
        {
            tableColliders.Add(child);
        }
        
        // 初始化 Food Prefabs
        LoadFoodPrefabs();

        // 立即生成最大數量的食材
        SpawnInitialFoods();

        // 啟動定時檢查並補充的協程
        StartCoroutine(SpawnFoodPeriodically());
    }

    /// <summary>
    /// 加載 Assets/Resources/Foods 資料夾中的所有 Prefabs
    /// </summary>
    void LoadFoodPrefabs()
    {
        foodPrefabs = new Dictionary<string, GameObject>();

        foreach (var foodKey in FoodDatabase.foodDictionary.Keys)
        {
            string assetPath = $"{foodFolderPath}/{foodKey}";
            GameObject prefab = Resources.Load<GameObject>(assetPath);

            if (prefab != null)
            {
                foodPrefabs[foodKey] = prefab;
            }
            else
            {
                Debug.LogWarning($"Prefab not found for Food Key: {foodKey} at path: {assetPath}");
            }
        }
    }

    /// <summary>
    /// 初始生成最大數量的食材
    /// </summary>
    void SpawnInitialFoods()
    {
        int currentCount = spawnParent.childCount; // 父節點的當前子物件數量
        for (int i = currentCount; i < maxSpawnCount; i++)
        {
            SpawnRandomFood();
        }
    }

    /// <summary>
    /// 周期性檢查並補充食材
    /// </summary>
    IEnumerator SpawnFoodPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 獲取當前父節點的子物件數量
            int currentCount = spawnParent.childCount;

            // 如果數量少於最大生成量，補充食材
            if (currentCount < maxSpawnCount)
            {
                int foodsToSpawn = maxSpawnCount - currentCount;

                for (int i = 0; i < foodsToSpawn; i++)
                {
                    SpawnRandomFood();
                }
            }
        }
    }

    /// <summary>
    /// 隨機生成一個食材
    /// </summary>
    void SpawnRandomFood()
    {
        // 根據概率從 FoodDatabase 中選擇食材
        string selectedKey = GetRandomFoodKey();
        if (selectedKey == null || !foodPrefabs.ContainsKey(selectedKey)) return;

        GameObject prefab = foodPrefabs[selectedKey];

        // 獲取桌子範圍內的隨機生成位置
        Vector3 randomPosition = GetRandomSpawnPosition();

        // 生成食材
        GameObject food = Instantiate(prefab, randomPosition, Quaternion.identity, spawnParent);

        // 附加屬性
        // AttachFoodBehaviour(food, selectedKey);
    }

    /// <summary>
    /// 根據 FoodDatabase 的概率選擇一個食材
    /// </summary>
    /// <returns>選中的食材 Key</returns>
    string GetRandomFoodKey()
    {
        float totalProbability = 0;
        foreach (var food in FoodDatabase.foodDictionary.Values)
        {
            totalProbability += food.spawnProbability;
        }

        float randomValue = Random.Range(0, totalProbability);
        float cumulativeProbability = 0;

        foreach (var kvp in FoodDatabase.foodDictionary)
        {
            cumulativeProbability += kvp.Value.spawnProbability;
            if (randomValue <= cumulativeProbability)
            {
                return kvp.Key;
            }
        }

        return null;
    }

    /// <summary>
    /// 獲取桌子範圍內的隨機生成位置
    /// </summary>
    /// <returns>生成位置</returns>
    Vector3 GetRandomSpawnPosition()
    {
        const float margin = 6f; // 邊緣偏移量，確保不生成在邊緣
        for (int i = 0; i < 10; i++) // 嘗試 10 次
        {
            // 隨機選擇一個 Collider
            Collider selectedCollider = tableColliders[Random.Range(0, tableColliders.Count)];
            Bounds bounds = selectedCollider.bounds;

            // 考慮邊緣偏移量計算生成位置
            float x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
            float z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
            float y = bounds.max.y;

            Vector3 position = new Vector3(x, y, z);

            // 如果位置沒有被佔用，返回該位置
            if (!IsPositionOccupied(position))
            {
                return position;
            }
        }

        Debug.LogWarning("Could not find a free position to spawn food.");
        return Vector3.zero;
    }

    /// <summary>
    /// 檢查位置是否被佔用
    /// </summary>
    /// <param name="position">檢查的位置</param>
    /// <param name="radius">檢查半徑</param>
    /// <returns>是否被佔用</returns>
    bool IsPositionOccupied(Vector3 position, float radius = 0.5f)
    {
        return Physics.CheckSphere(position, radius, LayerMask.GetMask("Food"));
    }

    /// <summary>
    /// 附加 FoodBehaviour 並設置屬性
    /// </summary>
    /// <param name="food">生成的物件</param>
    /// <param name="key">Food Key</param>
    void AttachFoodBehaviour(GameObject food, string key)
    {
        FoodBehaviour behaviour = food.AddComponent<FoodBehaviour>();
        behaviour.SetAttributes(FoodDatabase.foodDictionary[key]);
    }
}