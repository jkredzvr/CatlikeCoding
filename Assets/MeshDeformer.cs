using UnityEngine;
using System.Collections;

public class MeshDeformer : MonoBehaviour
{
    private Mesh deformingMesh;
    //arrays for storing original and displaced vertices vectors
    private Vector3[] originalVertices, displacedVertices, vertexVelocities;

    public float springForce = 20f;
    public float damping = 5f;

    float uniformScale = 1f;


    void Start ()
	{
        //initialize the mesh
	    deformingMesh = GetComponent<MeshFilter>().mesh;
        //original vertices copy of the mesh vertices
        originalVertices = deformingMesh.vertices;
        //displaced vertices is a copy of orignal vertices
	    displacedVertices = new Vector3[originalVertices.Length];
        for (int i=0; i < originalVertices.Length; i++)
	    {
	        displacedVertices[i] = originalVertices[i];
	    }

        vertexVelocities = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update() {

        uniformScale = transform.localScale.x;

        //this will constantly deform the mesh
        for (int i = 0; i < displacedVertices.Length; i++) {
            UpdateVertex(i);
        }
        //update the deformed mesh vertices
        deformingMesh.vertices = displacedVertices;
        //recalculate the normals which changes the loook of the mesh mapping
        deformingMesh.RecalculateNormals();
    }

    //when hit, do this.
    public void AddDeformingForce(Vector3 point, float force) {
        //draw line from ray point source to touched point
        Debug.DrawLine(Camera.main.transform.position, point);

        //convert the raycasted point world space into object's local space
        point = transform.InverseTransformPoint(point);

        //loop through displaced Vertice array and add force to each vertex
        for (int i = 0; i < displacedVertices.Length; i++) {
            AddForceToVertex(i, point, force);
        }
    }

    void AddForceToVertex(int i, Vector3 point, float force) {
        //vector from point to the vertex
        Vector3 pointToVertex = displacedVertices[i] - point;

        //this scales the distance from point to vertice
        pointToVertex *= uniformScale;

        //attenuated force can now be found using the inverse-square law
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        //final velocity is the normalized velocity multiplied by calculated velocity based on the force
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    void UpdateVertex(int i) {
        Vector3 velocity = vertexVelocities[i];
        //displacement vector, how far has vertice moved
        Vector3 displacement = displacedVertices[i] - originalVertices[i];

        displacement *= uniformScale;

        //calculate velocity effect of spring 
        velocity -= displacement * springForce * Time.deltaTime;
        //dampen spring effect over time
        velocity *= 1f - damping * Time.deltaTime;
        //update the velocity
        vertexVelocities[i] = velocity;
        //reposition the vertice position based on dampening
        displacedVertices[i] += velocity * (Time.deltaTime/uniformScale);
    }
}
