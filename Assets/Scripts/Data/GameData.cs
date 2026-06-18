using System;
using UnityEngine;
using UnityEngine.Serialization;


namespace Data
{
    [CreateAssetMenu(menuName = "Custom menu/Data/GameData")]
    public class GameData : ScriptableObject
    {
        public ScenarioData ScenarioData;
        public UIScreenData UIScreenData;
        public FBXPoolData FBXPoolData;
        public JoystickData JoystickData;
        public CameraData CameraData;
    }

    [Serializable]
    public class CameraData
    {
        public Vector3 maxOffsetInSide;
    }

    [Serializable]
    public class JoystickData
    {
        public float joystickSensitivity = 1f;
        public float joystickMaxMagnitude = 60;
        public float maxDistanceStick = 40;
    }
}