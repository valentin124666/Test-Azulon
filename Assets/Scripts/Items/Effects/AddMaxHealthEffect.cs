using Modules.Player.Modules;
using Modules.Player.Modules.Comands;
using UnityEngine;

namespace Items.Effects
{
    [CreateAssetMenu(menuName = "Custom menu/Item/Effect/AddMaxHealthEffect")]
    public class AddMaxHealthEffect : ItemEffectBase
    {
        public int Amount;

        public override IPlayerCommand Apply()
        {
           return new PCMAddMaxHealth(Amount);
        }

        public override IPlayerCommand Remove()
        {
            return new PCMAddMaxHealth(-Amount);
        }
    }
} 
