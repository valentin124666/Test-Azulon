using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class BackpackItemView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text quantityInStack;
        [SerializeField] private TMP_Text numberOfUses;
        [SerializeField] private Button use;

        public event Action<string> UseItem;

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int CurrentCharges { get; private set; }
        public int CountItem { get; private set; }
        public Sprite Icon { get; private set; }

        public string IdSlot { get; private set; }

        private void Start()
        {
            use.onClick.AddListener(Use);
        }

        private void Use()
        {
            UseItem?.Invoke(IdSlot);
        }

        public void SetData(string name, string description, Sprite icon, string idSlot, int currentCharges,
            int countItem)
        {
            Name = name;
            Description = description;
            Icon = icon;
            IdSlot = idSlot;
            CurrentCharges = currentCharges;
            CountItem = countItem;

            this.icon.sprite = Icon;
            this.name.text = name;
            this.description.text = description;
            if (countItem > 1)
            {
                quantityInStack.transform.parent.gameObject.SetActive(true);
                quantityInStack.text = countItem.ToString();
            }
            else
            {
                quantityInStack.transform.parent.gameObject.SetActive(false);
            }

            if (currentCharges > 1)
            {
                numberOfUses.transform.parent.gameObject.SetActive(true);
                numberOfUses.text = currentCharges.ToString();
            }
            else
            {
                numberOfUses.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}