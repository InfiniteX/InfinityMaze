using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerConteroller : MonoBehaviour {
	
	//Player Handling
	public float speed = 8;
	public float acceleration = 30;
	public float gravity = 20;
	public float jumpHeight = 12;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	
	private PlayerPhysics playerPhysics;
	
	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(playerPhysics.movementStopped){
			targetSpeed = 0;
			currentSpeed = 0;
		}
		
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);
		
		if(playerPhysics.grounded){
			amountToMove.y = 0;
			
			//Jump
			if(Input.GetButtonDown("Jump")){
				amountToMove.y = jumpHeight;	
			}
		}
		
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove *Time.deltaTime);
	}
	
	
	//Increase n towards target speed
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
