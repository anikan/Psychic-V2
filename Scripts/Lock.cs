using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {
    public GameObject door;
    public AudioClip unlockSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("key lps");
        if (collision.collider.gameObject.name.Equals("Key"))
        {
            door.GetComponent<MeshCollider>().enabled = false;
            door.GetComponent<BoxCollider>().enabled = true;
            door.GetComponent<Rigidbody>().isKinematic = false;

            AudioSource.PlayClipAtPoint(unlockSound, this.transform.position);
            Destroy(this);
        }
    }
}
