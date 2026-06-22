using UnityEngine;

public class StunRotateAll : MonoBehaviour
{
    [Header("旋轉速度設定")]
    public float rotateSpeed = 150f;

    // 讓每顆星星的初始旋轉方向稍微錯開，看起來比較自然
    private Vector3 randomAxis;

    void Start()
    {
        // 隨機一個旋轉軸，如果不想要隨機，可以直接用 Vector3.up (繞Y軸轉)
        randomAxis = Vector3.up;
    }

    void Update()
    {
        // 讓自己持續旋轉
        transform.Rotate(randomAxis * rotateSpeed * Time.deltaTime,Space.World);
    }
}
