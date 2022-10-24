using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool
{
    [SerializeField]
    private GameObject prefabToSpawn;
    private List<GameObject> pooledObjects;

    public void Add(GameObject gameObject)
    {
        pooledObjects.Add(gameObject);
    }

    public ObjectPool(GameObject prefab)
    {
        prefabToSpawn = prefab;
    }

    // Called from ObjectPoolManager's Start.
    public void Start()
    {
        pooledObjects = new List<GameObject>();

        // If any objects are already set in the inspector, we wanna make sure they have PooledObjects attached.
        if (pooledObjects.Count != 0)
        {
            foreach (GameObject gameObject in pooledObjects)
            {
                if (!gameObject.GetComponent<PooledObject>())
                {
                    AddComponentPooledObject(gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Gets and removes a object from pool.
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject Get()
    {
        if (pooledObjects.Count != 0)
        {
            var temp = pooledObjects[0];
            pooledObjects.Remove(temp);
            return temp;
        }
        else
        {
            return InstantiateNew();
        }
    }

    /// <summary>
    /// Gets and removes a random object from pool.
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetRandom()
    {
        if (pooledObjects.Count != 0)
        {
            if (pooledObjects.Count > 1)
            {
                var temp = pooledObjects[UnityEngine.Random.Range(0, pooledObjects.Count)];
                pooledObjects.Remove(temp);
                return temp;
            }
            else
            {
                return Get();
            }
        }
        else
        {
            return InstantiateNew();
        }
    }

    /// <summary>
    /// Instatiates a new object from the pool.
    /// </summary>
    /// <returns>GameObject</returns>
    private GameObject InstantiateNew()
    {
        var temp = GameObject.Instantiate(prefabToSpawn);
        temp.SetActive(false);
        AddComponentPooledObject(temp);
        return temp;
    }

    public void AddComponentPooledObject(GameObject gameObject)
    {
        gameObject.AddComponent<PooledObject>().pool = this;
    }
}