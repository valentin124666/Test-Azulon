using Modules.Player.Modules;
using Modules.Player.Modules.Comands;
using UnityEngine;

namespace Items.Effects
{
    [CreateAssetMenu(menuName = "Custom menu/Item/Effect/AddArmorEffect")]
    public class AddArmorEffect : ItemEffectBase
    {
        public int Amount;

        public override IPlayerCommand Apply()
        {
           return new PCMAddArmor(Amount);
        }

        public override IPlayerCommand Remove()
        {
            return new PCMAddArmor(-Amount);
        }
    }
}  
