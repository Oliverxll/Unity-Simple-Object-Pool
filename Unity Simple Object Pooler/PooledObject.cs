using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public ObjectPool pool;

    public void Release()
    {
        gameObject.SetActive(false);
        pool.Add(gameObject);
    }
}
