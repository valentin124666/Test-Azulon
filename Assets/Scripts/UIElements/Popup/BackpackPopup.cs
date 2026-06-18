using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Cysharp.Threading.Tasks;
using Managers;
using Managers.Controller;
using Managers.Interfaces;
using Modules.Player;
using Modules.Player.Commands;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Zenject;

namespace UIElements.Popup
{
    public class BackpackPopup : MonoBehaviour, IUIPopup
    {
        [SerializeField] private Transform content;
        [SerializeField] private AssetReference backpackItemView;

        [SerializeField] private List<BackpackItemView> backpackItems;
        [SerializeField] private Button backButton;

        private ResourceLoader _resourceLoader;
        private PlayerController _playerController;

        private BackpackItemView backpackItemPrefab;
        private AsyncOperationHandle<GameObject> _prefabHandle;

        public bool IsActive => gameObject.activeSelf;

        [Inject]
        public void Init(DiContainer container)
        {
            _resourceLoader = container.Resolve<ResourceLoader>();
            _playerController = container.Resolve<GameplayManager>().GetController<PlayerController>();

            LoadPrefab().Forget();
            backButton.onClick.AddListener(Hide);
        }

        private async UniTask LoadPrefab()
        {
            var prefab = await backpackItemView.LoadAssetAsync<GameObject>();
            backpackItemPrefab = prefab.GetComponent<BackpackItemView>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            var backpack = ((PDRPlayerBackpack)_playerController.RetrievePlayerInfo(new PDCGetPlayerBackpack()))
                .PlayerBackpack;
            
            backpack.AddSlot += CreateSlot;
            backpack.UpdateSlot += UpdateSlotView;
            backpack.RemoveSlot += RemoveSlotView;
            
            CreateItemView(backpack);
        }

        public void Show(Action callback)
        {
            gameObject.SetActive(true);
            var backpack = ((PDRPlayerBackpack)_playerController.RetrievePlayerInfo(new PDCGetPlayerBackpack()))
                .PlayerBackpack;
            
            backpack.AddSlot += CreateSlot;
            backpack.UpdateSlot += UpdateSlotView;
            backpack.RemoveSlot += RemoveSlotView;
            
            CreateItemView(backpack);
        }

        public void Hide()
        {
            var backpack = ((PDRPlayerBackpack)_playerController
                    .RetrievePlayerInfo(new PDCGetPlayerBackpack()))
                .PlayerBackpack;

            backpack.AddSlot -= CreateSlot;
            backpack.UpdateSlot -= UpdateSlotView;
            backpack.RemoveSlot -= RemoveSlotView;

            gameObject.SetActive(false);
        }
        
        private void CreateSlot(string slotId)
        {
            Debug.Log($"Creating slot {slotId}");
            var backpack = ((PDRPlayerBackpack)_playerController
                    .RetrievePlayerInfo(new PDCGetPlayerBackpack()))
                .PlayerBackpack;

            var slot = backpack.GetInfoSlots()
                .FirstOrDefault(x => x.IdSlot == slotId);

            if (slot == null)
                return;

            var view = Instantiate(backpackItemPrefab, content);

            view.SetData(
                slot.ItemData.Name,
                slot.ItemData.Description,
                slot.ItemData.Icon,
                slot.IdSlot,
                slot.CurrentCharges,
                slot.CountItem);

            view.UseItem += UseItem;

            backpackItems.Add(view);
        }
        private void UpdateSlotView(string slotId)
        {
            var backpack = ((PDRPlayerBackpack)_playerController
                    .RetrievePlayerInfo(new PDCGetPlayerBackpack()))
                .PlayerBackpack;

            var slot = backpack.GetInfoSlots()
                .FirstOrDefault(x => x.IdSlot == slotId);

            var view = backpackItems
                .FirstOrDefault(x => x.IdSlot == slotId);

            if (slot == null || view == null)
                return;

            view.SetData(
                slot.ItemData.Name,
                slot.ItemData.Description,
                slot.ItemData.Icon,
                slot.IdSlot,
                slot.CurrentCharges,
                slot.CountItem);
        }
        private void RemoveSlotView(string slotId)
        {
            var view = backpackItems
                .FirstOrDefault(x => x.IdSlot == slotId);

            if (view == null)
                return;

            view.UseItem -= UseItem;

            backpackItems.Remove(view);

            Destroy(view.gameObject);
        }
        private void CreateItemView(PlayerBackpack backpack )
        {
            foreach (var item in backpackItems)
            {
                Destroy(item.gameObject);
            }
            backpackItems.Clear();
            
            var slots = backpack.GetInfoSlots();
            
            foreach (var slot in slots)
            {
                var view = Instantiate(backpackItemPrefab, content);

                view.SetData(
                    slot.ItemData.Name,
                    slot.ItemData.Description,
                    slot.ItemData.Icon,
                    slot.IdSlot,
                    slot.CurrentCharges,
                    slot.CountItem);

                view.UseItem += UseItem;

                backpackItems.Add(view);
            }
        }

        private void UseItem(string idSlot)
        {
            _playerController.UseItem(idSlot);
        }

        private void OnDestroy()
        {
            if (_prefabHandle.IsValid())
            {
                Addressables.Release(_prefabHandle);
            }
        }

        public void Reset()
        {
        }
    }
}