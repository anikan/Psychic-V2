using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using PicoGames.QuickRopes;
using UnityStandardAssets.Characters.FirstPerson;


public class PsychicScript : MonoBehaviour
{

    int power = 100;
    public bool TKActive = false;
    public bool GActive = false;
    public bool debug = false;

    //TK values.
    const float k_Spring = 100.0f;//50.0f;
    const float k_Damper = 5.0f;
    const float k_Drag = 10.0f;
    const float k_AngularDrag = 5.0f;
    const float k_Distance = 0.2f;
    const bool k_AttachToCenterOfMass = false;

    private SpringJoint m_SpringJoint;

    [SerializeField]
    private QuickRope grappleRope;

    public LineRenderer TKRenderer;
    public LineRenderer grappleRenderer;

    Transform oldParent;
    Transform TKObject;

    private Animator leftHand;
    private Animator rightHand;

    private ParticleSystem leftHandEffect;
    private ParticleSystem rightHandEffect;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        leftHand = GameObject.Find("upperarm_l").GetComponent<Animator>();
        rightHand = GameObject.Find("upperarm_r").GetComponent<Animator>();

        leftHandEffect = GameObject.Find("Telekinesis Enchant").GetComponent<ParticleSystem>();
        rightHandEffect = GameObject.Find("Boost Splinters").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit checkHit = new RaycastHit();
        Ray forward = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Let user go if escape is pushed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Telekinesis power.
        if (Input.GetMouseButtonDown(0))
        {
            //Allow user to rejoin if left.
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }


            //If looking at an object and need to activate TK.
            if (!TKActive && Physics.Raycast(forward, out checkHit) && checkHit.rigidbody != null)
            {
                ObjectProperties properties = checkHit.transform.gameObject.GetComponent<ObjectProperties>();

                //Checking if TK-able
                if (properties != null && properties.telekinesesble)
                {
                    /************* Make transparent ******************/
                    //checkHit.transform.gameObject.GetComponent<Renderer>().a = .5;
                    TKActive = true;

                    leftHand.SetBool("TKActive", true);
                    leftHandEffect.Play(true);

                    if (!m_SpringJoint)
                    {
                        var go = new GameObject("Rigidbody dragger");
                        Rigidbody body = go.AddComponent<Rigidbody>();
                        m_SpringJoint = go.AddComponent<SpringJoint>();
                        body.isKinematic = true;
                    }

                    m_SpringJoint.transform.position = checkHit.point;
                    m_SpringJoint.anchor = Vector3.zero;

                    m_SpringJoint.spring = k_Spring;
                    m_SpringJoint.damper = k_Damper;
                    m_SpringJoint.maxDistance = k_Distance;
                    m_SpringJoint.connectedBody = checkHit.rigidbody;

                    TKEffect effect = checkHit.transform.gameObject.GetComponent<TKEffect>();
                    

                    if (effect != null)
                    {
                        Debug.Log("hi");
                        effect.onTKStart();
                    }
                    StartCoroutine("DragObject", checkHit.distance);
                }
            }
        }

        //Leap.
        if (Input.GetMouseButtonDown(1))
        {
            rightHandEffect.Stop(true);

            //If looking at an object and need to activate Grapple.
            if (!GActive)// && Physics.Raycast(forward, out checkHit))
            {
                GetComponent<Rigidbody>().AddForce(forward.direction.normalized * 100, ForceMode.Impulse);
                rightHand.SetTrigger("LeapActive");
                rightHandEffect.Play(true);
            }
        }
    }

    private IEnumerator DragObject(float distance)
    {
        var oldDrag = m_SpringJoint.connectedBody.drag;
        var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
        m_SpringJoint.connectedBody.drag = k_Drag;
        m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
        var mainCamera = Camera.main;

        //while (Input.GetMouseButton(0) && TKActive == true)

        //Test for click once to activate TK.
        while (TKActive == true)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            m_SpringJoint.transform.position = ray.GetPoint(distance);
            yield return null;

            //Simple Drop
            if (Input.GetKeyDown(KeyCode.E))
            {
                TKActive = false;
            }

            //Throw
            if (Input.GetMouseButtonDown(0))
            {
                //Add force
                m_SpringJoint.connectedBody.AddForce((m_SpringJoint.connectedBody.transform.position - transform.position).normalized * 1000);

                TKActive = false;
            }

            //Bring closer or further.
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                //float moveDistance = Input.GetAxis("Mouse ScrollWheel");
                distance += Input.GetAxis("Mouse ScrollWheel") * 2;

            }
        }
        leftHand.SetBool("TKActive", false);
        leftHandEffect.Stop(true);

        /*****************Make solid *******************/
        TKActive = false;

        //Set the shader to the regular one.
        Material[] objectMats = m_SpringJoint.connectedBody.GetComponent<Renderer>().materials;
        for (int i = 0; i < objectMats.Length; i++)
        {
            objectMats[i].shader = Shader.Find("Standard");
        }

        m_SpringJoint.connectedBody.GetComponent<Renderer>().materials = objectMats;

        //Disconnect object.
        if (m_SpringJoint.connectedBody)
        {
            m_SpringJoint.connectedBody.drag = oldDrag;
            m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
            m_SpringJoint.connectedBody = null;
        }
    }
}
