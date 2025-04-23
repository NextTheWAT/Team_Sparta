using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float rotationSpeed = 360f; // �ʴ� ȸ�� ���� (�� ����)

    void Update()
    {
        // Z���� �������� �ð� ���� ȸ�� (������ �ݽð�)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}