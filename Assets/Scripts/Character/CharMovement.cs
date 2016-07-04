﻿using UnityEngine;
using System.Collections;

//This class handles the movement for the player, including walking and jumps.
public class CharMovement : MonoBehaviour {
	//Fields
	CharStatus status;
	Rigidbody2D rigidBody2D;
	public float moveSpeed, airSpeed, maxMoveSpeed, moveForce;
	float axisH;

	//Start is called once on initialization
	void Start() {
		status = GetComponent<CharStatus> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();
	}

	//FixedUpdate executes with a set time interval and calculates all physics equations required.
	void FixedUpdate() {
		axisH = Input.GetAxis("Horizontal");
		//Test if trying to move towards wall and stop movement as well as decrease negative y velocity.
		if (status.onLeftWall && axisH < 0) {
			if (rigidBody2D.velocity.y < -1) {
				rigidBody2D.velocity = new Vector2 (0, -1);
			} else {
				rigidBody2D.velocity = new Vector2 (0, rigidBody2D.velocity.y);
			}
		} else if (status.onRightWall && axisH > 0) {
			if (rigidBody2D.velocity.y < -1) {
				rigidBody2D.velocity = new Vector2 (0, -1);
			} else {
				rigidBody2D.velocity = new Vector2 (0, rigidBody2D.velocity.y);
			}
		}
		//Movement for when in air.
		else if (!status.onGround) {
			if (Mathf.Abs (rigidBody2D.velocity.x) < maxMoveSpeed) {
				rigidBody2D.AddForce (Vector2.right * axisH * airSpeed);
			} else if (Mathf.Sign (axisH) != Mathf.Sign (rigidBody2D.velocity.x)) {
				rigidBody2D.AddForce (Vector2.right * axisH * airSpeed);
			}
		}
		//Movement
		else if (Mathf.Abs (rigidBody2D.velocity.x) < maxMoveSpeed) {
			rigidBody2D.AddForce (Vector2.right * axisH * moveSpeed);
		} else if (Mathf.Sign (axisH) != Mathf.Sign (rigidBody2D.velocity.x)) {
			rigidBody2D.AddForce (Vector2.right * axisH * moveSpeed);
		}
		//Decrease velocity when not trying to move.
		if (axisH == 0 && status.onGround) {
			if (rigidBody2D.velocity.x > maxMoveSpeed / 3) {
				rigidBody2D.AddForce (Vector2.left * moveSpeed);
			}
			else if (rigidBody2D.velocity.x < maxMoveSpeed / -3) {
				rigidBody2D.AddForce (Vector2.right * moveSpeed);
			}
		}
	}
}