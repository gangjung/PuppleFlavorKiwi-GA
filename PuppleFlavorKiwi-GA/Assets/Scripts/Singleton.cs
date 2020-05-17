using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Common {
    public class BaseSingleton
    {
        public void Init()
        {

        }
    }

    public class BaseSingleton_Object : MonoBehaviour
    {
        public void Init()
        {
            gameObject.AddComponent<DontDestroyComponent>();
        }
    }

    public class Singleton<T> : BaseSingleton where T : BaseSingleton, new()
    {
        public static T Instance
        {
            get { 
                if(instance == null)
                {
                    instance = new T();
                    instance.Init();
                }

                return instance;
            }
        }
        protected static T instance;
    }

    public class Singleton_Object<T> : BaseSingleton_Object where T : BaseSingleton_Object
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