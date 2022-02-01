using System.Collections;
using System.Collections.Generic;
using _Game.Characters;
using _Game.Scripts.Enemy;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;
using Vanta.Levels;

namespace _Game.Levels.Base
{
    public class Level : BaseLevel
    {
        public SplineComputer spline;
        public Player player => _player as Player;

        [SerializeField] private List<BaseEnemy> enemies;
        private float _nextSpawnTime = 0f;

        #region Life Cycle

        private void OnValidate()
        {
        }

        protected override void Start()
        {
            base.Start();

            // TODO: Initialize level

            _state = State.Loaded;
            listener.Level_DidLoad(this);
            Debug.Log("Loaded");
            
        }

        public void StartLevel()
        {
            _state = State.Started;
            listener.Level_DidStart(this);
        }

        public void InvokeEnemy()
        {
            var rnd = Random.Range(0, enemies.Count);
            _nextSpawnTime = rnd == 0 ? 3f : 7f;
            var spawnedEnemy = enemies[0];
            Instantiate(spawnedEnemy, transform);
            Invoke(nameof(InvokeEnemy), _nextSpawnTime);
        }

        #endregion
    }
}