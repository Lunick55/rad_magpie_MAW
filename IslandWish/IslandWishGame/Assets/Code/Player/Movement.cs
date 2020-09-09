using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[Header("Important Basics")]
	[SerializeField] GameObject playerObj;
	[SerializeField] Transform playerTrans;
	[SerializeField] Transform playerHead; //for look stuff
	[SerializeField] Camera playerCamera;
	[SerializeField] CharacterController cc;
	private Animator anim;

	[Header("Player Stats")]
	[SerializeField] float playerSpeed = 1;
	[SerializeField] float playerSprintSpeed = 2, playerJumpSpeed = 1, playerDashSpeed = 3, dashDistance = 1, gravity = 9.8f;
	[SerializeField] Player player;
	float vSpeed = 0;
	Vector3 inputDir = Vector3.zero; //the vector determining player direction
	Vector3 dashStartPosition = Vector3.zero;
	Vector3 dashDestination = Vector3.zero;
	[SerializeField] float dashSafetyTimer = 1;
	private float timer = 0;

	Vector3 lookDirection = Vector3.zero;
	bool grounded = false;
	bool dashing = false;
	bool acceptInput = true;

    // Start is called before the first frame update
    void Start()
    {
		anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {



    }

	private void FixedUpdate()
	{
		if (player.canMove)
		{
			RotateToMouse();

			//get the direction relative to the camera
			Vector3 camForward = playerCamera.transform.forward;
			Vector3 camRight = playerCamera.transform.right;
			camForward.y = camRight.y = 0;
			camForward.Normalize();
			camRight.Normalize();

			//apply speed
			if (acceptInput)
			{
				inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
				//all for them anims baybee
				//prepare the corrected "forward vector"
				Vector3 forwardVec = Vector3.forward;
				forwardVec = camRight * forwardVec.x + camForward * forwardVec.z;
				//get the angle between where the player is looking and the corrected vector
				float angle = Vector3.SignedAngle(forwardVec, lookDirection, Vector3.down);
				Vector3 directionMoving = inputDir;
				//use the corrected angle the rotate the moving vector
				directionMoving = Quaternion.AngleAxis(angle, Vector3.up) * directionMoving;

				anim.SetFloat("MovementForward", directionMoving.z);
				anim.SetFloat("MovementSide", directionMoving.x);


				if (inputDir != Vector3.zero)
				{
					AudioManager.Instance.SafePlay("PCWalking");
					anim.SetBool("Moving", true);
					inputDir.Normalize();
				}
				else
				{
					anim.SetBool("Moving", false);
				}
				inputDir = camRight * inputDir.x + camForward * inputDir.z;

				if (Input.GetKey(KeyCode.Space) && !dashing && inputDir != Vector3.zero)
				{
					print("DASH");
					AudioManager.Instance.Play("PCDash");

					dashing = true;
					acceptInput = false;
					inputDir *= playerDashSpeed;

					dashStartPosition = playerTrans.position;
					dashDestination = playerTrans.position + (inputDir.normalized * dashDistance);

				}
				else if (Input.GetKey(KeyCode.LeftShift))
				{
					inputDir *= playerSprintSpeed;
				}
				else
				{
					inputDir *= playerSpeed;
				}
			}

			grounded = cc.isGrounded;			
		
			if (grounded)
			{
				vSpeed = -0.1f;

				//if (Input.GetKeyDown(KeyCode.Space))
				//{
				//	Jump();
				//}
			}

			//Fall down
			inputDir.y = (vSpeed -= (gravity * Time.deltaTime));// Time.deltaTime));

			cc.Move(inputDir * Time.deltaTime);
		}
		else
		{
			inputDir = Vector3.zero;
			anim.SetBool("Moving", false);
		}

		if (!dashing)
		{
			inputDir = Vector3.zero;
		}
		else 
		{
			timer += Time.deltaTime;
			if (((playerTrans.position - dashStartPosition).magnitude >= dashDistance) || timer > dashSafetyTimer)
			{
				timer = 0;
				dashing = false;
				acceptInput = true;
			}
		}
	}

	private void Jump()
	{
		vSpeed = playerJumpSpeed;
		inputDir.y = (vSpeed -= Time.deltaTime);
	}

	public void Teleport(Vector3 position)
	{
		cc.enabled = false;
		transform.position = position;
		cc.enabled = true;
	}

	private void RotateToMouse()
	{
		//Create vector from the object to the mouse in screen coords
		Vector3 lineToMouse = Input.mousePosition - playerCamera.WorldToScreenPoint(playerHead.position);
		//transform the line from the xy plane to the xz plane (rotate around x axis) also accounting for the -45 degree rotation of the camera (iso view)
		lineToMouse = Quaternion.Euler(90, 0, 45) * lineToMouse;
		//Debug.DrawLine(Vector3.zero, lineToMouse.normalized);

		//nullify any rotations along the y axis. don't need it
		lineToMouse = new Vector3(lineToMouse.x, 0, lineToMouse.z);

		lookDirection = lineToMouse;
		//apply!
		transform.rotation = Quaternion.LookRotation(lineToMouse, Vector3.up);
	}

	private void OnDrawGizmosSelected()
	{
#if UNITY_EDITOR
		UnityEditor.Handles.DrawWireDisc(dashDestination, Vector3.up, 1);
#endif
	}
}
