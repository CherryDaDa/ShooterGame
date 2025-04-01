using Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework.Asset
{
  public class AssetMgr : MonoSingleton<AssetMgr>
  {
    private static string m_StreamingAssetsPath = Application.streamingAssetsPath;
    private static string m_PersistentDataPath = Application.persistentDataPath;
    private static string m_DataPath = Application.dataPath;
    private static Dictionary<string, AssetBundle> m_AssetBundleCache = new Dictionary<string, AssetBundle>();
    private static AssetBundleManifest m_AssetBundleManifest;

    public static event Action<float> onLoadingEvent;

    public static event Action onCompletedEvent;

    public static T LoadAsset<T>(string path, string abPath = null) where T : UnityEngine.Object => string.IsNullOrEmpty(abPath) ? Resources.Load<T>(path) : AssetMgr.LoadAssetBundle(abPath).LoadAsset<T>(path);

    public static void LoadAssetAsync<T>(string path, Action<T> loaded = null) where T : UnityEngine.Object
    {
      
      ResourceRequest request = Resources.LoadAsync<T>(path);
      request.completed += (Action<AsyncOperation>) (operation =>
      {
        if (request.asset == (UnityEngine.Object) null)
        {
          Debug.LogError((object) ("资源加载失败：" + path));
        }
        else
        {
          if (!(operation is ResourceRequest resourceRequest2))
            return;
          Action<T> action = loaded;
          if (action == null)
            return;
          action(resourceRequest2.asset as T);
        }
      });
    }

    public static T LoadInstantiateAsset<T>(string path, string abPath = null) where T : UnityEngine.Object => UnityEngine.Object.Instantiate<T>(Resources.Load<T>(path));

    public static T[] LoadAllAssets<T>(string abPath) where T : UnityEngine.Object
    {
      AssetBundle assetBundle = AssetMgr.LoadAssetBundle(abPath);
      return !(bool) (UnityEngine.Object) assetBundle ? Resources.LoadAll<T>(abPath) : assetBundle.LoadAllAssets<T>();
    }

    public static void UnloadAsset(UnityEngine.Object asset)
    {
      if (!(bool) asset)
        return;
      Resources.UnloadAsset(asset);
    }

    public static void HasBundle()
    {
    }

    public static AssetBundle LoadAssetBundle(string path)
    {
      AssetBundle assetBundle1;
      if (AssetMgr.m_AssetBundleCache.TryGetValue(path, out assetBundle1))
        return assetBundle1;
      if ((bool) (UnityEngine.Object) AssetMgr.m_AssetBundleManifest)
      {
        string[] allDependencies = AssetMgr.m_AssetBundleManifest.GetAllDependencies(Path.Combine(AssetMgr.m_StreamingAssetsPath, path));
        int length = allDependencies.Length;
        if (length > 0)
        {
          for (int index = 0; index < length; ++index)
          {
            string str = allDependencies[index];
            if (!AssetMgr.m_AssetBundleCache.ContainsKey(str))
            {
              AssetBundle assetBundle2 = AssetMgr.LoadAssetBundle(str);
              AssetMgr.m_AssetBundleCache.Add(str, assetBundle2);
            }
          }
        }
      }
      try
      {
        Debug.Log((object) ("Attempting to load AssetBundle from path: " + path));
        if (!File.Exists(path))
        {
          Debug.Log((object) ("File does not exist at path: " + path));
        }
        else
        {
          assetBundle1 = AssetBundle.LoadFromFile(path);
          if ((UnityEngine.Object) assetBundle1 == (UnityEngine.Object) null)
            Debug.Log((object) ("Failed to load AssetBundle from path: " + path));
          else
            AssetMgr.m_AssetBundleCache.Add(path, assetBundle1);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) string.Format("Exception occurred while loading AssetBundle from path: {0}. Exception: {1}", (object) path, (object) ex));
        throw;
      }
      return assetBundle1;
    }

    public static void UnloadAllCacheAssetBundle(bool unloadAllLoadedObjects = false)
    {
      foreach (AssetBundle assetBundle in AssetMgr.m_AssetBundleCache.Values)
        assetBundle.Unload(unloadAllLoadedObjects);
      AssetMgr.m_AssetBundleCache.Clear();
    }

    public static void UnloadCacheAssetBundle(string bundlePath, bool unloadAllLoadedObjects = false)
    {
      AssetBundle assetBundle;
      if (!AssetMgr.m_AssetBundleCache.TryGetValue(bundlePath, out assetBundle))
        return;
      assetBundle.Unload(unloadAllLoadedObjects);
    }
  }
}
