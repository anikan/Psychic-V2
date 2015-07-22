using UnityEngine;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts
{   
    public class InitFrozenObjectProperties : TKEffect
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void onTKStart()
        {
            //Stop the object being frozen in space.
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
        }
        
        //Do nothing for these cases.
        public override void onTK() { }
        public override void onTKEnd() {  }
    }
}