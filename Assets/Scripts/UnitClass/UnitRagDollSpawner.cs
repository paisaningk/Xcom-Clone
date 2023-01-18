using System;
using UnityEngine;

namespace UnitClass
{
    public class UnitRagDollSpawner : MonoBehaviour
    {
        [SerializeField] private UnitRagdoll ragBollPrefab;
        [SerializeField] private Transform  originalRootBone;

        private HealthSystem healthSystem;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            
            healthSystem.OnDead += HealthSystemOnDead;
        }

        private void HealthSystemOnDead(object sender, EventArgs e)
        {
            var parent = transform;
            var ragDoll  =Instantiate(ragBollPrefab, parent.position, parent.rotation);
            ragDoll.Setup(originalRootBone);
        }
    }
}
