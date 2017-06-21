using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltSkidSteer : MonoBehaviour {
public ConfigurableJoint Right,Left;
Rigidbody rb;
public float LoopDist=0.1f;
Transform L,R;
	// Use this for initialization
	void Start () {
		rb=GetComponent<Rigidbody>();
		L=Left.gameObject.transform;
		R=Right.gameObject.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.AddRelativeForce(new Vector3(0,0,Input.GetAxis("Vertical")*50));
		rb.AddRelativeTorque(new Vector3(0,Input.GetAxis("Horizontal")*500,0));
		if(R.localPosition.z>LoopDist)//R.localPosition=new Vector3(0,0,-LoopDist);
		R.GetComponent<Rigidbody>().position=R.TransformPoint(new Vector3(0,0,-LoopDist));
		if(R.localPosition.z<-LoopDist)//R.localPosition=new Vector3(0,0,+LoopDist);
		R.GetComponent<Rigidbody>().position=R.TransformPoint(new Vector3(0,0,LoopDist));
		if(L.localPosition.z>LoopDist)//L.localPosition=new Vector3(0,0,-LoopDist);
		L.GetComponent<Rigidbody>().position=L.TransformPoint(new Vector3(0,0,-LoopDist));
		if(L.localPosition.z<-LoopDist)//L.localPosition=new Vector3(0,0,+LoopDist);
		L.GetComponent<Rigidbody>().position=L.TransformPoint(new Vector3(0,0,LoopDist));
	}
}
