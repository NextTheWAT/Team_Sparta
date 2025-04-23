using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GameObject ammoPrefab;      // �Ѿ� ������
    public GameObject muzzle_Flash;     //���� �÷���
    public GameObject empty_Cartridge;

    public Transform firePoint;        // �Ѿ� �߻� ��ġ
    public float fireRate = 1f;        // �߻� ���� (��)
    public float bulletSpeed = 10f;    // �Ѿ� �ӵ�

    private float fireCooldown = 0f;

    public float recoilDistance = 0.1f;     // �ݵ� �Ÿ�
    public float recoilDuration = 0.05f;    // �ݵ� �ð�

    public float rotationSpeed = 720f; // �ʴ� ȸ�� �ӵ� (�� ����)

    public AudioClip shotSound;               // �� �߻� ���� Ŭ��
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
            // ���� ���� �ٶ󺸰� �� ȸ��
            RotateGunTowards(target.transform.position);

            // ���� �ð����� �Ѿ� �߻�
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
        int pelletCount = 5;            // ���� źȯ ����
        float spreadAngle = 10f;        // ��ü ���� ���� (��: ��15��)

        Vector2 baseDir = (targetPos - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pelletCount; i++)
        {
            // ������ ���� ���� (�� spreadAngle / 2)
            float angleOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            float currentAngle = baseAngle + angleOffset;

            // ������ ���� ���ͷ� ��ȯ
            Vector2 shootDir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

            // �Ѿ� ���� �� �߻�
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

        // �ð�/�ݵ� ȿ���� �� ����
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

        // �ݵ� �������� �б�
        transform.localPosition += recoilOffset;

        yield return new WaitForSeconds(recoilDuration);

        // ���� ��ġ�� �ǵ�����
        transform.localPosition = originalPos;
    }
    void RotateGunTowards(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - transform.position).normalized;

        // ��ǥ ȸ���� ���
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        // ���� ȸ������ ��ǥ ȸ������ �ε巴�� ȸ��
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
