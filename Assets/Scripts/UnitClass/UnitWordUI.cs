using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnitClass
{
   public class UnitWordUI : MonoBehaviour
   {
      [SerializeField] private TMP_Text actionPointsText;
      [SerializeField] private Unit unit;
      [SerializeField] private Image healthBarImage;
      [SerializeField] private HealthSystem healthSystem;
      private void Start()
      {
         Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
         healthSystem.OnDamaged += HealthSystem_OnDamaged;
         UpdateActionPointsText();
         UpdateHealthBar();
      }

      private void HealthSystem_OnDamaged(object sender, EventArgs e)
      {
         UpdateHealthBar();
      }

      private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
      {
         UpdateActionPointsText();
      }

      private void UpdateActionPointsText()
      {
         actionPointsText.SetText(unit.GetActionPoint().ToString());
      }

      private void UpdateHealthBar()
      {
         healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
      }
   }
}
