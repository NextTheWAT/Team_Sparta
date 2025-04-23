using System.Collections;
using UnityEngine;

public class Monster_Sound : MonoBehaviour
{
    public AudioClip[] monsterSounds;          // ���� ���� �迭 (5��)
    private AudioSource audioSource;

    public Vector2 delayBetweenSounds = new Vector2(2f, 5f); // ���� ��� ����

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;

        if (monsterSounds.Length > 0 && audioSource != null)
        {
            StartCoroutine(PlayMonsterSounds());
        }
    }

    IEnumerator PlayMonsterSounds()
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                // ���� ���� ����
                AudioClip randomClip = monsterSounds[Random.Range(0, monsterSounds.Length)];
                audioSource.PlayOneShot(randomClip);
            }

            // ���� ������� ��ٸ� �ð�
            float delay = Random.Range(delayBetweenSounds.x, delayBetweenSounds.y);
            yield return new WaitForSeconds(delay);
        }
    }
}
