using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GrabAndThrow : MonoBehaviour
{
    public GameObject Camera;
    public float throwForce = 15.0f;

    [Header("Trajectory Settings")]
    public LineRenderer trajectoryLine; // 引導線
    public int trajectoryPoints = 50;
    public float timeStep = 0.1f;
    public Vector3 throwOffset = new Vector3(2.5f, 2f, 0f);

    [Header("Backpack Settings")]
    private GameObject[] backpack = new GameObject[2]; // 背包
    public Image slot0Image; // Slot 0 的 UI 元素
    public Image slot1Image; // Slot 1 的 UI 元素

    void Start()
    {
        if (trajectoryLine != null)
        {
            trajectoryLine.startWidth = 1f;
            trajectoryLine.endWidth = 1f;
            trajectoryLine.useWorldSpace = true;
            trajectoryLine.positionCount = trajectoryPoints;
        }

        // 初始化 UI
        UpdateUI();
    }

    void Update()
    {
        // Grab (E)
        if (Input.GetKeyDown(KeyCode.E) && backpack[0] == null)
        {
            GrabObject();
        }

        // Throw (F)
        if (Input.GetKeyDown(KeyCode.F) && backpack[0] != null)
        {
            ThrowObject();
        }

        // Swap (Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapBackpackItems();
        }

        // 更新引導線
        if (backpack[0] != null)
        {
            UpdateTrajectory();
        }
        else
        {
            ClearTrajectory();
        }
    }

    void GrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, 10.0f))
        {
            if (hit.transform.CompareTag("Food"))
            {
                // 嘗試抓取物品
                var orgName = hit.transform.gameObject.name;
                var removestr = "(Clone)";
                var objectName = orgName.Replace(removestr, "");

                // 加載抓取的物品到位置0
                backpack[0] = Resources.Load<GameObject>("prefabs/" + objectName);
                Destroy(hit.transform.gameObject);

                Debug.Log($"Grabbed {objectName} into Slot 0.");

                // 更新 UI
                UpdateUI();
            }
        }
    }

    void ThrowObject()
    {
        Vector3 throwStartPosition = Camera.transform.position +
                                     Camera.transform.right * throwOffset.x +
                                     Camera.transform.up * throwOffset.y +
                                     Camera.transform.forward * throwOffset.z;

        GameObject clone = Instantiate(backpack[0], throwStartPosition, quaternion.identity);
        clone.AddComponent<DestroyWithTimer>();

        Rigidbody cloneRigidbody = clone.GetComponent<Rigidbody>();
        cloneRigidbody.velocity = Camera.transform.forward * throwForce;

        Debug.Log($"Threw {backpack[0].name} from Slot 0.");

        backpack[0] = null;

        // 更新 UI
        UpdateUI();
        ClearTrajectory();
    }

    void SwapBackpackItems()
    {
        GameObject temp = backpack[0];
        backpack[0] = backpack[1];
        backpack[1] = temp;

        Debug.Log($"Swapped items: Slot 0 = {backpack[0]?.name ?? "Empty"}, Slot 1 = {backpack[1]?.name ?? "Empty"}");

        // 更新 UI
        UpdateUI();
    }

    void UpdateUI()
{
    // 更新 Slot 0 的圖標
    if (backpack[0] != null)
    {
        string objectName = backpack[0].name;

        // 從 Prefab 加載
        GameObject prefab = Resources.Load<GameObject>("Icons/" + objectName);
        if (prefab != null)
        {
            Sprite sprite = prefab.GetComponent<SpriteRenderer>()?.sprite;
            if (sprite != null)
            {
                slot0Image.sprite = sprite;
                slot0Image.enabled = true;
                slot0Image.color = new Color(1, 1, 1, 1); // 確保不透明
                Debug.Log($"Successfully updated Slot 0 with sprite from Prefab: {sprite.name}");
            }
            else
            {
                Debug.LogError($"Prefab {objectName} does not contain a SpriteRenderer or Sprite.");
            }
        }
        else
        {
            Debug.LogError($"Prefab not found in path: Icons/{objectName}");
        }
    }
    else
    {
        slot0Image.sprite = null;
        slot0Image.enabled = false;
    }

    // 更新 Slot 1 的圖標
    if (backpack[1] != null)
    {
        string objectName = backpack[1].name;

        // 從 Prefab 加載
        GameObject prefab = Resources.Load<GameObject>("Icons/" + objectName);
        if (prefab != null)
        {
            Sprite sprite = prefab.GetComponent<SpriteRenderer>()?.sprite;
            if (sprite != null)
            {
                slot1Image.sprite = sprite;
                slot1Image.enabled = true;
                slot1Image.color = new Color(1, 1, 1, 1); // 確保不透明
                Debug.Log($"Successfully updated Slot 1 with sprite from Prefab: {sprite.name}");
            }
            else
            {
                Debug.LogError($"Prefab {objectName} does not contain a SpriteRenderer or Sprite.");
            }
        }
        else
        {
            Debug.LogError($"Prefab not found in path: Icons/{objectName}");
        }
    }
    else
    {
        slot1Image.sprite = null;
        slot1Image.enabled = false;
    }
}

    void UpdateTrajectory()
    {
        if (trajectoryLine == null || backpack[0] == null) return;

        Vector3 startPosition = Camera.transform.position +
                                Camera.transform.right * throwOffset.x +
                                Camera.transform.up * throwOffset.y +
                                Camera.transform.forward * throwOffset.z;

        Vector3 startVelocity = Camera.transform.forward * throwForce;
        Vector3 currentPosition = startPosition;

        trajectoryLine.positionCount = trajectoryPoints;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * timeStep;
            Vector3 trajectoryPoint = currentPosition + startVelocity * time +
                                      0.5f * Physics.gravity * time * time;
            trajectoryLine.SetPosition(i, trajectoryPoint);
        }
    }

    void ClearTrajectory()
    {
        if (trajectoryLine != null)
        {
            trajectoryLine.positionCount = 0;
        }
    }
}