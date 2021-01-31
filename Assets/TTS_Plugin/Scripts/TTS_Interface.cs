////IMPORTANT
/// Remove this #define if you do not have TextMeshPro
#define TextMeshPro

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace LowteckTTS
{
    [RequireComponent(typeof(TTS))]
    public class TTS_Interface : MonoBehaviour
    {
        // Singleton
        public static TTS_Interface singleton_;
        static GameObject eventSystem_;
        bool canToggle = true;
        TtsUiManager UIObject;
        public static bool UIActive = false;
        bool[] m_raycaster_state_;
        float prevSpeed = 0.0f;

        //List of objects
        List<GraphicRaycaster> m_Raycaster_;
        List<TextMesh> Rndrs;

#if TextMeshPro
        List<TMPro.TextMeshPro> tmpRndrs;
#endif

        //Key bindings
        public bool DisableWithEsc = true;
        public KeyCode[] KeyBinding = new KeyCode[1] { KeyCode.T };

        // Audio Sources
        public bool pauseAllAudio_ = false;
        public delegate void ButtonPress();
        public static event ButtonPress TTSActivated, TTSDeactivated;

        //public static bool canRead = false;
        [SerializeField]
        public string renderTextureTag = "RenderTexture";

        AudioSource[] AudSources;
        bool[] AudToResume;
        TTS ttsSystem;
        Animator anim_;

        public static void EnablePlugin()
        {
            if(!singleton_)
                Instantiate(Resources.Load("TTS_Prefab"));
        }
        public static void DisablePlugin()
        {
            if (singleton_)
                GameObject.Destroy(singleton_.gameObject);
        }
        public static List<T> FindObjectsOfTypeAll<T>()
        {
            List<T> results = new List<T>();


            var s = SceneManager.GetActiveScene();
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }

            return results;
        }

        void Awake()
        {
            if (!singleton_)
            {
                singleton_ = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }


            //Instantiate event system if there is none.
            if (FindObjectOfType<EventSystem>() == null)
                eventSystem_ = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
           //Deactivate UI object
           UIObject = GetComponentInChildren<TtsUiManager>(true);

            UIObject.gameObject.SetActive(false);

            anim_ = UIObject.GetComponentInChildren<Animator>();
            //initialize TTS
            ttsSystem = GetComponent<TTS>();
        }

        void Update()
        {


            //Check if the left Mouse button is clicked
            if (UIActive)
            {
                // If mouse is clicked with right parameters
                if (!TtsUiManager.optionsActive && Input.GetMouseButtonDown(0))
                {
                    List<String> str = new List<String>();
                    //for(int i=0;i<500;i++)
                    RayForTextMesh(str);
                    RaycastForUIText(str);
                    if (str.Count > 0)
                        ttsSystem.skipToEnd();
                    foreach (var S in str)
                    {
                        ttsSystem.playTTS(S);
                        //print(S);
                    }
                }

                //Managing UI Wave animation
                if (ttsSystem.isSpeaking())
                    UIObject.startReading();
                else
                    UIObject.stopReading();
            }

            if (DisableWithEsc && Input.GetKeyDown(KeyCode.Escape))
                DeactivateTtsUi();

            foreach (var k in KeyBinding)
                if (Input.GetKeyDown(k))
                    toggleUI();


        }

        public void toggleUI()
        {
            if (canToggle)
            {
                if (UIActive) DeactivateTtsUi();
                else ActivateTtsUi();

                canToggle = false;
                StartCoroutine("resetToggle");
            }
        }
        IEnumerator resetToggle()
        {
            yield return new WaitForSecondsRealtime(0.5f); ;
            canToggle = true;
        }
        public void ActivateTtsUi()
        {
            UIActive = true;
            prevSpeed = Time.timeScale;
            Time.timeScale = 0;
            //UIObject.stopReading();
            UIObject.gameObject.SetActive(true);

            GetAllRaycasters();
            int i = 0;
            foreach (var m_Raycaster in m_Raycaster_)
            {
                m_raycaster_state_[i] = m_Raycaster.enabled;
                m_Raycaster.enabled = false;
                i++;
            }


            Rndrs = FindObjectsOfTypeAll<TextMesh>();
#if TextMeshPro
            tmpRndrs = FindObjectsOfTypeAll<TMPro.TextMeshPro>();
#endif

            //Handle Audio pauses
            if (pauseAllAudio_)
                PauseAllAudio();

            if (TTSActivated != null)
                TTSActivated();

        }
        public void DeactivateTtsUi()
        {
            UIActive = false;
            Time.timeScale = prevSpeed;
            UIObject.DeactivateOptions();
            anim_.SetBool("Close", true);
            Invoke("disableUIObj", 0.5f);
            //UIObject.gameObject.SetActive(false);
            ttsSystem.skipToEnd();

            for (int i = 0; i < m_Raycaster_.Count; i++)
                m_Raycaster_[i].enabled = m_raycaster_state_[i];

            //Handle audio resume
            if (pauseAllAudio_)
                ResumeAllAudio();

            if (TTSDeactivated != null)
                TTSDeactivated();
        }
        void disableUIObj()
        {
            UIObject.gameObject.SetActive(false);
        }

        void RaycastForUIText(List<String> strs)
        {
            foreach (var m_Raycaster in m_Raycaster_)
            {

                //Set up the new Pointer Event
                var m_PointerEventData = new PointerEventData(EventSystem.current);

                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventData.position = Input.mousePosition;

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {

                    var tst = result.gameObject.GetComponent<Text>();
                    //Debug.Log(result.gameObject.name);
                    if (tst)
                    {
                        //Debug.Log("TEXT: " + tst.text);
                        strs.Add(tst.text);
                        break;
                    }

#if TextMeshPro
                    var tmpro = result.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    //Debug.Log(result.gameObject.name);
                    if (tmpro)
                    {
                        //Debug.Log("TEXT: " + tmpro.text);
                        strs.Add(tmpro.text);
                        break;
                    }
#endif
                }




            }
        }

        void RayForTextMesh(List<String> strs)
        {
            List<Vector3> mousePos = new List<Vector3>();
            mousePos.Add(Input.mousePosition);

            if (Camera.allCamerasCount > 1)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // do we hit a RT plane?
                if (Physics.Raycast(ray, out hit))
                {
                    //Check if Raycast has hit RenderTexture
                    if (hit.transform.tag == renderTextureTag)
                    {
                        var localPoint = hit.textureCoord;
                        // convert the hit texture coordinates into camera coordinates
                        foreach (var cam in Camera.allCameras)
                        {
                            if (cam.isActiveAndEnabled && cam != Camera.main)
                                mousePos.Add(Camera.main.WorldToScreenPoint(cam.ScreenToWorldPoint(new Vector2(localPoint.x * cam.pixelWidth, localPoint.y * cam.pixelHeight))));
                        }
                    }

                }
            }

            foreach (var txtmsh in Rndrs)
            {

                var T = txtmsh.GetComponent<MeshRenderer>();
                if (!txtmsh.gameObject.activeInHierarchy || !T.enabled)
                    continue;
                var rect = GUIRectWithRenderer(T);
                foreach (var mp in mousePos)
                    if (rect.Contains(mp))
                        strs.Add(txtmsh.text);


            }

#if TextMeshPro
            foreach (var txtmsh in tmpRndrs)
            {
                var T = txtmsh.GetComponent<MeshRenderer>();
                if (!txtmsh.gameObject.activeInHierarchy || !T.enabled)
                    continue;
                var rect = GUIRectWithRenderer(T);
                foreach (var mp in mousePos)
                    if (rect.Contains(mp))
                        strs.Add(txtmsh.text);


            }
#endif



        }

        public static Rect GUIRectWithRenderer(MeshRenderer go)
        {
            Vector3 cen = go.bounds.center;
            Vector3 ext = go.bounds.extents;
            Vector2[] extentPoints = new Vector2[8];


            extentPoints[0] = Camera.main.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z - ext.z));
            extentPoints[1] = Camera.main.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z - ext.z));
            extentPoints[2] = Camera.main.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y - ext.y, cen.z + ext.z));
            extentPoints[3] = Camera.main.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y - ext.y, cen.z + ext.z));
            extentPoints[4] = Camera.main.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z - ext.z));
            extentPoints[5] = Camera.main.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z - ext.z));
            extentPoints[6] = Camera.main.WorldToScreenPoint(new Vector3(cen.x - ext.x, cen.y + ext.y, cen.z + ext.z));
            extentPoints[7] = Camera.main.WorldToScreenPoint(new Vector3(cen.x + ext.x, cen.y + ext.y, cen.z + ext.z));

            Vector2 min = extentPoints[0];
            Vector2 max = extentPoints[0];
            foreach (Vector2 v in extentPoints)
            {

                min = Vector2.Min(min, v);
                max = Vector2.Max(max, v);
            }


            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        void GetAllRaycasters()
        {
            //Fetch the Raycaster from the GameObject (the Canvas)
            var OurRCs = GetComponentsInChildren<GraphicRaycaster>();
            var tmps = FindObjectsOfType<GraphicRaycaster>();
            m_Raycaster_ = new List<GraphicRaycaster>();
            foreach (var tmp in tmps)
            {
                bool selfCheck = false;
                foreach (var OurRC in OurRCs)
                    if (tmp == OurRC)
                        selfCheck = true;

                if (!selfCheck)
                    m_Raycaster_.Add(tmp);

            }
            m_raycaster_state_ = new bool[m_Raycaster_.Count];
        }

        void PauseAllAudio()
        {
            AudSources = FindObjectsOfType<AudioSource>();
            AudToResume = new bool[AudSources.Length];
            for (int i = 0; i < AudSources.Length; i++)
            {
                AudToResume[i] = AudSources[i].isPlaying;
                if (AudSources[i].isPlaying)
                    AudSources[i].Pause();
            }

        }

        void ResumeAllAudio()
        {
            for (int i = 0; i < AudSources.Length; i++)
                if (AudToResume[i])
                    AudSources[i].Play();

        }
    }

}