using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DamageNumbersPro.Demo
{
    public class DNP_DemoManager : MonoBehaviour
    {
        public static DNP_DemoManager instance;

        Text currentPrefabText;
        Text currentIndexText;

        DamageNumber[] prefabs;
        int currentIndex;
        DNP_PrefabSettings currentSettings;

        CanvasGroup fade;
        bool fadeOut;
        string loadScene;

        void Awake()
        {
            instance = this;

            // 프리팹 가져오기
            Transform parent = GameObject.Find("Special").transform.Find("Prefabs/Damage Numbers");
            prefabs = new DamageNumber[parent.childCount];
            for (int n = 0; n < parent.childCount; n++)
            {
                prefabs[n] = parent.GetChild(n).GetComponent<DamageNumber>();
            }
            parent.gameObject.SetActive(false);

            // 텍스트 연결
            Transform guiParent = GameObject.Find("Special").transform.Find("GUI");
            currentPrefabText = guiParent.Find("Background/Current").GetComponent<Text>();
            currentIndexText = guiParent.Find("Background/Index").GetComponent<Text>();

            // 페이드용 캔버스
            Transform fadeTransform = transform.Find("GUI/Fade");
            if (fadeTransform != null)
            {
                fade = fadeTransform.GetComponent<CanvasGroup>();
            }

            currentIndex = 0;
            UpdateCurrent();

#if !UNITY_EDITOR && UNITY_WEBGL
                WebGLInput.captureAllKeyboardInput = true;    
#endif
        }

        void Start()
        {
            if (fade != null)
            {
                fade.alpha = 1f;
            }
        }

        void Update()
        {
            if (fade != null)
            {
                if (fadeOut)
                {
                    fade.alpha += Time.deltaTime * 4;

                    if (fade.alpha >= 0.999f)
                    {
                        SceneManager.LoadScene(loadScene);
                        enabled = false;
                    }

                    return;
                }
                else
                {
                    if (fade.alpha > 0)
                    {
                        fade.alpha -= Time.deltaTime * 3;
                    }
                }
            }
        }

        public void SwitchScene(string sceneName)
        {
            fadeOut = true;
            loadScene = sceneName;

            if (DNP_Camera.instance != null)
            {
                DNP_Camera.instance.enabled = false;
            }

            DNP_2DDemo demo2D = FindObjectOfType<DNP_2DDemo>();
            if (demo2D)
            {
                demo2D.enabled = false;
            }

            if (DNP_GUI.instance != null)
            {
                DNP_GUI.instance.enabled = false;
            }
        }

        void UpdateCurrent()
        {
            currentPrefabText.text = "➞ " + prefabs[currentIndex].name;
            currentIndexText.text = (currentIndex + 1) + "/" + prefabs.Length;

            currentSettings = prefabs[currentIndex].GetComponent<DNP_PrefabSettings>();
        }

        public DamageNumber GetCurrent()
        {
            return prefabs[currentIndex];
        }

        public DNP_PrefabSettings GetSettings()
        {
            if (currentSettings == null)
            {
                currentSettings = prefabs[currentIndex].gameObject.AddComponent<DNP_PrefabSettings>();
            }

            return currentSettings;
        }
    }
}
