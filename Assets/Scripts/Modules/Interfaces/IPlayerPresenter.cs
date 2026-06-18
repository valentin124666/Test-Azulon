using System;
using Modules.Player.Commands;
using Player.Commands;
using UnityEngine;

namespace Modules.Interfaces
{
   public interface IPlayerPresenter 
   {
      event Action<BasePlayerPresenterEvent> PlayerPresenterEvent;
      BasePlayerPresenterDataRequests RetrievePlayerInfo(BasePlayerPresenterDataCommands playerDataCommands);
      void SetActive(bool active);
      void HandleEventMassage(BasePlayerEventMassage eventMassage);
      void ResetPos();
      void OnDestroy();
   }
}
