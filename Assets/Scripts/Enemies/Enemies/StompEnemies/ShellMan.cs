using UnityEngine;
using System.Collections;

public class ShellMan : MonoBehaviour {
	public Sprite withoutShell;
	public float moveSpeed, knockForce, jumpSpeed,  initialFreezeTime, activeMoveSpeed;
	public bool isMirrored = false, deShelled, invulnerable;
	Rigidbody2D rb2D;
	public int health = 6, damage = 3, invulnerabilityTime;

	void Start() {
		rb2D = GetComponent<Rigidbody2D> ();
		initialFreezeTime = gameObject.GetComponent<SpawnProperties> ().initialFreezeTime;
		if (initialFreezeTime > 0)
			Invoke ("InitializeMoveSpeed", initialFreezeTime);
	}

	void FixedUpdate() {
		if (activeMoveSpeed > 0) {
			if (isMirrored) {
				rb2D.velocity = new Vector2 (-1 * activeMoveSpeed, rb2D.velocity.y);
			} else {
				rb2D.velocity = new Vector2 (activeMoveSpeed, rb2D.velocity.y);
			}
		}
	}

	void OnBecameVisible() {
		if (initialFreezeTime == 0)
			InitializeMoveSpeed ();
	}

	void OnCollisionEnter2D(Collision2D col) {
		switch(col.gameObject.tag) {

		case "Char":
			if (!col.gameObject.GetComponent<CharStomp> ().groundStomping) {
				col.gameObject.GetComponent<CharHealth> ().TakeDamage (damage, gameObject, knockForce);
			}
			GetMirrored();
			break;

		case "SmallCritter":
		case "JumpingCritter":
		case "HardEnemy":
		case "BigEyeGuy":
		case "CrawlerCritter":
		case "ShellMan":
		case "Wall":
		case "Door":
		case "Barrier":
			GetMirrored ();
			break;

		case "PickupableItem":
			if (col.gameObject.GetComponent<PickUpableItem> ().GetItemType () == "Rock") {
				if (col.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude >= 3.0f && deShelled) {
					TakeDamage (col.gameObject.GetComponent<PickUpableItem> ().damage);
				}
			}
			GetMirrored ();
			break;
		}
	}

	/**
	 * Mirrors the enemy and therefor makes it change direction.
	 */
	void GetMirrored() {
		if(!isMirrored) {
			transform.rotation = Quaternion.Euler(0, 180, 0);
			isMirrored = true;
		}
		else {
			transform.rotation = Quaternion.Euler(0, 0, 0);
			isMirrored = false;
		}
	}

	void BreakShell() {
		//Change sprite into the DeShelled one and play relevant things.
		GetComponent<SpriteRenderer>().sprite = withoutShell;
		deShelled = true;
		damage = 5;
		moveSpeed += 3;
		knockForce += 3;
		jumpSpeed += 3;
	}

	/**
	 * Method called when enemy is hit by the player
	 */
	public void TakeDamage(int damage) {
		if (!invulnerable) {
			//Play a sound and animation.
			health -= damage;
			invulnerable = true;
			Invoke ("SetVulnerable", invulnerabilityTime);
			if (health <= 0) {
				//Enemy is dead, play animation and sound.
				int ranNumb = Random.Range (0, 60);
				if (ranNumb < 20) {
					Instantiate (Resources.Load ("HealthDrop"), transform.position, Quaternion.identity);
				} else if (ranNumb < 40) {
					Instantiate (Resources.Load ("EnergyDrop"), transform.position, Quaternion.identity);
				}
				Destroy (this.gameObject);
			}
			GetMirrored ();
		}
	}

	void SetVulnerable() {
		invulnerable = false;
	}
	public void Jump () {
		if (Random.Range (0, 250) < 5) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpSpeed);
		}
	}

	void InitializeMoveSpeed() {
		activeMoveSpeed = moveSpeed;
	}
}