using UnityEngine;

namespace Items.Property
{
    [CreateAssetMenu(menuName = "Custom menu/Item/ConsumableProperty/StackableProperty")]
    public class StackableProperty : ConsumableProperty
    {
        public int MaxStack;
    }
}  
