using System;
using System.Collections;
using System.Collections.Generic;
using FBXPool;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Custom menu/Data/FBXPoolData")]
    public class FBXPoolData : ScriptableObject
    {
        public List<FBXPoolPrefab> FBXPoolPrefabs;

    }
    [Serializable]
    public class FBXPoolPrefab
    {
        public string viewType;
        public string locationSuffix;
        public int numberOfExamples;

        public FBXPoolItem FbxPrefab;  
    }
}