using System.Collections;
using UnityEngine;
using System.Text;

namespace Framework.Debugger
{
    public class DebugInfo : MonoBehaviour
    {
        public float updateFrequency = 1.0f; // 更新频率，例如1秒
        public bool enableManualGC = true; // 是否启用手动GC
        public float gcInterval = 30.0f; // 手动GC的时间间隔，例如30秒
        public bool defaultDisplay = true;

        private string infoText = "";
        // private LocationInfo currentLocation;
        private GUIStyle guiStyle;
        private Rect backgroundRect;
        private Rect textRect;
        private Color backgroundColor;
        private StringBuilder sb;
        // private bool isDebugInfoVisible = false; // 是否显示调试信息

        private void Start()
        {
            InitGUI();
            StartCoroutine(InitializeLocationService());
            if (enableManualGC)
            {
                StartCoroutine(ManualGC());
            }
        }

        private IEnumerator InitializeLocationService()
        {
            UnityEngine.Input.location.Start();

            int maxWait = 20;
            while (UnityEngine.Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                infoText = "Location service timed out";
                yield break;
            }

            if (UnityEngine.Input.location.status == LocationServiceStatus.Failed)
            {
                infoText = "Unable to determine device location";
                yield break;
            }

            // 开始循环更新信息
            StartCoroutine(UpdateInfo());
        }

        private void InitGUI()
        {
            guiStyle = new GUIStyle
            {
                fontSize = 24,
                normal = { textColor = Color.white }
            };

            // 设置背景矩形和文本矩形
            backgroundRect = new Rect(10, 10, 1000, 500);
            textRect = new Rect(10, 10, 1000, 500);

            // 设置背景颜色和透明度
            backgroundColor = new Color(0, 0, 0, 0.5f); // 黑色半透明

            // 初始化 StringBuilder
            sb = new StringBuilder();
        }

        private IEnumerator UpdateInfo()
        {
            while (true)
            {
                float fps = 1.0f / Time.unscaledDeltaTime;
                float memUsage = System.GC.GetTotalMemory(false) / (1024.0f * 1024.0f);

                // currentLocation = UnityEngine.Input.location.lastData;

                sb.Clear();
                sb.AppendFormat("FPS: {0:0.} \n", fps);
                sb.AppendFormat("Memory: {0:0.00} MB\n", memUsage);
                // sb.AppendFormat("Device: {0}\n", SystemInfo.deviceModel);
                // sb.AppendFormat("OS: {0}\n", SystemInfo.operatingSystem);
                // sb.AppendFormat("Location: {0}, {1}\n", currentLocation.latitude, currentLocation.longitude);
                // sb.AppendFormat("Time: {0}", System.DateTime.Now.ToString());

                infoText = sb.ToString();

                yield return new WaitForSeconds(updateFrequency);
            }
        }

        private void Update()
        {
            // 检测组合键（例如Ctrl+D）
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.F2))
            {
                defaultDisplay = !defaultDisplay;
            }
        }

        private void OnGUI()
        {
            if (defaultDisplay)
            {
                // 设置背景颜色和透明度
                GUI.backgroundColor = backgroundColor;

                // 绘制半透明背景
                GUI.Box(backgroundRect, GUIContent.none);

                // 绘制文本信息
                GUI.Label(textRect, infoText, guiStyle);
            }
        }

        private IEnumerator ManualGC()
        {
            while (true)
            {
                yield return new WaitForSeconds(gcInterval);
                System.GC.Collect();
                Debug.Log("Manual GC executed.");
            }
        }

        // 手动触发 GC 的方法
        public void TriggerGC()
        {
            System.GC.Collect();
            Debug.Log("Manual GC triggered.");
        }
    }
}
