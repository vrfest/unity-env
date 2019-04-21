using UnityEngine;

/// ------------------------------
/// Creating instance of particles
/// ------------------------------
public class InstanceExample : MonoBehaviour
{
	/// ------------------------------
	/// Singleton
	/// ------------------------------
	public static InstanceExample Instance;

	public ParticleSystem effectA;
	public ParticleSystem effectB;
    private Vector3 currentPos;

	void Awake()
	{
		/// ---------------------
		// Register the singleton
		/// ---------------------
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of InstanceExample script!");
		}
        currentPos = gameObject.transform.position;
		Instance = this;
	}
    private int counter = 0;
	void Update(){
        /// -----------------------------------------
        /// Instanciate into a box of 5 x 5 x 5 (xyz)
        /// -----------------------------------------
        if (counter++ == 15) {
            InstanceExample.Instance.Explosion(new Vector3(currentPos.x + Random.Range(-30.0f, 30.0f), currentPos.y + Random.Range(-30.0f, 30.0f), currentPos.z));
            counter = 0;
        }
	}

	/// -----------------------------------------
	/// Create an explosion at the given location
	/// -----------------------------------------
	public void Explosion(Vector3 position)
	{
		instantiate(effectA, position);
		instantiate(effectB, position);
	}

	/// -----------------------------------------
	/// Instantiate a Particle system from prefab
	/// -----------------------------------------
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(prefab,position,Quaternion.identity) as ParticleSystem;

		/// -----------------------------
		// Make sure it will be destroyed
		/// -----------------------------
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
		);

		return newParticleSystem;
	}



}