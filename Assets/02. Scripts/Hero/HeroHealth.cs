using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DamageNumbersPro;
using DamageNumbersPro.Demo;

public class HeroHealth : MonoBehaviour
{
    public int maxHp = 10;
    private int hp;

    public Slider hpSlider;
    public GameObject deathEffectPrefab;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;


    void Start()
    {
        hp = maxHp;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        hp = Mathf.Clamp(hp, 0, maxHp);

        if (hpSlider != null)
        {
            hpSlider.value = hp;
        }

        StartCoroutine(FlashWhite());
        SpawnDamageNumber(amount);

        if (hp <= 0)
        {
            Die();
        }
    }
    void SpawnDamageNumber(float damage)
    {
        // ���� DamageNumber �����հ� ���� �ҷ�����
        DamageNumber prefab = DNP_DemoManager.instance.GetCurrent();
        DNP_PrefabSettings settings = DNP_DemoManager.instance.GetSettings();

        // ���� ��ġ (���� �߰Բ� ��¦ ���� ������)
        Vector3 spawnPos = transform.position + new Vector3(0f, 1f, 0f);

        // DamageNumber ����
        DamageNumber newNumber = prefab.Spawn(spawnPos, damage);

        // ���ڰ� ĳ���͸� ����ٴϵ��� ����
        newNumber.SetFollowedTarget(transform);

        // ���� ���� ����
        settings.Apply(newNumber);
    }

    IEnumerator FlashWhite()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

