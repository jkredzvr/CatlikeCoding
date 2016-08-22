//Keep a buffer of objects so they dont need to be destroyed
//If more is need, they can be created

//Prefabs can be edited like objects in the hiearchy 

using UnityEngine;

public class PooledObject : MonoBehaviour
{
    /// <summary>
    /// PooledObjects are returned to type ObjectPool Pool 
    /// </summary>
    
    [System.NonSerialized]
    ObjectPool poolInstanceForPrefab;
    
    //Gives access to this PooledObject's Pool
    public ObjectPool Pool { get; set; }

    public void ReturnToPool()
    {
        if (Pool)
        {
            Pool.AddObject(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Method that gets a pooled instance of an object
    public T GetPooledInstance<T>() where T : PooledObject
    {
        //If there is no instance of a pool for this PooledObject
        if (!poolInstanceForPrefab)
        {
            poolInstanceForPrefab = ObjectPool.GetPool(this);
        }
        return (T) poolInstanceForPrefab.GetObject();
    }
}