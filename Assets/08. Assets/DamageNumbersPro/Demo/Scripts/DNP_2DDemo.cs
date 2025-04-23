using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

namespace DamageNumbersPro.Demo
{
    public class DNP_2DDemo : MonoBehaviour
    {
        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // ❗ 외부에서 호출할 수 있도록 public으로 공개
        public void ShootAtPosition(Vector3 screenPosition)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = -5;

            RaycastHit hit;
            Physics.Raycast(worldPosition, Vector3.forward, out hit, 10f);

            // 데미지 숫자 프리팹 및 설정 불러오기
            DNP_PrefabSettings settings = DNP_DemoManager.instance.GetSettings();
            DamageNumber prefab = DNP_DemoManager.instance.GetCurrent();

            // 데미지 수치 생성
            float number = 1 + Mathf.Pow(Random.value, 2.2f) * settings.numberRange;
            if (prefab.digitSettings.decimals == 0)
            {
                number = Mathf.Floor(number);
            }

            // 데미지 숫자 생성
            DamageNumber newDamageNumber = prefab.Spawn(worldPosition, number);

            if (hit.collider != null)
            {
                DNP_Target dnpTarget = hit.collider.GetComponent<DNP_Target>();
                if (dnpTarget != null)
                {
                    dnpTarget.Hit();
                }

                newDamageNumber.SetFollowedTarget(hit.collider.transform);
            }

            // 설정 적용
            settings.Apply(newDamageNumber);
        }
    }
}
