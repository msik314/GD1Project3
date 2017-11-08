using UnityEngine;

public class cameraScript : MonoBehaviour
{
	private const float Y_ANGLE_MIN = 0.0f;
	private const float Y_ANGLE_MAX = 35.0f;

	[SerializeField]private Transform lookAt;

	private Camera cam;

	[SerializeField]private float distance = 10.0f;
    [SerializeField]private float verticalOffset;
    
	private float currentX = 0.0f;
	private float currentY = 0.0f;
	private float sensivityX = 0.15f;
	private float sensivityY = 0.15f;

	private void start()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		currentX += 1 * Input.GetAxis("MouseX") * sensivityX;
		currentY += -1 * Input.GetAxis("MouseY") * sensivityY;

		currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        Vector3 dir = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(currentY, lookAt.rotation.eulerAngles.y, 0);
		transform.position = lookAt.position + new Vector3(0, verticalOffset, 0) + rotation * dir;
		transform.LookAt(lookAt.position+ new Vector3(0, verticalOffset, 0));
	}

}
