using System;

namespace Items
{
    public class InventoryItem
    {
        public ItemData Data;
        public string InstanceId;

        public InventoryItem(ItemData data, string instanceId)
        {
            Data = data;
            InstanceId = instanceId;
        }
    }
}