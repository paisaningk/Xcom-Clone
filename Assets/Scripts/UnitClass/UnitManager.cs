using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UnitClass
{
    public class UnitManager : Singleton<UnitManager>
    {
        [SerializeField]private List<Unit> unitList;
        [SerializeField]private List<Unit> friendlyUnitList;
        [SerializeField]private List<Unit> enemyUnitList;

        private void Awake()
        {
            unitList = new List<Unit>();
            friendlyUnitList = new List<Unit>();
            enemyUnitList = new List<Unit>();
        }

        private void Start()
        {
            Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
            Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        }

        private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
        {
            var unit = sender as Unit;
            
            unitList.Add(unit);
                        
            #if UNITY_EDITOR
            Debug.Log(unit + "have spawned");
            #endif
            
            if (unit.IsEnemy())
            {
                enemyUnitList.Add(unit);
            }
            else
            {
                friendlyUnitList.Add(unit);
            }
        }

        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
        {
            var unit = sender as Unit;
            
            unitList.Remove(unit);

            #if UNITY_EDITOR
            Debug.Log(unit + "have die");
            #endif
            
            if (unit.IsEnemy())
            {
                enemyUnitList.Remove(unit);;
            }
            else
            {
                friendlyUnitList.Remove(unit);;
            }
        }

        public List<Unit> GetUnitList()
        {
            return unitList;
        }
        
        public List<Unit> GetFriendlyUnitList()
        {
            return friendlyUnitList;
        }
        
        public List<Unit> GetEnemyUnitList()
        {
            return enemyUnitList;
        }
    }
}
