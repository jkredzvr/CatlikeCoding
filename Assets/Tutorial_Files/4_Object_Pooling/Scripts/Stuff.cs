using UnityEngine;

/// <summary>
/// This Stuff is a Poolable Object inheriting PooledObject's  ReturneToPool() Method
/// Stuff Component attached to the root of the "Physical Thing"
/// On Awake, get an array of MeshRenderers in Children
/// Expose a SetMaterial Method to automatically pass the material;
/// And Collision Behaviour
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class Stuff : PooledObject
{
    private MeshRenderer[] meshRenderers;


	public Rigidbody Body { get; private set; }

	void Awake () {
		Body = GetComponent<Rigidbody>();
	    meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}

    //Public method to change the material
    public void SetMatieral(Material m)
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = m;
        }
    }

    //Collision behviour
	void OnTriggerEnter (Collider enteredCollider) {
		if (enteredCollider.CompareTag("Kill Zone")) {

            //return to Pool
			ReturnToPool();
		}
	}
}