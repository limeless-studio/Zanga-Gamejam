using System.Collections;
using Core;
using UnityEngine;

namespace Utilities
{
    public class ExplosiveBarrelScript : Actor {
    
    	float randomTime;
    	bool routineStarted = false;
    
    	//Used to check if the barrel 
    	//has been hit and should explode 
    	public bool explode = false;
    
    	[Header("Prefabs")]
    	//The explosion prefab
    	public Transform explosionPrefab;
    	//The destroyed barrel prefab
    	public Transform destroyedBarrelPrefab;

	    [Header("Customizable Options")] public float damage = 10f;
    	//Minimum time before the barrel explodes
    	public float minTime = 0.05f;
    	//Maximum time before the barrel explodes
    	public float maxTime = 0.25f;
    
    	[Header("Explosion Options")]
    	//How far the explosion will reach
    	public float explosionRadius = 12.5f;
    	//How powerful the explosion is
    	public float explosionForce = 4000.0f;
    	
    	private void Update () {
    		//Generate random time based on min and max time values
    		randomTime = Random.Range (minTime, maxTime);
    
    		//If the barrel is hit
    		if (explode == true) 
    		{
    			if (routineStarted == false) 
    			{
    				//Start the explode coroutine
    				StartCoroutine(Explode());
    				routineStarted = true;
    			} 
    		}
    	}
    	
    	private IEnumerator Explode () {
    		//Wait for set amount of time
    		yield return new WaitForSeconds(randomTime);
    
    		//Spawn the destroyed barrel prefab
    		Instantiate (destroyedBarrelPrefab, transform.position, 
    		             transform.rotation); 
    
    		//Explosion force
    		Vector3 explosionPos = transform.position;
    		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
    		foreach (Collider hit in colliders) {
    			Rigidbody rb = hit.GetComponent<Rigidbody> ();
    			
    			//Add force to nearby rigidbodies
    			if (rb != null)
    				rb.AddExplosionForce (explosionForce * 50, explosionPos, explosionRadius);
    
			    if (hit.gameObject.TryGetComponent(out IDamageable damageable))
			    {
				    if (hit.gameObject.TryGetComponent(out GasTankScript tank))
				    {
					    tank.explosionTimer = 0.05f;
				    }
				    damageable.TakeDamage(damage, DamageCause.DamagedByActor);
			    }
    		}
    
    		//Raycast downwards to check the ground tag
    		RaycastHit checkGround;
    		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
    		{
    			//Instantiate explosion prefab at hit position
    			Instantiate (explosionPrefab, checkGround.point, 
    				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
    		}
    
    		//Destroy the current barrel object
    		Destroy (gameObject);
    	}

	    public override void Die()
	    {
		    base.Die();
		    explode = true;
	    }
    }
}