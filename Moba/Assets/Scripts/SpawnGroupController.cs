using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroupController : MonoBehaviour
{
    [System.Serializable]
    public struct Spawner
    {
        public GameObject enemyType;
        public Transform spawnMarker;
    }

    [SerializeField] float mFirstSpawnTime = 2.0f;
    [SerializeField] float mSpawnDelay = 1.0f;
    [SerializeField] List<Spawner> mSpawners;
    [SerializeField] WayPointPath mPath;
    private float mNextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        mNextSpawnTime = mFirstSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(mNextSpawnTime < Time.time)
        {
            foreach (var spawner in mSpawners)
            {
                var enemy = Instantiate(spawner.enemyType, spawner.spawnMarker).GetComponent<EnemyController>();
                enemy.Start();
                enemy.SetPath(mPath);
            }
            mNextSpawnTime += mSpawnDelay;
        }
    }
}
