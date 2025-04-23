using UnityEngine;

public class MonsterHitPart : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ammo"))
        {
            // 상위 몬스터의 체력 컴포넌트 가져와서 데미지
            MonsterHealth monster = GetComponentInParent<MonsterHealth>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }

            Destroy(other.gameObject);
        }
    }
}
