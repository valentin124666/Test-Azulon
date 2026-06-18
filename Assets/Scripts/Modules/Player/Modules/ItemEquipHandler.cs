using System.Collections.Generic;
using Items;
using Items.Property;
using Managers.Controller;
using UnityEngine;

namespace Modules.Player.Modules
{
    public class ItemEquipHandler 
    {
        private PlayerEquipmentSlot[] _equipmentSlots;
        
        private Dictionary<PlayerEquipmentSlot,ItemData> _equipment ;
        private PlayerController _playerController;
        
        public ItemEquipHandler(PlayerController playerController)
        {
            _equipment = new Dictionary<PlayerEquipmentSlot,ItemData>();
            _playerController = playerController;
        }
        public void SetEquipSlot(IEquipmentSlot equipmentSlot)
        {
            _equipmentSlots = equipmentSlot.GetEquipmentSlot();
        }

        public void Equip(ItemData itemData)
        {
            if (itemData.InteractionType != ItemInteractionType.Equip )
            {
                return;
            }

            var equipSlotProperty = itemData.EquipSlotProperty;
            
            if (equipSlotProperty == null )
            {
                return;
            }

            PlayerEquipmentSlot equipmentSlots = null;
            
            foreach (var slot in _equipmentSlots)
            {
                if (slot.EquipmentSlotType != equipSlotProperty.Slot) continue;
                
                equipmentSlots = slot;
                if (!equipmentSlots.IsEquipped)
                {
                    break;
                }
            }
            
            if (equipmentSlots == null)
            {
                return;
            }

            if (equipmentSlots.IsEquipped)
            {
                equipmentSlots.UnequipSlot();
                _playerController.CollectItem(_equipment[equipmentSlots]);
                _playerController.RemoveEffects(_equipment[equipmentSlots].ItemEffects);
            }
            
            var itemProperties = itemData.GetProperty();
            
            ItemSlot itemSlot = new ItemSlot();
            equipSlotProperty.Equip(itemSlot);
            
            if (itemProperties.Length < 1)
            {
                foreach (var item in itemProperties)
                {
                    ((EquippableProperty)item).Equip(itemSlot);
                }
            }
            equipmentSlots.EquipSlot(itemSlot);
            
            _playerController.AddEffects(itemData.ItemEffects);
            
            _equipment[equipmentSlots] = itemData;
        }
    }
}
