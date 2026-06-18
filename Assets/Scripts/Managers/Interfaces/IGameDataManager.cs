using System;
using Data.Characteristics;
using UnityEngine;

namespace Managers.Interfaces
{
    public interface IGameDataManager : IInitializableState
    {
        T GetDataScriptable<T>() where T : ScriptableObject;
        T Registration<T>(IDataClient client) where T : IData;
        void SaveDataClients();
        event Action DataDownload;
    }
}
