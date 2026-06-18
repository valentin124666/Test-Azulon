using UnityEngine;

namespace Items
{
    public class ItemSlot
    {
        public GameObject Equipment;
        public EquipmentSlot Slot = 0;

        public int MaxDurability = -1;
        public int MaxCharges = -1;
    }
}