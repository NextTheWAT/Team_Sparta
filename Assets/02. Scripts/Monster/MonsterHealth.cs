using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DamageNumbersPro;
using DamageNumbersPro.Demo;


public class MonsterHealth : MonoBehaviour
{
    public int maxHp = 5;
    private int hp;

    public GameObject deathEffectPrefab;
    public Slider hpSlider;

    private Image fillImage; // 슬라이더 내부의 색상 바꿀 이미지

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;

    void Start()
    {
        hp = maxHp;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;

            // Fill 이미지 가져오기
            fillImage = hpSlider.fillRect.GetComponent<Image>();
            //UpdateHealthColor(); // 초기 색상 설정
        }
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        hp = Mathf.Clamp(hp, 0, maxHp);

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {hp}");

        SpawnDamageNumber(amount);

        if (hpSlider != null)
        {
            hpSlider.value = hp;
            //UpdateHealthColor();
        }

        StartCoroutine(FlashWhite());

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

        // 숫자 위치 (몬스터 위에 뜨게끔 살짝 위로 오프셋)
        Vector3 spawnPos = transform.position + new Vector3(0f, 1f, 0f);

        // DamageNumber 생성
        DamageNumber newNumber = prefab.Spawn(spawnPos, damage);

        // 숫자가 좀비를 따라다니도록 설정
        newNumber.SetFollowedTarget(transform);

        // 현재 설정 적용
        settings.Apply(newNumber);
    }


    IEnumerator FlashWhite()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.yellow;
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }
    }

    /*
    void UpdateHealthColor()
    {
        if (fillImage != null)
        {
            float ratio = (float)hp / maxHp;
            fillImage.color = Color.Lerp(Color.red, Color.white, ratio);
        }
    }
    */

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (deathEffectPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 0.5f, 0f);
            GameObject effect = Instantiate(deathEffectPrefab, spawnPos, Quaternion.identity);
            Destroy(effect, 2f);
        }

        Destroy(gameObject);
    }
}
