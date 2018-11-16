using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour {

    public GameObject roomPrefab;

	// Use this for initialization
	void Start () {
        Generator generator = new Generator(roomPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
