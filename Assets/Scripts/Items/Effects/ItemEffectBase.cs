using Modules.Player.Modules;
using UnityEngine;

namespace Items.Effects
{
    public abstract class ItemEffectBase : ScriptableObject
    {
        public abstract IPlayerCommand Apply();     
        public abstract IPlayerCommand Remove();
    }

}


