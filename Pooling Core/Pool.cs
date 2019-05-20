using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pooling_Core
{
    public class Pool
    {
        private Queue<GameObject> poolingObjectsQueue = new Queue<GameObject>();

        private GameObject _poolingObject;

        /// <summary>
        /// Construct the pool with given object and amount
        /// </summary>
        /// <param name="poolingObject">Object that needed to be pooled</param>
        /// <param name="requiredAmount">Amount of the objects</param>
        public Pool(GameObject poolingObject, int requiredAmount)
        {
            for (int i = 0; i < requiredAmount; i++)
            {
                GameObject spawnedObject = Object.Instantiate(poolingObject);
                spawnedObject.SetActive(false);
                poolingObjectsQueue.Enqueue(spawnedObject);
            }

            _poolingObject = poolingObject;
        }

        /// <summary>
        /// Returns a pooled object
        /// </summary>
        /// <param name="activateOnReturn">Set true to activate the gameobject, default is false</param>
        /// <returns></returns>
        /// <exception cref="Exception">Exception is thrown if the pool is empty</exception>
        public GameObject GetObject(bool activateOnReturn = false)
        {
            if (poolingObjectsQueue.Count > 0)
            {
                GameObject go = poolingObjectsQueue.Dequeue();
                if (activateOnReturn)
                {
                    go.SetActive(true);
                }

                return go;
            }

            throw new Exception("Queue is empty");
        }

        /// <summary>
        /// Add a used gameobject to the pool again
        /// </summary>
        /// <param name="gameObject">Gameobject</param>
        public void ReturnGameObject(GameObject gameObject)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }

            poolingObjectsQueue.Enqueue(gameObject);
        }

        /// <summary>
        /// Add a used gameobject to the pool after a delay
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <param name="delayInMiliSeconds">Delay In MiliSeconds</param>
        public void ReturnGameObjectAfterDelay(GameObject gameObject, int delayInMiliSeconds)
        {
            Task.Delay(delayInMiliSeconds).ContinueWith(t =>
            {
                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }

                poolingObjectsQueue.Enqueue(gameObject);
            });
        }

        /// <summary>
        /// Returns the amount of gameobjects remaining in the pool
        /// </summary>
        /// <returns></returns>
        public int GetRemainingObjectsAmount()
        {
            int amount = poolingObjectsQueue.Count;
            return amount;
        }

        /// <summary>
        /// Increase the pool size
        /// </summary>
        /// <param name="amount">Required amount of gameobjects</param>
        public void IncreasePool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject spawnedObject = Object.Instantiate(_poolingObject);
                spawnedObject.SetActive(false);
                poolingObjectsQueue.Enqueue(spawnedObject);
            }
        }

        /// <summary>
        /// Clear the pool Immediately
        /// </summary>
        public void ClearPool()
        {
            poolingObjectsQueue.Clear();
        }
    }
}