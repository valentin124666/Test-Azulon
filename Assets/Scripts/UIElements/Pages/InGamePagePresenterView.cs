using System;
using System.Collections.Generic;
using Core;
using Managers.Controller;
using Modules.Player.Modules;
using Services;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace UIElements.Pages
{
    [OverrideTypes(typeof(InGamePagePresenter), typeof(InGamePagePresenterView))]
    public class InGamePagePresenterView : PagePresentersView
    {
        [SerializeField] private TMP_Text health;
        [SerializeField] private TMP_Text attack;
        [SerializeField] private TMP_Text defense;
        [SerializeField] private TMP_Text xpBoost;
        [SerializeField] private TMP_Text maxSpeed;
        
        [SerializeField] private Button _backpack;

        [Inject]
        private void Init(DiContainer container)
        {
        }

        public void SetStatsView(string healthStats,
            string attackStats, string defenseStats, string xpBoostStats, string maxSpeedStats)
        {
            health.text = healthStats;
            attack.text = attackStats;
            defense.text = defenseStats;
            xpBoost.text = xpBoostStats;
            maxSpeed.text = maxSpeedStats;
        }

        public void ConnectActivationBackpack(UnityAction backpackAction)
        {
            _backpack.onClick.AddListener(backpackAction);
        }   
        public void UnconnectActivationBackpack(UnityAction backpackAction)
        {
            _backpack.onClick.RemoveListener(backpackAction);
        }

        public void UpdateStatView(TypeStatsView typeStatsView, string stats)
        {
            switch (typeStatsView)
            {
                case TypeStatsView.health:
                    health.text = stats;
                    break;
                case TypeStatsView.attack:
                    attack.text = stats;
                    break;
                case TypeStatsView.defense:
                    defense.text = stats;
                    break;
                case TypeStatsView.xpBoost:
                    xpBoost.text = stats;
                    break;
                case TypeStatsView.speed:
                    maxSpeed.text = stats;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeStatsView), typeStatsView, null);
            }
        }
    }
}