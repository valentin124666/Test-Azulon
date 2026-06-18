using Modules.Player;
using UnityEngine;

namespace Items.Property
{
    public abstract class EquippableProperty : ItemPropertyBase
    {
        public abstract void Equip(ItemSlot itemSlot);
    }

}
