using UnityEngine;
using TMPro;

public class MoneyPopup : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private TextMeshPro textMesh; // 3D 世界用這個

    public void SetText(string val)
    {
        textMesh.text = val;
    }

    void Start()
    {
        // 1 秒後銷毀
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 每一幀往上移動
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 文字永遠面向相機 
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }
}