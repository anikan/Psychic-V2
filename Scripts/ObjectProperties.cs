using UnityEngine;
using System.Collections;

public class ObjectProperties : MonoBehaviour {
    public bool grappleble = true;
    public bool telekinesesble = true;
    private Shader highlightShader;
    private Shader standardShader;
    public Color purple;


	// Use this for initialization
	void Start () {
        highlightShader = Shader.Find("Outlined/Silhouetted Diffuse");
        standardShader = Shader.Find("Standard");
    //    highlightShader
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        PsychicScript psychic = GameObject.FindGameObjectWithTag("Player").GetComponent<PsychicScript>();

        if (!psychic.TKActive)
        {
            //Set the shader to the outline one.
            Material[] objectMats = GetComponent<Renderer>().materials;
            for (int i = 0; i < objectMats.Length; i++)
            {
                objectMats[i].shader = highlightShader;
            }

            GetComponent<Renderer>().materials = objectMats;
        }
//        startcolor = GetComponent<Renderer>().GetComponent<Material>().color;
        //GetComponent<Renderer>().GetComponent<Material>().color = Color.yellow;
    }
    void OnMouseExit()
    {
        PsychicScript psychic = GameObject.FindGameObjectWithTag("Player").GetComponent<PsychicScript>();

        if (!psychic.TKActive)
        {
            //Set the shader to the regular one.
            Material[] objectMats = GetComponent<Renderer>().materials;
            for (int i = 0; i < objectMats.Length; i++)
            {
                objectMats[i].shader = standardShader;
            }

            GetComponent<Renderer>().materials = objectMats;
        }
  //      GetComponent<Renderer>().GetComponent<Material>().color = startcolor;
    }
}
