using UnityEngine;

public class NPCVision : MonoBehaviour
{
    [Header("視線設定")]
    [SerializeField] private float viewWidth = 400f;  // 矩形的寬度（左右範圍）
    [SerializeField] private float viewDepth = 600f;  // 矩形的深度（前方距離）
    [SerializeField] private LayerMask obstacleMask; // 設定哪些層級是障礙物 (例如 "Obstacle")
    [SerializeField] private LayerMask playerMask;   // 設定玩家層級 (例如 "Player")

    // 檢查目標玩家是否在視線內且未被阻擋
    public bool CanSeePlayer(Transform playerTransform)
    {
        // 1. 將玩家的世界座標轉換成 NPC 的「局部座標」
        // 這會讓 NPC 的正前方永遠是 Z 軸，左右是 X 軸
        Vector3 localPos = transform.InverseTransformPoint(playerTransform.position);

        // 2. 矩形範圍判定
        // Z 軸要在 0 到深度之間
        // X 軸要在負的一半寬度到正的一半寬度之間
        bool isInRectangle = (localPos.z > 0 && localPos.z < viewDepth) &&
                             (Mathf.Abs(localPos.x) < viewWidth / 2f);

        if (isInRectangle)
        {
            // 3. 障礙物檢測 (Linecast)
            if (!Physics.Linecast(transform.position, playerTransform.position, obstacleMask))
            {
                return true; // 在矩形內且沒被擋住
            }
        }
        return false;
    }

    // 在 Scene 視窗畫出矩形框，方便你手動擺放
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 建立一個矩陣讓 Gizmos 跟著 NPC 旋轉
        Gizmos.matrix = transform.localToWorldMatrix;

        // 畫出視線矩形（中心點在前方一半深度處）
        Vector3 center = new Vector3(0, 0, viewDepth / 2f);
        Vector3 size = new Vector3(viewWidth, 10f, viewDepth);
        Gizmos.DrawWireCube(center, size);
    }
}
