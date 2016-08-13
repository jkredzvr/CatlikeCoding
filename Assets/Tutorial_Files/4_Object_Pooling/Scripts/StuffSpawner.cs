using UnityEngine;

public class StuffSpawner : MonoBehaviour {

	public FloatRange timeBetweenSpawns, scale, randomVelocity, angularVelocity;

	public float velocity;

	public Material stuffMaterial;

	public Stuff[] stuffPrefabs;

	float timeSinceLastSpawn;
	float currentSpawnDelay;

	void FixedUpdate () {
		timeSinceLastSpawn += Time.deltaTime;
		if (timeSinceLastSpawn >= currentSpawnDelay) {
			timeSinceLastSpawn -= currentSpawnDelay;
			currentSpawnDelay = timeBetweenSpawns.RandomInRange;
			SpawnStuff();
		}
	}

	void SpawnStuff () {
		Stuff prefab = stuffPrefabs[Random.Range(0, stuffPrefabs.Length)];
        //Grab a pooled Instance from a random prefab object (Sphere, Cube, Caltrop, etc)
		Stuff spawn = prefab.GetPooledInstance<Stuff>();

		spawn.transform.localPosition = transform.position;
		spawn.transform.localScale = Vector3.one * scale.RandomInRange;
		spawn.transform.localRotation = Random.rotation;
        spawn.SetMatieral(stuffMaterial);

        spawn.Body.velocity = transform.up * velocity +
			Random.onUnitSphere * randomVelocity.RandomInRange;
		spawn.Body.angularVelocity =
			Random.onUnitSphere * angularVelocity.RandomInRange;
        
    }
}