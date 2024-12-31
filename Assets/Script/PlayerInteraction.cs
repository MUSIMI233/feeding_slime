using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform grabPoint; // 玩家抓取點（放置物體的位置）
    public Transform targetPoint; // 投擲的目標點
    public float grabRange = 2f; // 抓取範圍
    public float basePlaceDistance = 1.5f; // 基礎放置距離
    public Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 0.5f); // 縮小尺寸
    private GameObject grabbedObject; // 當前抓取的物體
    private Vector3 originalScale; // 物體原始尺寸
    private bool isGrabbing = false;


    void Update()
    {
        // 抓取或放下
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isGrabbing)
            {
                TryGrab();
            }
            else
            {
                PlaceObject(); // 放置物體
            }
        }
        if (Input.GetKeyDown(KeyCode.T) && isGrabbing)
        {
            PlaceObject(targetPoint);
        }
    }

    /// <summary>
    /// 嘗試抓取範圍內的物體
    /// </summary>
    void TryGrab()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange); // 檢測範圍內的碰撞體
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Food")) // 確保物體帶有 "Food" 標籤
            {
                grabbedObject = hitCollider.gameObject;

                // 保存原尺寸並縮小物體
                originalScale = grabbedObject.transform.localScale;
                grabbedObject.transform.localScale = shrinkScale;

                // 將物體移動到抓取點並禁用物理效果
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.parent = grabPoint; // 設置為抓取點的子物件

                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // 禁用動力學
                }

                isGrabbing = true;
                return;
            }
        }
    }

    /// <summary>
    /// 放置當前抓取的物體
    /// </summary>
    void PlaceObject(Transform target = null)
    {
        if (grabbedObject)
        {
            // 恢復原尺寸並解除父子關係
            grabbedObject.transform.localScale = originalScale;
            grabbedObject.transform.parent = null;

            // 判斷是否有目標位置
            if (target != null)
            {
                // 如果有目標，直接將物品放置到目標位置
                grabbedObject.transform.position = AlignWithGround(target.position);
            }
            else
            {
                // 如果沒有目標，按照原放置邏輯計算放置位置
                Vector3 placePosition = CalculatePlacePosition();

                // 如果物體沒有剛體，對齊地面
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false; // 啟用動力學
                }
                else
                {
                    placePosition = AlignWithGround(placePosition);
                }

                grabbedObject.transform.position = placePosition; // 設置最終放置位置
            }

            // 重置抓取狀態
            grabbedObject = null;
            isGrabbing = false;
        }
    }

    /// <summary>
    /// 計算物體放置的位置，考慮食材大小
    /// </summary>
    /// <returns>放置位置</returns>
    Vector3 CalculatePlacePosition()
    {
        // 根據玩家面朝方向計算放置位置
        Vector3 forward = transform.forward;

        // 獲取食材的邊界（Collider.bounds）
        Collider objectCollider = grabbedObject.GetComponent<Collider>();
        float objectDepth = objectCollider != null ? objectCollider.bounds.extents.z : 0.5f; // 物體深度（前後）
        float objectWidth = objectCollider != null ? objectCollider.bounds.extents.x : 0.5f; // 物體寬度（左右）

        // 計算放置距離：基礎距離 + 物體深度
        float adjustedDistance = basePlaceDistance + objectDepth;

        // 計算最終放置位置
        Vector3 placePosition = transform.position + forward * adjustedDistance;
        placePosition.y = transform.position.y; // 保持與玩家相同高度
        return placePosition;
    }

    /// <summary>
    /// 使用射線對齊地面，保證物體底部接觸地面
    /// </summary>
    /// <param name="position">初始放置位置</param>
    /// <returns>地面對齊後的位置</returns>
    Vector3 AlignWithGround(Vector3 position)
    {
        Collider objectCollider = grabbedObject.GetComponent<Collider>();
        if (objectCollider != null)
        {
            // 確定物體底部位置
            float objectBottom = objectCollider.bounds.min.y;

            // 從物體底部向下發射射線
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(position.x, objectBottom + 1f, position.z), Vector3.down, out hit, 2f))
            {
                position.y = hit.point.y + (position.y - objectBottom); // 保證底部對齊地面
            }
        }
        return position;
    }

    
    
}