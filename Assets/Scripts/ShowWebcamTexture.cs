using UnityEngine;
using System.Collections;

public class ShowWebcamTexture : MonoBehaviour {

	// Use this for initialization
	void Start() {
		Debug.Log (WebCamTexture.devices.Length);
		foreach (WebCamDevice wd in WebCamTexture.devices) {
			Debug.Log (wd.name);
		}

		WebCamTexture webcamTexture = new WebCamTexture();
		webcamTexture.requestedFPS = 60;
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
