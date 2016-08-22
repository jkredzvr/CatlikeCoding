using UnityEngine;
using System.Collections;

public class DestroyOnTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Walker")
        {
            Destroy(gameObject);
        }
    }

}
