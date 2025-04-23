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
        // 현재 DamageNumber 프리팹과 설정 불러오기
        DamageNumber prefab = DNP_DemoManager.instance.GetCurrent();
        DNP_PrefabSettings settings = DNP_DemoManager.instance.GetSettings();

        // 숫자 위치 (위에 뜨게끔 살짝 위로 오프셋)
        Vector3 spawnPos = transform.position + new Vector3(0f, 1f, 0f);

        // DamageNumber 생성
        DamageNumber newNumber = prefab.Spawn(spawnPos, damage);

        // 숫자가 캐릭터를 따라다니도록 설정
        newNumber.SetFollowedTarget(transform);

        // 현재 설정 적용
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

