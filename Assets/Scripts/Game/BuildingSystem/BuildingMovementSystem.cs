using System;
using System.Collections.Generic;
using Game.Environment;
using Game.Player;
using Game.PoolSystem;
using Infrastructure.Data.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.BuildingSystem
{
    public class BuildingMovementSystem
    {
        private readonly BuildingsData _data;
        private readonly IEnvironmentHolder _environmentHolder;
        private readonly LinkedList<BuildingConnector> _spawnedBuildings;
        private readonly AnimatorBackground _animatorBackground;
        private readonly AnimatorBackground _animatorBackground1;
        private readonly AnimatorBackground _animatorBackground2;
        
        private PoolCollection<BuildingConnector> _poolSystem;
        private Action _onMove;
        private IDisposable _buildingMovement;

        public BuildingMovementSystem(BuildingsData data,
            IEnvironmentHolder environmentHolder)
        {
            _data = data;
            _environmentHolder = environmentHolder;
            _spawnedBuildings = new LinkedList<BuildingConnector>();
            _animatorBackground = new AnimatorBackground();
            _animatorBackground1 = new AnimatorBackground();
            _animatorBackground2 = new AnimatorBackground();
        }

        private Transform Parent => _environmentHolder.Environment.BuildingRoot;
        private EnvironmentObjects Environment => _environmentHolder.Environment;
        private PlayerAnimation PlayerAnimation => _environmentHolder.Environment.Player;

        public void Initialize()
        {
            _poolSystem = new PoolCollection<BuildingConnector>(Parent);
            SpawnFirst();
            _animatorBackground.Initialize(Environment.DynamicLayer1, true, 0.05f);
            _animatorBackground1.Initialize(Environment.DynamicLayer2);
            _animatorBackground2.Initialize(Environment.DynamicLayer3, true);
        }

        private void SpawnFirst()
        {
            BuildingConnector lastBuilding = _poolSystem.Get(GetRandomPrefab(), Parent);
            lastBuilding.transform.localPosition = new Vector3(0f, _data.StartSpawnPosition);
            _spawnedBuildings.AddFirst(lastBuilding);

            while (lastBuilding.transform.position.y > _data.MinimumPosition)
            {
                BuildingConnector building = _poolSystem.Get(GetRandomPrefab(), Parent);
                building.ConnectUpperTo(lastBuilding.Border.BorderDown);
                _spawnedBuildings.AddLast(building);
                lastBuilding = building;
            }
        }

        public void Subscribe()
        {
            _buildingMovement = PlayerAnimation.AddClimbListener(DoMove);
            _onMove += DoHideLowerEnvironment;
        }

        public void UnSubscribe()
        {
            // refactor to IDisposable
            _buildingMovement.Dispose();
            _onMove -= DoHideLowerEnvironment;
        }

        private void DoMove()
        {
            foreach (var building in _spawnedBuildings)
            {
                float value = _data.SpeedBuildings * Time.deltaTime * PlayerAnimation.SpeedMultiplier;
                building.MoveY(value);
            }

            _onMove?.Invoke();
            OnAnimateBackGround();

            if (_spawnedBuildings.Last.Value.Position.y < _data.MinimumPosition)
                GenerateNext();
        }

        private void OnAnimateBackGround()
        {
            _animatorBackground.Animate(Environment.DynamicLayer1);
            _animatorBackground1.Animate(Environment.DynamicLayer2);
            _animatorBackground2.Animate(Environment.DynamicLayer3);
        }

        private void DoHideLowerEnvironment()
        {
            Transform lowerEnvironment = Environment.LowerLevel;
            if (lowerEnvironment.position.y <= _data.MinLowerLevelPosition)
            {
                UnConnectLowerLevel();
                return;
            }

            float value = _data.MoveLowerLevelSpeed * Time.deltaTime;
            var pos = lowerEnvironment.position;
            lowerEnvironment.position = new Vector2(pos.x, pos.y - value);
        }

        private void UnConnectLowerLevel()
        {
            _onMove -= DoHideLowerEnvironment;
        }
 
        private void GenerateNext()
        {
            _poolSystem.Return(_spawnedBuildings.Last.Value);
            _spawnedBuildings.RemoveLast();

            BuildingConnector first = _spawnedBuildings.First.Value;
            BuildingConnector building = _poolSystem.Get(GetRandomPrefab(), Parent);
            building.ConnectDownTo(first.Border.BorderUp);
            _spawnedBuildings.AddFirst(building);
        }

        private BuildingConnector GetRandomPrefab()
        {
            int index = Random.Range(0, _data.BuildingsPrefabs.Length);
            return _data.BuildingsPrefabs[index];
        }
    }
}