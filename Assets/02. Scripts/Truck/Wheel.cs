using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float rotationSpeed = 360f; // 초당 회전 각도 (도 단위)

    void Update()
    {
        // Z축을 기준으로 시계 방향 회전 (음수면 반시계)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}