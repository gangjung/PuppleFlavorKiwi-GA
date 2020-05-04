using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Common {
    public class BaseSingleton : MonoBehaviour
    {
        public void Init()
        {
            gameObject.AddComponent<DontDestroyComponent>();
        }
    }

    public class Singleton<T> : BaseSingleton where T : BaseSingleton
    {
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    //instance = new T();
                    GameObject obj = GameObject.Find(typeof(T).Name);
                    if(obj == null)
                    {
                        obj = new GameObject(typeof(T).Name);
                        instance = obj.AddComponent<T>();
                        instance.Init();
                    }
                    else
                    {
                        instance = obj.GetComponent<T>();
                    }
                }

                return instance;
            }
        }
        protected static T instance;
    }
}