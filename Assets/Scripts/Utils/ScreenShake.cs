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

        public void Start()
        {
            GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        }

        private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
        {
            Shake(5);
        }

        public void Shake(int intensity = 1)
        {
            cinemachineImpulseSource.GenerateImpulse(intensity);
        }
    }
}