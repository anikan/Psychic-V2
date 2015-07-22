using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour {
    public float damageResist = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.attachedRigidbody != null)
        {
            float damage = collision.collider.attachedRigidbody.velocity.magnitude * collision.collider.attachedRigidbody.mass;

            if (damage > damageResist)
            {
                SendMessage("DealDamage", 20);
                SendMessage("SetLastHitTime");
                Vector3 hitPoint = collision.contacts[0].point;//hit.point;

                hitPoint.y = transform.position.y;
                Vector3 hitDirection = hitPoint - transform.position;
                hitDirection.Normalize();
                hitDirection = transform.root.InverseTransformDirection(hitDirection);
                SendMessage("SetHitDirection", hitDirection);
                Vector3 recoilDirection = transform.position - hitPoint;
                recoilDirection.Normalize();
                SendMessage("SetrecoilDirecion", recoilDirection);
                //SendMessage("DealDamage", )
            }
        }
    }
}
