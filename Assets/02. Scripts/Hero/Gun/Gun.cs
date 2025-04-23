using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GameObject ammoPrefab;      // 총알 프리팹
    public GameObject muzzle_Flash;     //머즐 플래쉬
    public GameObject empty_Cartridge;

    public Transform firePoint;        // 총알 발사 위치
    public float fireRate = 1f;        // 발사 간격 (초)
    public float bulletSpeed = 10f;    // 총알 속도

    private float fireCooldown = 0f;

    public float recoilDistance = 0.1f;     // 반동 거리
    public float recoilDuration = 0.05f;    // 반동 시간

    public float rotationSpeed = 720f; // 초당 회전 속도 (도 단위)

    public AudioClip shotSound;               // 총 발사 사운드 클립
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
    }


    void Update()
    {
        fireCooldown -= Time.deltaTime;

        GameObject target = FindClosestMonster();

        if (target != null)
        {
            // 몬스터 방향 바라보게 총 회전
            RotateGunTowards(target.transform.position);

            // 일정 시간마다 총알 발사
            if (fireCooldown <= 0f)
            {
                Shoot(target.transform.position);
                fireCooldown = fireRate;
            }
        }
    }


    GameObject FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector2.Distance(transform.position, monster.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = monster;
            }
        }

        return closest;
    }

    void Shoot(Vector3 targetPos)
    {
        int pelletCount = 5;            // 샷건 탄환 개수
        float spreadAngle = 10f;        // 전체 퍼짐 범위 (예: ±15도)

        Vector2 baseDir = (targetPos - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pelletCount; i++)
        {
            // 랜덤한 퍼짐 각도 (± spreadAngle / 2)
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float currentAngle = baseAngle + angleOffset;

            // 각도를 방향 벡터로 변환
            Vector2 shootDir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

            // 총알 생성 및 발사
            GameObject bullet = Instantiate(ammoPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDir.normalized * bulletSpeed;
            }

            Destroy(bullet, 2f);
        }
        if (audioSource != null && shotSound != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        // 시각/반동 효과는 한 번만
        StartCoroutine(FlashMuzzle());
        StartCoroutine(Empty_Cartridge());
        StartCoroutine(ApplyRecoil(-baseDir));
    }



    IEnumerator FlashMuzzle()
    {
        muzzle_Flash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzle_Flash.SetActive(false);
    }
    IEnumerator Empty_Cartridge()
    {
        empty_Cartridge.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        empty_Cartridge.SetActive(false);
    }
    IEnumerator ApplyRecoil(Vector2 recoilDir)
    {
        Vector3 originalPos = transform.localPosition;
        Vector3 recoilOffset = (Vector3)(recoilDir * recoilDistance);

        // 반동 방향으로 밀기
        transform.localPosition += recoilOffset;

        yield return new WaitForSeconds(recoilDuration);

        // 원래 위치로 되돌리기
        transform.localPosition = originalPos;
    }
    void RotateGunTowards(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - transform.position).normalized;

        // 목표 회전각 계산
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        // 현재 회전에서 목표 회전으로 부드럽게 회전
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
