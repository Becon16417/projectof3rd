using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("目標對象")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("縮放設定 (像素單位)")]
    [Tooltip("相機最低高度 (兩人在中點時)")]
    [SerializeField] private float minHeight = 800f;
    [Tooltip("相機最高高度 (兩人拉開時)")]
    [SerializeField] private float maxHeight = 1800f;
    [Tooltip("縮放靈敏度，數值越大縮越快 (建議 0.5 - 1.0)")]
    [SerializeField] private float zoomFactor = 0.7f;
    [Tooltip("縮放平滑速度")]
    [SerializeField] private float zoomSpeed = 5f;

    [Header("移動與邊界 (像素單位)")]
    [SerializeField] private float followSmoothTime = 0.2f;
    [Tooltip("請輸入符合的偏移量，例如 (0, 0, -800)")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -800);

    [Tooltip("地圖左邊界 (X座標)")]
    [SerializeField] private float minX = -960f;
    [Tooltip("地圖右邊界 (X座標)")]
    [SerializeField] private float maxX = 960f;

    private Camera cam;
    private Vector3 currentVelocity;
    private float currentHeight;

    void Start()
    {
        cam = GetComponent<Camera>();
        // 改為 Perspective (透視) 模式以獲得 Overcooked 感
        cam.orthographic = false;
        // 初始高度
        currentHeight = minHeight;

        // 提醒：Far Clipping Plane 記得在 Inspector 設為 5000 以上
        if (cam.farClipPlane < 3000) cam.farClipPlane = 5000;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        // 核心邏輯順序與舊版一致
        HandleZoom();
        HandlePosition();
    }

    void HandleZoom()
    {
        // 1. 計算兩人的距離
        float distance = Vector3.Distance(player1.position, player2.position);

        // 2. 換算目標高度 (取代舊版的 Size) 並限制範圍
        float targetHeight = Mathf.Clamp(distance * zoomFactor + minHeight, minHeight, maxHeight);

        // 3. 平滑變更高度數值
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * zoomSpeed);
    }

    void HandlePosition()
    {
        Vector3 midpoint = (player1.position + player2.position) / 2f;

        // 1. 計算目標位置
        Vector3 targetPos = midpoint;

        // 2. 動態計算 Z 偏移，讓玩家保持在中心
        // 假設旋轉角是 60 度，比例約為 -0.577
        // 你可以把 0.577f 變成一個可調變數 [SerializeField] float zOffsetFactor
        float dynamicZOffset = currentHeight * -0.577f;

        targetPos.y = currentHeight;
        targetPos.z += dynamicZOffset; // 套用動態偏移
        targetPos.x += offset.x;       // 保留 X 軸的手動位移（通常是 0）

        // 3. 限制 X 軸邊界
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

        // 4. 平滑移動
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, followSmoothTime);
    }

    private void OnDrawGizmos()
    {
        if (player1 == null || player2 == null) return;

        // 畫出中點
        Vector3 midpoint = (player1.position + player2.position) / 2f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(midpoint, 20f); // 放大 Gizmo 尺寸以符合像素比例

        // 畫出目標位置
        Gizmos.color = Color.green;
        Vector3 targetPos = midpoint + offset;
        targetPos.y = currentHeight;
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        Gizmos.DrawSphere(targetPos, 20f);

        Gizmos.DrawLine(midpoint, targetPos);
    }
}
