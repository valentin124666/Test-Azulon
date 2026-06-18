using System;
using Items.Effects;
using Items.Property;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Custom menu/Item/ItemData")]
    public class ItemData : ScriptableObject, IItemDataInfo
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemInteractionType _interactionType;

        [SerializeField] private ConsumableProperty[] _consumableProperties;
        [SerializeField] private EquippableProperty[] _equippableProperty;
        [SerializeField] private ItemEffectBase[] _itemEffects;
        [SerializeField] private EquipSlotProperty _equipSlotProperty;


        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public ItemInteractionType InteractionType => _interactionType;
        public ItemEffectBase[] ItemEffects => _itemEffects;
        public EquipSlotProperty EquipSlotProperty => _equipSlotProperty;

        public ItemPropertyBase[] GetProperty()
        {
            if (_interactionType == ItemInteractionType.Consume)
            {
                return _consumableProperties;
            }
            else
            {
                return _equippableProperty;
            }
        }

        [ContextMenu("GenerateId")]
        public void GenerateId()
        {
            if (!string.IsNullOrEmpty(_id))
            {
                Debug.LogWarning($"ItemData already has ID: {_id}");
                return;
            }

            _id = Guid.NewGuid().ToString("N");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public interface IItemDataInfo
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
    }

    public enum ItemInteractionType
    {
        Consume,
        Equip
    }

    public enum EquipmentSlot
    {
        Unknown,

        LeftHand,
        RightHand,
        Neck,
    }
}