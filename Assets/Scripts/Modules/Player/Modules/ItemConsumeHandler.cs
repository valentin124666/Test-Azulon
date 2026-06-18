using Items;
using Managers.Controller;

namespace Modules.Player.Modules
{
    public class ItemConsumeHandler
    {
        private PlayerController _playerController;
        
        public ItemConsumeHandler(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void UseItem(ItemData itemData)
        {
            if (itemData.InteractionType != ItemInteractionType.Consume )
            {
                return;
            }
            
            _playerController.AddEffects(itemData.ItemEffects);
        }
    }
}
