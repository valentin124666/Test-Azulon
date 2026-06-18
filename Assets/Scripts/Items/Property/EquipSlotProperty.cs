using Modules.Player;
using UnityEngine;

namespace Items.Property
{
    [CreateAssetMenu(menuName = "Custom menu/Item/EquippableProperty/EquipSlotProperty")]
    public class EquipSlotProperty : EquippableProperty
    {
        public EquipmentSlot Slot;
        public GameObject PrefabEquip;
        public override void Equip(ItemSlot itemSlot)
        {
            itemSlot.Equipment = PrefabEquip;
            itemSlot.Slot = Slot;
        }
    }
}
