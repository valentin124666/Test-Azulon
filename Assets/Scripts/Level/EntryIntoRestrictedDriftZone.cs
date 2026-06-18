using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class EntryIntoRestrictedDriftZone : MonoBehaviour
    {
        [SerializeField] private bool _blockRightTurn;

        public DriftZoneData GetDriftZoneData()
        {
            return _blockRightTurn ? new DriftZoneData(value => value > 0) : new DriftZoneData(value => value < 0);
        }
    }

    public class DriftZoneData
    {
        private Func<float, bool> _comparator;

        public DriftZoneData(Func<float, bool> comparator)
        {
            _comparator = comparator;
        }

        public bool Check(float value)
        {
            return _comparator(value);
        }
    }
}