using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
    private MonsterAI ai;

    void Start()
    {
        ai = GetComponentInParent<MonsterAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster_Body"))
        {
            ai.headTouchingBody = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster_Body"))
        {
            ai.headTouchingBody = false;
        }
    }
}
