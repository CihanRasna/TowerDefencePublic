using System.Collections.Generic;
using _Game.Characters;
using _Game.Scripts.Enemy;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;
using Vanta.Levels;

namespace _Game.Levels.Base
{
    public class Level : BaseLevel
    {
        public SplineComputer spline;
        public Player player => _player as Player;
        private float _nextSpawnTime = 0f;

        [SerializeField] private int _currency = 100;
        public int currency => _currency;

        public UnityAction<int> currencyChanged;

        #region Life Cycle

        protected override void Start()
        {
            base.Start();

            _state = State.Loaded;
            listener.Level_DidLoad(this);
            Debug.Log("Loaded");
        }

        public void StartLevel()
        {
            _state = State.Started;
            listener.Level_DidStart(this);
        }

        public void IncomeCurrency(int prize) //TODO: New Script
        {
            _currency += prize;
            currencyChanged.Invoke(_currency);
        }

        public bool SpendCurrency(int price) // TODO: NEW SCRIPT
        {
            if (_currency < price) return false;
            _currency -= price;
            currencyChanged.Invoke(_currency);
            return true;
        }
        
        //TODO: SPAWNING SYSTEM SEPERATED SCRIPT

        #endregion
    }
}