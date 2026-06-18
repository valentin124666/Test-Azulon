using Core;

using UnityEngine;

namespace Level
{
    [PrefabInfo("Levels/")]
    public class LevelPresenterView : SimplePresenterView<LevelPresenter,LevelPresenterView>
    {
        [SerializeField] private Transform startLevel;
        
        public Transform StartLevel => startLevel;
        
        public override void Init()
        {
           
        }
    }
}
