//Object Pool Class

using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

    PooledObject prefab;
    List<PooledObject> availableObjects = new List<PooledObject>();


    //public method to grab a pooled object and return it
    public PooledObject GetObject()
    {
        PooledObject obj;
        int LastAvailableIndex = availableObjects.Count - 1;
        if (LastAvailableIndex >= 0)
        {
            obj = availableObjects[LastAvailableIndex];
            //remove object from pool
            availableObjects.RemoveAt(LastAvailableIndex);
            obj.gameObject.SetActive(true);
        }
        else
        {
            //Instantiate the prefab and make it a child of the ObjectPol go.
            obj = Instantiate<PooledObject>(prefab);
            obj.transform.SetParent(transform,false);
            //This Object belongs to this ObjectPool
            obj.Pool = this;
        }
        return obj;
    }

    //Method to create a pool instance
    public static ObjectPool GetPool(PooledObject prefab)
    {
        GameObject obj;
        ObjectPool pool;
        if (Application.isEditor)
        {
            //If Pool Exists
            obj = GameObject.Find(prefab.name + "Pool");
            if (obj)
            {
                pool = obj.GetComponent<ObjectPool>();
                if (pool)
                {
                    return pool;
                }
            }
        }
        obj = new GameObject(prefab.name + " Pool");
        pool = obj.AddComponent<ObjectPool>();
        pool.prefab = prefab;
        return pool;
    }

    //Disable and add pooledobject into its pool
    public void AddObject(PooledObject obj)
    {
        //Disable the 
        obj.gameObject.SetActive(false);
        availableObjects.Add(obj);
    }
}