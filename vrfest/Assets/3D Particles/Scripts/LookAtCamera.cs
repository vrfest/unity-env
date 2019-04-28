using UnityEngine;

public class LookAtCamera:MonoBehaviour{
    public Camera lookAtCamera;
    public bool lookOnlyOnAwake;

	public void Start() {
    	if(lookAtCamera == null){
    		lookAtCamera = Camera.main;
    	}
    	if(lookOnlyOnAwake){
			LookCamera();
    	}
    }
    
    public void Update() {
    	if(!lookOnlyOnAwake){
			LookCamera();
    	}
    }
    
    public void LookCamera() {
    	transform.LookAt(lookAtCamera.transform);
    }
}