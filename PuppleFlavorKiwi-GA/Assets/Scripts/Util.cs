using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Util : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public static HUDManager GetHUD()
        {
            HUDManager result = null;
            GameObject obj = null;

            obj = GameObject.Find("HUDManager");

            if(obj != null)
            {
                result = obj.GetComponent<HUDManager>();
            }

            return result;
        }
    }
}

