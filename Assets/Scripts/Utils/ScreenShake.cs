using System;
using Cinemachine;
using Projectile;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class ScreenShake : Singleton<ScreenShake>
    {
        private CinemachineImpulseSource cinemachineImpulseSource;

        private void OnValidate()
        {
            cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }


        public void Shake(int intensity = 1)
        {
            cinemachineImpulseSource.GenerateImpulse(intensity);
        }
    }
}