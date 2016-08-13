using UnityEngine;
using System.Collections;
//using UnityEditor;

//summary//
// procedural code that generates fractal shapes
//each child changes in
// - meshshape
// - scale
// - color (fading from a to b, based on depth
// - probability of spawning a child
// - a set max depth of children hiearchy
// - rotation in z-axis
// 

public class Fractal : MonoBehaviour {

    //child scale will be relative to parent.  Each child will be reduced by scale relative to parent.
    public float childScale;
    public Mesh mesh;
    public Mesh[] meshes;
    public Material material;
    private Material[,] materials;

    public float spawnProbability;

    public int maxDepth;
    private int depth;

    public float maxRotationSpeed;
    private float rotationSpeed;

    public float maxTwist;

    //static arrays
    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private void Start()
    {
        //first parent fractal instantiates materials array
        if (materials == null)
        {
            InitializeMaterials();
        }
        //Add a randomized mesh and color (based on depth of child)
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0,meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0,2)];

        //set rotation speed
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        
        if (depth < maxDepth)
        {
            //Coroutine to generate next depth's child fractal
            StartCoroutine(CreateChildren());
        }
        
    }

    

    private void InitializeMaterials()
    {
        //create x depth by 2 (different colored) array for materials
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i/(maxDepth - 1f);
            t *= t;
            materials[i,0]=     new Material(material);
            materials[i,0].color = Color.Lerp(Color.white, Color.yellow, t);

            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        //final color of fractal at the last depth are
        materials[maxDepth,0 ].color = Color.green;
        materials[maxDepth,1].color = Color.red;
    }

    //On creation of a child fractal, initialize its mesh, material, etc passing the parent's parameters.
    //Since Fractal class starts off empty, grab parameters from the parent
    private void Initialize(Fractal parent, int childIndex)
    {
        spawnProbability = parent.spawnProbability;
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        //increment the depth property for each child iteration
        depth = parent.depth + 1;
        childScale = parent.childScale;
        //Debug.Log(childScale);

        transform.parent = parent.transform;
        transform.localScale = Vector3.one*childScale;
        transform.localPosition = childDirections[childIndex] * (.5f +.5f *childScale);
        transform.localRotation = childOrientations[childIndex];
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
    }

    private IEnumerator CreateChildren()
    {
        //for each direction a child can be spawned
        for (int i = 0; i < childDirections.Length; i++)
        {
            //randomly spawn
            if (Random.value < spawnProbability)
            {
                //at random intervals 
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                //when Adding Components, it creates the component and returns a reference to it
                //this allows us to immediately access component values or methods even though it was not there before


                //create new gameboject, with fractal component, calling intialize method, passing reference of the parent and childindex
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
