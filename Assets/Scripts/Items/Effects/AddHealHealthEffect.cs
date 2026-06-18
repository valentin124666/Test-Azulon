using Modules.Player.Modules;
using Modules.Player.Modules.Comands;
using UnityEngine;

namespace Items.Effects
{
    [CreateAssetMenu(menuName = "Custom menu/Item/Effect/AddHealHealthEffect")]
    public class AddHealHealthEffect : ItemEffectBase
    {
        public int Amount;

        public override IPlayerCommand Apply()
        {
           return new PCMHeal(Amount);
        }

        public override IPlayerCommand Remove()
        {
            return new PCMHeal(-Amount);
        }
    }
} 
