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
        [SerializeField] private int _health;
        
        public int currency => _currency;
        public int health => _health;

        public UnityAction<int> currencyChanged;
        public UnityAction<int> healthChanged;

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

        public void TakeHit(int dmg)
        {
            _health -= dmg;
            healthChanged.Invoke(_health);
            if (health <= 0 && _state != State.Failed)
            {
                _state = State.Failed;
                listener.Level_DidFail(this);
            }
        }

        public void IncomeCurrency(int prize)
        {
            _currency += prize;
            currencyChanged.Invoke(_currency);
        }

        public bool SpendCurrency(int price)
        {
            if (_currency < price) return false;
            _currency -= price;
            currencyChanged.Invoke(_currency);
            return true;
        }

        #endregion
    }
}