using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_move : MonoBehaviour
{
    public GameObject background_1;         // ù ��° ��� ������Ʈ
    public GameObject background_2;         // �� ��° ��� ������Ʈ
    public float scrollSpeed = 2f;  // �̵� �ӵ�
    public float offsetX = 37.9f;   // ������Ʈ �� x �Ÿ�

    private void Update()
    {
        // �� �� �������� �̵�
        background_1.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        background_2.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // obj1�� ���� ������ ���������� �̵�
        if (background_1.transform.position.x <= -offsetX)
        {
            background_1.transform.position += new Vector3(offsetX * 2f, 0f, 0f);
        }

        // obj2�� ���� ������ ���������� �̵�
        if (background_2.transform.position.x <= -offsetX)
        {
            background_2.transform.position += new Vector3(offsetX * 2f, 0f, 0f);
        }
    }
}