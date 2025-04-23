using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    private MonsterAI ai;

    void Start()
    {
        ai = GetComponentInParent<MonsterAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster_Head"))
        {
            ai.bodyBlockedByHead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster_Head"))
        {
            ai.bodyBlockedByHead = false;
        }
    }
}
