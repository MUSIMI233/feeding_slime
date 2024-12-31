using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // 移動速度
    public float sprintSpeed = 8f; // 加速速度
    public float jumpForce = 5f; // 跳躍力度

    [Header("Power-Up Settings")]
    private bool isSprinting = false; // 是否處於加速狀態
    private float sprintDuration = 0f; // 超能力剩餘時間
    private Coroutine sprintCoroutine;

    public Image powerUpImage; // 超能力圖標

    private Rigidbody rb;
    private Vector3 moveDirection;
    private int groundContactCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleInput();

        // 更新超能力倒計時
        if (isSprinting && sprintDuration > 0)
        {
            sprintDuration -= Time.deltaTime;

            // 最後 5 秒內圖標閃爍
            if (sprintDuration <= 5f && powerUpImage != null)
            {
                float alpha = Mathf.PingPong(Time.time * 2f, 1f); // 透明度閃爍
                powerUpImage.color = new Color(1, 1, 1, alpha);
            }

            // 超能力結束
            if (sprintDuration <= 0f)
            {
                EndSprint();
            }
        }
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void HandleInput()
    {
        // 獲取鍵盤輸入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 計算基於玩家當前方向的移動向量
        moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        // 檢測跳躍
    
    }

    void ApplyMovement()
    {
        // 應用移動速度
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
        Vector3 velocity = moveDirection * currentSpeed;

        // 使用 Rigidbody 進行物理移動
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContactCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContactCount--;
        }
    }

    private bool IsGrounded()
    {
        return groundContactCount > 0;
    }

    /// <summary>
    /// 激活加速狀態
    /// </summary>
    /// <param name="duration">加速持續時間</param>
    public void ActivateSprint(float duration)
    {
        if (sprintCoroutine != null)
        {
            StopCoroutine(sprintCoroutine);
        }
        sprintCoroutine = StartCoroutine(SprintRoutine(duration));
    }

    private IEnumerator SprintRoutine(float duration)
    {
        isSprinting = true;
        sprintDuration = duration;

        // 顯示圖標
        if (powerUpImage != null)
        {
            powerUpImage.enabled = true;
            powerUpImage.color = new Color(1, 1, 1, 1); // 完全不透明
        }

        yield return new WaitForSeconds(duration);

        EndSprint();
    }

    private void EndSprint()
    {
        isSprinting = false;

        // 隱藏圖標
        if (powerUpImage != null)
        {
            powerUpImage.enabled = false;
        }

        sprintCoroutine = null;
    }
}