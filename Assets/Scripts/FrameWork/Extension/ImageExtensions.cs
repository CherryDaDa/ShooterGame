using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Framework.Extension
{
    public static class ImageExtensions
    {
        public static async void LoadRemoteTexture(this Image self, string url, Action onLoaded = null)
        {
            // 创建 UnityWebRequest 来下载图片
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                var asyncOp = www.SendWebRequest();

                while (!asyncOp.isDone)
                {
                    await Task.Yield();
                }

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error: {www.error}");
                }
                else
                {
                    // 获取下载的图片并应用到目标 Renderer
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    self.sprite = Sprite.Create(
                        texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    onLoaded?.Invoke();
                }
            }
        }
    }
}