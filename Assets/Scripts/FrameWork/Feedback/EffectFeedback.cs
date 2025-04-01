using System;
using Framework.Attributes;
using Framework.Component;
using UnityEngine;

namespace Framework.Feedback
{
    /// <summary>
    /// 反馈效果
    /// </summary>
    public class EffectFeedback : MonoBehaviour
    {
        public bool isParticleSystem = true;
        
        [IsShow("isParticleSystem == true")]
        public bool isPrefab;
        [IsShow("isParticleSystem == true;isPrefab == true")]
        public ParticleSystem effectPrefab;
        [IsShow("isParticleSystem == true;isPrefab == false")]
        public ParticleSystem effectObj;

        private ParticleSystem _particleSystem;

        public void Play()
        {
            TryPlayParticleSystem(true);
        }

        public void Stop()
        {
            TryPlayParticleSystem(false);
        }

        private void TryPlayParticleSystem(bool isPlay)
        {
            if (isPlay)
            {
                if (isParticleSystem)
                {
                    _particleSystem ??= isPrefab ? Instantiate(effectPrefab) : effectObj;
                    if (isPrefab)
                    {
                        if (effectPrefab)
                        {
                            if (effectPrefab.gameObject.activeSelf)
                            {
                                effectPrefab.gameObject.SetActive(false);
                            }
                        }
                        _particleSystem.gameObject.SetActive(true);
                        _particleSystem.transform.position = transform.position;
                    }

                    if (isPrefab)
                    {
                        var particleSystemMain = _particleSystem.main;
                        particleSystemMain.stopAction = ParticleSystemStopAction.Destroy;
                    }
                    
                    _particleSystem.Play();
                }
            }
            else
            {
                if (_particleSystem)
                {
                    _particleSystem.Stop();
                    // Debug.Log("停止特效");
                    _particleSystem = null;
                }
            }
        }
    }
}