using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField]
    private UDictionary<string, ObjectPool> pools = new UDictionary<string, ObjectPool>();

    public override void Awake()
    {
        base.Awake();
    } 

    private void Start()
    {
        // Make each pool check if any GameObjects has been set in the inspector, and make sure they have PooledObject component attached.
        foreach (KeyValuePair<string, ObjectPool> pool in pools)
        {
            pool.Value.Start();
        }
    }

    /// <summary>
    /// Dynamically create a pool.
    /// </summary>
    /// <param name="pool">Dictionary key</param>
    public void AddPool(string pool, ObjectPool objectPool)
    {
        pools.Add(pool, objectPool);
    }

    /// <summary>
    /// Dynamically create a pool with GameObject.
    /// </summary>
    /// <param name="pool">Dictionary key</param>
    public void AddPool(string pool, GameObject prefabToSpawn)
    {
        AddPool(pool, new ObjectPool(prefabToSpawn));
    }

    /// <summary>
    /// Dynamically create a pool loading GameObject prefab from path.
    /// </summary>
    /// <param name="pool">Dictionary key</param>
    /// <param name="pathToPrefab">i.e. "Prefabs/SomeObjectName"</param>
    public void AddPool(string pool, string pathToPrefab)
    {
        AddPool(pool, Resources.Load(pathToPrefab) as GameObject);
    }

    /// <summary>
    /// Gets the first object from specified pool.
    /// </summary>
    /// <param name="pool"></param>
    /// <returns>GameObject</returns>
    public GameObject GetPooledObject(string pool)
    {
        if (pools.TryGetValue(pool, out ObjectPool objPool))
        {
            return objPool.Get();
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Specified pool doesn't exist: {pool}");
#endif
            return null;
        }
    }

    /// <summary>
    /// Gets a random object from the specified pool.
    /// </summary>
    /// <param name="pool"></param>
    /// <returns>GameObject</returns>
    public GameObject GetRandomPooledObject(string pool)
    {
        if (pools.TryGetValue(pool, out ObjectPool objPool))
        {
            return objPool.GetRandom();
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Specified pool doesn't exist: {pool}");
#endif
            return null;
        }
    }
}