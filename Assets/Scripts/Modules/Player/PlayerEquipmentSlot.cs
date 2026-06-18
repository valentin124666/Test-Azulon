using Items;
using UnityEngine;

namespace Modules.Player
{
    public class PlayerEquipmentSlot : MonoBehaviour
    {
        [SerializeField] private GameObject equipmentSlot;
        [SerializeField] private EquipmentSlot equipmentSlotType;

        private GameObject _equipment;
        private ItemSlot _itemSlot;

        public EquipmentSlot EquipmentSlotType => equipmentSlotType;

        public bool IsEquipped => _itemSlot != null;

        public void EquipSlot(ItemSlot itemSlot)
        {
            _itemSlot = itemSlot;
            Equip(_itemSlot.Equipment);
        }

        public void UnequipSlot()
        {
            Unequip();
            _itemSlot = null;
        }

        private void Equip(GameObject equipment)
        {
            if (_equipment != null)
            {
                Unequip();
            }

            _equipment = Instantiate(_itemSlot.Equipment, equipmentSlot.transform);

            _equipment.transform.position = equipmentSlot.transform.position;
            _equipment.transform.rotation = equipmentSlot.transform.rotation;
        }

        private void Unequip()
        {
            if (_equipment != null)
            {
                Destroy(_equipment);
            }

            _equipment = null;
        }
    }
}