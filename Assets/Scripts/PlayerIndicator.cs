using UnityEngine;
using TMPro;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private float displayDuration = 3f; // 顯示時間

    private Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;

        // 啟動後，在 3 秒後自動隱藏此物件
        Invoke("HideIndicator", displayDuration);
    }

    // 每一幀都執行，讓文字永遠面對相機
    void LateUpdate()
    {
        if (mainCameraTransform != null)
        {
            // 讓文字的正面朝向相機，且保持垂直
            transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                             mainCameraTransform.rotation * Vector3.up);
        }
    }

    void HideIndicator()
    {
        gameObject.SetActive(false);
    }
}