using System;
using Projectile;
using UnitAction;
using UnityEngine;

namespace Utils
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
            SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
            GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        }

        private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake(2);
        }

        private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake(5);
        }
        
        private void ShootAction_OnAnyShoot(object sender, OnShootEventArgs e)
        {
           ScreenShake.Instance.Shake();
           
        }
    }
}