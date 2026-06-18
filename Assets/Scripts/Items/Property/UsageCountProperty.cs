using UnityEngine;

namespace Items.Property
{
    [CreateAssetMenu(menuName = "Custom menu/Item/ConsumableProperty/UsageCountProperty")]
    public class UsageCountProperty : ConsumableProperty
    {
        public int MaxCharges;
    }
}
