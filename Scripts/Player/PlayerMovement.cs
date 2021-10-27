using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	int playerIndex;

	[SerializeField]
	Camera cam;

	public float speed = 6f;

	private Vector3 movement;
	private Animator anim;
	private Rigidbody playerRigidbody;
	private int floorMask;
	private float camRayLength = 100f;

	void Awake()
	{
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		float h = Input.GetAxisRaw("Horizontal" + playerIndex);
		float v = Input.GetAxisRaw("Vertical" + playerIndex);

		Move(h, v);
		Turning();
		Animating(h, v);
	}

	void Move(float h, float v)
	{
		movement.Set(h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition(transform.position + movement);
	}

	void Turning()
	{
		if (playerIndex == 1)
		{
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit floorHit;

			if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
			{
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;

				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
				playerRigidbody.MoveRotation(newRotation);
			}
		} else if(playerIndex == 2)
        {

			Vector3 pointAtNumpad = new Vector3(Input.GetAxisRaw("Numpad X"),0, Input.GetAxisRaw("Numpad Y"));

			Quaternion newRotation = Quaternion.LookRotation(pointAtNumpad);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void Animating(float h, float v)
	{
		bool walking = h != 0f || v != 0f;

		anim.SetBool("IsWalking", walking);
	}
}
