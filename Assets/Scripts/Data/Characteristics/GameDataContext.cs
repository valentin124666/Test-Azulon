using System;
using Tools;
using UnityEngine;

namespace Data.Characteristics
{
    public interface IData
    {
        string GetDataJson();
    }

    [Serializable]
    public class SimpleData
    {
    }

    #region Player

    [Serializable]
    [DataName("PlayerDataJson")]
    public class PlayerDataJson : SimpleData
    {
        public int PlayerBackpackSize;
        public int PlayerCurrentCoin;
        public int FameLevel;
        public int NumberNextFameLevel;

        public float Health;
        public int AddedHealth;
        public int Attack;
        public int Defense;

        public float AttackSpeed;
        public float PauseBeforeAttack;
        public float ProtectionTime;
        public float DodgeTime;
        public ushort IdModelShield;
        public ushort IdModelSword;
    }

    [Serializable]
    public class PlayerDataScriptable : SimpleData
    {
        public float PlayerMovementSpeed;
        public float SpeedMovementReceptacleObject;
        public float PauseBetweenEjectionReceptacleObject;
        public float FlightCurvatureReceptacleObject;
        public float AccelerationsProductionAClick;
        public float NPCStunTime;
        public int FactorHealthShield;
    }

    [Serializable]
    [DataInfo(typeof(PlayerDataJson), typeof(PlayerDataScriptable))]
    public class PlayerData : IData
    {
        public float PlayerMovementSpeed;
        public float SpeedMovementReceptacleObject;
        public float PauseBetweenEjectionReceptacleObject;
        public float FlightCurvatureReceptacleObject;
        public float AccelerationsProductionAClick;
        public float NPCStunTime;
        public int FactorHealthShield;

        public int PlayerBackpackSize;
        public int PlayerCurrentCoin;
        public int FameLevel;
        public int NumberNextFameLevel;

        public float Health;
        public int AddedHealth;
        public int Attack;
        public int Defense;

        public ushort IdModelShield;
        public ushort IdModelSword;
        public float AttackSpeed;
        public float PauseBeforeAttack;
        public float ProtectionTime;
        public float DodgeTime;

        public PlayerData(object playerDataJson, object playerDataScriptable)
        {
            var dataScriptable = InternalTools.GetTypeObject<PlayerDataScriptable>(playerDataScriptable);

            PlayerMovementSpeed = dataScriptable.PlayerMovementSpeed;
            SpeedMovementReceptacleObject = dataScriptable.SpeedMovementReceptacleObject;
            PauseBetweenEjectionReceptacleObject = dataScriptable.PauseBetweenEjectionReceptacleObject;
            FlightCurvatureReceptacleObject = dataScriptable.FlightCurvatureReceptacleObject;
            AccelerationsProductionAClick = dataScriptable.AccelerationsProductionAClick;
            NPCStunTime = dataScriptable.NPCStunTime;
            FactorHealthShield = dataScriptable.FactorHealthShield;
            
            var dataJson = InternalTools.GetTypeObject<PlayerDataJson>(playerDataJson);
            PlayerBackpackSize = dataJson.PlayerBackpackSize;
            PlayerCurrentCoin = dataJson.PlayerCurrentCoin;
            FameLevel = dataJson.FameLevel;
            NumberNextFameLevel = dataJson.NumberNextFameLevel;

            Health = dataJson.Health;
            Attack = dataJson.Attack;
            Defense = dataJson.Defense;
            AddedHealth = dataJson.AddedHealth;

            IdModelShield = dataJson.IdModelShield;
            IdModelSword = dataJson.IdModelSword;
            AttackSpeed = dataJson.AttackSpeed;
            PauseBeforeAttack = dataJson.PauseBeforeAttack;
            ProtectionTime = dataJson.ProtectionTime;
            DodgeTime = dataJson.DodgeTime;
        }

        public string GetDataJson()
        {
            var data = new PlayerDataJson();
            data.PlayerBackpackSize = PlayerBackpackSize;
            data.PlayerCurrentCoin = PlayerCurrentCoin;
            data.FameLevel = FameLevel;
            data.NumberNextFameLevel = NumberNextFameLevel;

            data.Health = Health;
            data.Attack = Attack;
            data.Defense = Defense;
            data.AddedHealth = AddedHealth;

            data.IdModelShield = IdModelShield;
            data.IdModelSword = IdModelSword;
            data.AttackSpeed = AttackSpeed;
            data.PauseBeforeAttack = PauseBeforeAttack;
            data.ProtectionTime = ProtectionTime;
            data.DodgeTime = DodgeTime;

            return JsonUtility.ToJson(data);
        }
    }

    #endregion
    
    
}