using UnityEngine;

public class MonsterHitPart : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ammo"))
        {
            // ���� ������ ü�� ������Ʈ �����ͼ� ������
            MonsterHealth monster = GetComponentInParent<MonsterHealth>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }

            Destroy(other.gameObject);
        }
    }
}
