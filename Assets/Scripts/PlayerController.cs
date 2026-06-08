using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private int playerID;
    public int GetPlayerID() => playerID;

    [Tooltip("Movement")]
    [Range(500f, 800f)]
    public float moveSpeed = 600f;

    [Header("KeyCode")]
    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode giveKey;

    [Header("attack")]
    [SerializeField] private float attackRange = 200f; // 攻擊距離
    [SerializeField] [Range(80f, 120f)] private float attackRadius = 100f; // 攻擊判定半徑
    [SerializeField] [Range(0.5f, 5f)] private float stunDuration = 2.0f; // 暈眩時間

    [Header("NPC Cache")]
    private NPCVision[] cachedNPCs; // 用來儲存場景中所有 NPC 的引用

    [Header("Invincibility & Boost")]
    [SerializeField] private float invincibilityDuration = 2.0f; // 無敵時間
    [SerializeField] private float speedBoostMultiplier = 1.5f; // 加速倍率
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;

    private bool isStunned;
    private float stunTimer = 0f;
    private Rigidbody rb;
    private Vector3 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 在遊戲開始時只抓取一次，存進快取中
        cachedNPCs = FindObjectsOfType<NPCVision>();

        Debug.Log($"玩家 {playerID} 已快取 {cachedNPCs.Length} 個 NPC 視線系統");
    }

    void Update()
    {
        // 處理暈眩計時
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }

            if (rb.linearVelocity.magnitude < 0.5f)
            {
                rb.linearVelocity = Vector3.zero;
            }
            return;
        }

        GetInput();
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }
    }

    void FixedUpdate()
    {
        if (!isStunned)
        {
            ApplyMovement();
        }
    }

    void GetInput()
    {
        float x = 0;
        float z = 0;

        if (Input.GetKey(moveUp)) z = 1;
        else if (Input.GetKey(moveDown)) z = -1;

        if (Input.GetKey(moveLeft)) x = -1;
        else if (Input.GetKey(moveRight)) x = 1;

        moveInput = new Vector3(x, 0, z).normalized;
    }

    void ApplyMovement()
    {
        if (isStunned) return;

        // 基礎速度
        float currentSpeed = moveSpeed;

        // 如果在無敵期間，速度加倍
        if (isInvincible)
        {
            currentSpeed *= speedBoostMultiplier;
        }

        rb.linearVelocity = moveInput * currentSpeed;

        if (moveInput != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveInput, Time.deltaTime * 15f);
        }
    }

    void HandleStun()
    {
        rb.linearVelocity = Vector3.zero; // 暈眩不能動
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            isStunned = false;
            Debug.Log($"玩家 {playerID} 恢復行動");
        }
    }

    // 被打中時觸發
    public void GetHit(float duration)
    {
        // 如果正在暈眩中 或 處於無敵狀態，則直接無視這次攻擊
        if (isStunned || isInvincible) return;

        // 觸發掉錢邏輯
        PlayerStats stats = GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.LoseMoneyOnAttack();
        }

        // 觸發暈眩
        isStunned = true;
        stunTimer = duration;
    }


    void PerformAttack()
    {
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, attackRadius);

        bool hitAnyPlayer = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.CompareTag("Player"))
            {
                PlayerController opponent = hitCollider.GetComponent<PlayerController>();
                if (opponent != null)
                {
                    opponent.GetHit(stunDuration);
                    hitAnyPlayer = true;
                }
            }
        }

        // 只要有擊中對手，就檢查是否有 NPC 看到
        if (hitAnyPlayer)
        {
            CheckIfCaughtByNPC();
        }
    }

    void CheckIfCaughtByNPC()
    {        
        if (cachedNPCs == null || cachedNPCs.Length == 0) return;

        foreach (NPCVision npc in cachedNPCs)
        {
            if (npc != null && npc.CanSeePlayer(this.transform))
            {
                PlayerStats myStats = GetComponent<PlayerStats>();
                if (myStats != null)
                {
                    myStats.AddAffection(-1);
                    Debug.Log($"{gameObject.name} 被 NPC 抓到了！");
                }
                break;
            }
        }
    }


    // 在 Scene 視窗畫出攻擊範圍 (方便調整數值)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackPoint, attackRadius);
    }
}
