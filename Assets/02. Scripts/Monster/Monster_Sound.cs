using System.Collections;
using UnityEngine;

public class Monster_Sound : MonoBehaviour
{
    public AudioClip[] monsterSounds;          // 몬스터 사운드 배열 (5개)
    private AudioSource audioSource;

    public Vector2 delayBetweenSounds = new Vector2(2f, 5f); // 랜덤 재생 간격

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
                // 랜덤 사운드 선택
                AudioClip randomClip = monsterSounds[Random.Range(0, monsterSounds.Length)];
                audioSource.PlayOneShot(randomClip);
            }

            // 다음 사운드까지 기다릴 시간
            float delay = Random.Range(delayBetweenSounds.x, delayBetweenSounds.y);
            yield return new WaitForSeconds(delay);
        }
    }
}
