using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BB.Helpers
{
    public class MapInfo : MonoBehaviour
    {
        static MapInfo mInstance;


        //x - левый и правый border, y - верхний и нижний
        public Vector2 Borders;

        public static MapInfo Instance
        {
            get
            {
                if (mInstance == null)
                {
                    GameObject go = new GameObject();
                    mInstance = go.AddComponent<MapInfo>();
                }
                return mInstance;
            }
        }

        private void Awake()
        {
            mInstance = this;
        }
    }
}