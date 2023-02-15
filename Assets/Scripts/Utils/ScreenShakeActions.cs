using System;
using UnitAction;
using UnityEngine;

namespace Utils
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        }

        private void ShootAction_OnAnyShoot(object sender, OnShootEventArgs e)
        {
           ScreenShake.Instance.Shake(5);
           
        }
    }
}