using UnityEngine;

public class cameraScript : MonoBehaviour
{
	private const float Y_ANGLE_MIN = 0.0f;
	private const float Y_ANGLE_MAX = 35.0f;

	public Transform lookAt;
	public Transform camTransform;

	private Camera cam;

	private float distance = 10.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensivityX = 0.15f;
	private float sensivityY = 0.15f;

	private void start()
	{
		camTransform = transform;
		cam = Camera.main;
	}

	private void Update()
	{
		currentX += 1 * Input.GetAxis("MouseX") * sensivityX;
		currentY += -1 * Input.GetAxis("MouseY") * sensivityY;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}

	private void LateUpdate()
	{
		Vector3 dir = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(currentY, currentX+90, 0);
		camTransform.position = lookAt.position + rotation * dir;

		camTransform.LookAt(lookAt.position);
	}

}