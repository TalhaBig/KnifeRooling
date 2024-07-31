using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour {

    public float lifeTime = 0f;

	// Use this for initialization
	void Start () 
    {
        Destroy(gameObject, lifeTime);
	}
}
