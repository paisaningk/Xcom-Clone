using TMPro;
using UnitAction;
using UnitClass;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIAction
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        [SerializeField] private GameObject selectedGameObject;
        private BaseAction baseAction;
        
        public void SetBaseAction(BaseAction baseAction)
        {
            this.baseAction = baseAction;
            text.text = baseAction.GetActionName().ToUpper();
            
            button.onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(baseAction));
        }

        public void UpdateSelectedVisual()
        {
            selectedGameObject.SetActive(baseAction ==  UnitActionSystem.Instance.GetSelectAction());
        }
    }
}