using UnityEngine;
using System.Collections;

public class MeshDeformerInput : MonoBehaviour
{

    public float force = 10f;
    public float forceOffset = 0.1f;
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(0))
	    {
	        HandleInput();
	    }
	}

    void HandleInput()
    {
        //cast ray from camera to mouse position.  Convert into mouse x,y position
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        //if ray inputRay hits something, output the raycast hit.  Get MeshDeformer component
        if (Physics.Raycast(inputRay, out hit)) {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();

            if (deformer) {
                Vector3 point = hit.point;
                //offset causes the point surface to be pushed into(indented) instead of pushed asside
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
        }
    }
}
