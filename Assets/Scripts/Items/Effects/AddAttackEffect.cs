using Modules.Player.Modules;
using Modules.Player.Modules.Comands;
using UnityEngine;

namespace Items.Effects
{
    [CreateAssetMenu(menuName = "Custom menu/Item/Effect/AddAttackEffect")]
    public class AddAttackEffect : ItemEffectBase
    {
        public int Amount;
        public float Duration;

        public override IPlayerCommand Apply()
        {
           return new PCMAddAttack(Amount, Duration);
        }

        public override IPlayerCommand Remove()
        {       return new PCMAddAttack(-Amount, 0);
        }
    }
}
