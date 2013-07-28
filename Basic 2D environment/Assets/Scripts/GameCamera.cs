using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	public float trackSpeed;
	
	private Transform target;
	
	
	public void setTarget(Transform t){
		target = t;
	}
	
	//Same as update method except it happens after all the other update methods
	void LateUpdate(){
		if(target){
			float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);
			transform.position = new Vector3(x,y, transform.position.z);
		}
	}
	
	//Incrase n towards target by speed
	private float IncrementTowards(float n, float target, float speed){
		if(n == target){
			return n;	
		} else {
			//must n be increased or decreased to get closer to target
			float dir = Mathf.Sign(target - n); 	
			n += speed * Time.deltaTime * dir;
			
			// if n has now passed target then return target, otherwise return n
			return (dir == Mathf.Sign(target - n))? n : target;
		}
	}
	
}
