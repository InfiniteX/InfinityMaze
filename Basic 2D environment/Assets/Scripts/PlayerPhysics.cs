using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;
	
	
	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;
	
	private float skin = 0.005f;
	
	Ray ray;
	RaycastHit hit;
	
	void Start(){
		collider = GetComponent<BoxCollider>();
		size = collider.size;
		center = collider.center;
	}
	
	public void Move(Vector2 moveAmount){
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 position = transform.position;
		
		
		//Up and down collisions
		grounded = false;
		
		for(int i = 0; i<3; i++){
			float direction = Mathf.Sign(deltaY);
			
			//left, center and then rightmost point of collider
			float x = (position.x + center.x - size.x/2) + size.x/2 * i;
			
			// bottoms of the collider
			float y = position.y + center.y + size.y/2 * direction;
			
			ray = new Ray(new Vector2(x,y), new Vector2(0, direction));
			Debug.DrawRay(ray.origin, ray.direction);
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask)){
				//Get distance between player and ground
				float distance = Vector3.Distance(ray.origin, hit.point);
				
				//Stop player's downwards movement after coming within skin width of the collider
				if( distance > skin){
					deltaY = distance * direction - skin * direction;	
				} else {
					deltaY = 0;
				}
				
				grounded = true;
				break;
			}
		}
		
		//Left and Right collisions
		movementStopped = false;
		for(int i = 0; i<3; i++){
			float direction = Mathf.Sign(deltaX);
			float x = position.x + center.x + size.x/2 * direction; //left, center and then rightmost point of collider
			float y = position.y + center.y - size.y/2 + size.y/2 * i ;// bottoms of the collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(direction, 0));
			Debug.DrawRay(ray.origin, ray.direction);
			
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask)){
				//Get distance between player and ground
				float distance = Vector3.Distance(ray.origin, hit.point);
				
				//Stop player's downwards movement after coming within skin width of the collider
				if( distance > skin){
					deltaX = distance * direction - skin * direction;	
				} else {
					deltaX = 0;
				}
				movementStopped = true;
				break;
			}
		}
		
		if( !grounded && !movementStopped){
			Vector3 playerDirection = new Vector3(deltaX, deltaY);
			Vector3 origin = new Vector3(position.x + center.x + size.x/2 * Mathf.Sign(deltaX), position.y + center.y + size.y/2 * Mathf.Sign(deltaY));
			
			ray = new Ray(origin, playerDirection.normalized);
			if(Physics.Raycast(ray, Mathf.Sqrt( deltaX * deltaX + deltaY * deltaY), collisionMask)){
				grounded = true;
				deltaY = 0;
			}
		}
		
		Vector2 finalTransform = new Vector2(deltaX, deltaY);
		transform.Translate(finalTransform);	
	}
}
