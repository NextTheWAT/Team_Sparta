using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_move : MonoBehaviour
{
    public GameObject background_1;         // 첫 번째 배경 오브젝트
    public GameObject background_2;         // 두 번째 배경 오브젝트
    public float scrollSpeed = 2f;  // 이동 속도
    public float offsetX = 37.9f;   // 오브젝트 간 x 거리

    private void Update()
    {
        // 둘 다 왼쪽으로 이동
        background_1.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        background_2.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // obj1이 왼쪽 끝나면 오른쪽으로 이동
        if (background_1.transform.position.x <= -offsetX)
        {
            background_1.transform.position += new Vector3(offsetX * 2f, 0f, 0f);
        }

        // obj2가 왼쪽 끝나면 오른쪽으로 이동
        if (background_2.transform.position.x <= -offsetX)
        {
            background_2.transform.position += new Vector3(offsetX * 2f, 0f, 0f);
        }
    }
}