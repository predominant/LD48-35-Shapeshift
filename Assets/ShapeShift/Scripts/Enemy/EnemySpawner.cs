using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShapeShift.Terrain;

namespace ShapeShift.Enemy
{
    [RequireComponent(typeof(TerrainRenderer))]
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> EnemyPrefabs;

        private TerrainRenderer _terrain;

        public void Start()
        {
            this._terrain = this.GetComponent<TerrainRenderer>();
            this.SpawnEnemies(this._terrain.Size.x / this._terrain.StepWidth / 10f * 9f);
        }

        private void SpawnEnemies(float width)
        {
            int amount = (int)width / 5;
            for (int i = 0; i < amount; i++)
            {
                this.Spawn(i);
            }
        }

        private void Spawn(int index)
        {
            
        }
    }
}