using Modules.Player;
using UnityEngine;

namespace Items.Property
{
    [CreateAssetMenu(menuName = "Custom menu/Item/EquippableProperty/ChargeProperty")]
    public class ChargeProperty : EquippableProperty
    {
        public int MaxCharges;

        public override void Equip(ItemSlot itemSlot)
        {
            itemSlot.MaxCharges = MaxCharges;
        }
    }
}
