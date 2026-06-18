using Modules.Player.Modules;
using Modules.Player.Modules.Comands;
using UnityEngine;

namespace Items.Effects
{
    [CreateAssetMenu(menuName = "Custom menu/Item/Effect/AddXpEffect")]
    public class AddXpEffect : ItemEffectBase
    {
        public int Amount;

        public override IPlayerCommand Apply()
        {
           return new PCMAddXPBoost(Amount,-1);
        }
        public override IPlayerCommand Remove()
        {
            return new PCMAddXPBoost(-Amount,-1);
        }
    }
}  
