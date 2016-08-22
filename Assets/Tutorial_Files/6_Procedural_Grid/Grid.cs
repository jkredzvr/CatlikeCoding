//using System.CodeDom.Compiler;
//using UnityEngine;
//using System.Collections;

///// <summary>
///// Meshes need vertex positions and triangles, usually UV coordinates too – up to four sets – and often tangents as well. 
///// You can also add vertex colors, although Unity's standard shaders don't use those. 
///// You can create your own shaders that do use those colors.
///// </summary>

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
//public class Grid : MonoBehaviour
//{
//    private Vector3[] vertices;
//    public int xSize, ySize;
//    private Mesh mesh;

//    private void Awake()
//    {
//        StartCoroutine(Generate());
//    }

//    private IEnumerator Generate() {
//        WaitForSeconds wait = new WaitForSeconds(0.05f);

//        //assigning mesh to vertices
//        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
//        mesh.name = "Procedural Grid";

//        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
//        Vector2[] uv =new Vector2[vertices.Length];

//        //tangents to set up normal planes
//        Vector4[] tangents = new Vector4[vertices.Length];
//        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

//        //loop to columns last
//        for (int i = 0, y = 0; y <= ySize; y++) {
//            //loop through rows first, but keep incrementing by index
//            for (int x = 0; x <= xSize; x++, i++) {
//                vertices[i] = new Vector3(x, y);
                
//                //normalized vertices of UV 0-1
//                uv[i] = new Vector2((float) x/xSize, (float)y / ySize);
//                tangents[i] = tangent;
//                yield return wait;
//            }
//        }
//        //Assigned vertices to the mesh filter
//        mesh.vertices = vertices;
//        mesh.uv = uv;
//        mesh.tangents = tangents;

//        int[] triangles = new int[xSize * ySize * 6];
//        //Creates quads by creating two triangles using 6 vertexs
//        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
//        {
//            for (int x = 0; x < xSize; x++, ti += 6, vi++)
//            {
//                triangles[ti] = vi;
//                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
//                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
//                triangles[ti + 5] = vi + xSize + 2;
//                mesh.triangles = triangles;
//                mesh.RecalculateNormals();
//                yield return wait;
//            }
//        }
//    }

//    //visualize vertices by adding an OnDrawGizmos drawing a small black sphere in the scene view for every vertex.
//    private void OnDrawGizmos() {
//        if (vertices == null) {
//            return;
//        }
//        Gizmos.color = Color.black;
//        for (int i = 0; i < vertices.Length; i++) {
//            Gizmos.DrawSphere(vertices[i], 0.1f);
//        }
//    }
//}