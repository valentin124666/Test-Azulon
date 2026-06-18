using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Property;
using Managers.Controller;
using UnityEngine;

namespace Modules.Player
{
    public class PlayerBackpack
    {
        public PlayerController _playerController;
        private List<BackpackSlot> _backpackSlots;

        public event Action<string> UpdateSlot;
        public event Action<string> AddSlot;
        public event Action<string> RemoveSlot;

        public PlayerBackpack(PlayerController playerController)
        {
            _playerController = playerController;
            _backpackSlots = new List<BackpackSlot>();
        }

        public void AddItem(ItemData itemData)
        {
            var property = itemData.GetProperty();

            var usageCountProperty = property.FirstOrDefault(item => item is UsageCountProperty) as UsageCountProperty;
            var stackableProperty = property.FirstOrDefault(item => item is StackableProperty) as StackableProperty;

            var maxCharges = usageCountProperty != null ? usageCountProperty.MaxCharges : 1;
            BackpackSlot slotItem;
            
            if (stackableProperty != null)
            {
                var stackSlots = _backpackSlots.Where(item => item.ItemData == itemData).ToArray();

                if (stackSlots.Length > 0)
                {
                    foreach (var slot in stackSlots)
                    {

                        if (slot.IsPlaceInStack)
                        {

                            slot.AddToStack(itemData);
                            AddSlot?.Invoke(slot.IdSlot);
                            return;
                        }
                    }
                }

                slotItem = new BackpackSlot(itemData, stackableProperty.MaxStack, maxCharges);
               
                _backpackSlots.Add(slotItem);
                AddSlot?.Invoke(slotItem.IdSlot);
                return;
            }
            
            slotItem = new BackpackSlot(itemData, 1, maxCharges);
            _backpackSlots.Add(slotItem);
            AddSlot?.Invoke(slotItem.IdSlot);
        }

        public ItemData GetItem(string idSlot)
        {
            var slot = _backpackSlots.FirstOrDefault(item => item.IdSlot == idSlot);
            if (slot == null) return null;

            var item = slot.GetItem();
            if (slot.CountItem < 1)
            {
                _backpackSlots.Remove(slot);
                RemoveSlot?.Invoke(idSlot);
            }
            else
            {
                UpdateSlot?.Invoke(idSlot);
            }

            return item;
        }

        public ISlotInfo[] GetInfoSlots()
        {
            return _backpackSlots.ToArray();
        }
    }

    public class BackpackSlot : ISlotInfo
    {
        public ItemData ItemData { get; set; }
        public int CountItem { get; private set; }
        public int MaxQuantity { get; set; }
        public int MaxCharges { get; set; }
        public int CurrentCharges { get; private set; }
        public bool IsPlaceInStack => CountItem < MaxQuantity;
        public string IdSlot { get; private set; }

        public BackpackSlot(ItemData itemData, int maxQuantity = 1, int maxCharges = 1)
        {
            ItemData = itemData;
            CountItem = 1;
            MaxQuantity = maxQuantity;
            CurrentCharges = maxCharges;
            MaxCharges = maxCharges;
            IdSlot = Guid.NewGuid().ToString("N");
        }

        public bool AddToStack(ItemData item)
        {
            if (CountItem >= MaxQuantity)
            {
                return false;
            }

            CountItem += 1;
            return true;
        }

        public ItemData GetItem()
        {
            if (MaxCharges > 1)
            {
                CurrentCharges--;
            }

            if (CountItem > 0)
            {
                CountItem--;
                CurrentCharges = MaxCharges;
            }
            else
            {
                return null;
            }

            return ItemData;
        }
    }

    public interface ISlotInfo
    {
        public ItemData ItemData { get; }
        public int CountItem { get; }
        public int MaxQuantity { get; }
        public int MaxCharges { get; }
        public int CurrentCharges { get; }
        public bool IsPlaceInStack { get; }

        public string IdSlot { get; }
    }
}