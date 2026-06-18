using Modules.Player;
using UnityEngine;

namespace Items.Property
{
    [CreateAssetMenu(menuName = "Custom menu/Item/EquippableProperty/DurabilityProperty")]
    public class DurabilityProperty : EquippableProperty
    {
        public int MaxDurability;
        
        public override void Equip(ItemSlot itemSlot)
        {
            itemSlot.MaxDurability = MaxDurability;
        }
    }
}
