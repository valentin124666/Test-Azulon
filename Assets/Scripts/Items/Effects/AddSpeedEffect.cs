using Modules.Player.Modules;
using Modules.Player.Modules.Comands;
using UnityEngine;

namespace Items.Effects
{
    [CreateAssetMenu(menuName = "Custom menu/Item/Effect/AddSpeedEffect")]
    public class AddSpeedEffect : ItemEffectBase
    {
        public float Amount;

        public override IPlayerCommand Apply()
        {
           return new PCMAddSpeed(Amount,-1);
        }

        public override IPlayerCommand Remove()
        {
            return new PCMAddSpeed(-Amount,-1);
        }
    }
}
