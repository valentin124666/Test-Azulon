using UnityEngine;

namespace Items
{
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private ItemData _itemData;

        public ItemData GetItemData()
        {
            Destroy(gameObject);
            return _itemData;
        }
    }
}