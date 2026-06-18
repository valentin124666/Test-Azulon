using Modules.Interfaces;
using UnityEngine;
using Zenject;

namespace Modules.Player
{
    public class EquipmentSlotView : MonoBehaviour ,IPlayerModule ,IEquipmentSlot
    {
        [SerializeField] private PlayerEquipmentSlot[] equipmentSlots;

        public void Init(DiContainer controller, PlayerPresenterView playerView)
        {
           
        }

        public void Reset()
        {
           
        }

        public PlayerEquipmentSlot[] GetEquipmentSlot()
        {
           return equipmentSlots;
        }
    }
}
