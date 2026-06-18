using System;

namespace Settings
{
    public class Enumerators
    {
        public enum NamePrefabAddressable : short
        {

        }

        public enum AppState
        {
            Unknown,

            InMainMenu,
            InGameScene,
        }
        
        public enum LevelState
        {
            InReadyToStartLevel,
            InLevelToPlay,
            InWinLevel,
            InLosLevel
        }
        public enum VibrationStrength
        {
            Weak,
            Medium,
            Strong
        }
        // public enum SoundName
        // {
        //     Update,
        //     LateUpdate,
        //     FixedUpdate
        // }
    }
}