using System.Collections.Generic;

using Items;


namespace Player.Commands
{
    public abstract class BasePlayerEventMassage 
    {

    }
    
    public class PEMStartAction : BasePlayerEventMassage
    {
        public PEMStartAction()
        {
        }
    }  
    public class PEMAddItem : BasePlayerEventMassage
    {
        public ItemData  ItemData;
        public PEMAddItem(ItemData itemData)
        {
            ItemData = itemData;
        }
    }  
}