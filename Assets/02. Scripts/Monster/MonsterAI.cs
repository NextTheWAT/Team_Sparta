using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float jumpForce = 5f; // 점프 힘
    public float jumpCooldown = 3f; // 점프 쿨타임 (초)

    private Animator animator;
    private Rigidbody2D rb;

    private Transform currentTarget;
    private bool isDead = false;
    private bool isInRange = false;
    private bool isJumping = false;

    // ✅ 점프 제한 관련
    private float nextJumpTime = 0f;

    // 트리거 충돌 체크용 (있다면)
    [HideInInspector] public bool headTouchingBody = false;
    [HideInInspector] public bool bodyBlockedByHead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        FindClosestHero();

        if (currentTarget == null)
        {
            animator.SetBool("IsAttacking", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (distance <= attackRange)
        {
            isInRange = true;
            animator.SetBool("IsAttacking", true);
            rb.velocity = new Vector2(0f, rb.velocity.y);

            // ✅ 밀림 방지
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            isInRange = false;
            animator.SetBool("IsAttacking", false);

            // ✅ 다시 이동 가능
            if (!isJumping)
            {
                Vector2 dir = (currentTarget.position - transform.position).normalized;
                rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

                // ✅ 이동 가능 상태로 해제
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }


        // 자동 점프 조건 + 쿨타임 체크
        if (headTouchingBody && !bodyBlockedByHead && !isJumping && Time.time > nextJumpTime)
        {
            Jump();
        }

        // 테스트: 수동 점프도 쿨타임 적용
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextJumpTime)
        {
            Jump();
        }
    }

    void FindClosestHero()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        float minDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject hero in heroes)
        {
            float dist = Vector2.Distance(transform.position, hero.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = hero.transform;
            }
        }

        currentTarget = closest;
    }

    public void OnAttack()
    {
        if (!isInRange || currentTarget == null) return;

        var health = currentTarget.GetComponent<BoxHealth>();
        if (health != null)
        {
            health.TakeDamage(1);
            return;
        }

        var charHealth = currentTarget.GetComponent<HeroHealth>();
        if (charHealth != null)
        {
            charHealth.TakeDamage(1);
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        rb.velocity = Vector2.zero;
    }

    public void Jump()
    {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        nextJumpTime = Time.time + jumpCooldown; // ✅ 다음 점프 가능 시간 설정
        StartCoroutine(ResetJump());
    }

    System.Collections.IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.5f); // 공중 시간
        isJumping = false;
    }
}
