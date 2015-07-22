using UnityEngine;
using System.Collections;

    public abstract class TKEffect : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }
        
        // Update is called once per frame
        void Update()
        {

        }
        
        public abstract void onTKStart();
        public abstract void onTK();
        public abstract void onTKEnd();
    }

