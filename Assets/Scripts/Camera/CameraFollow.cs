using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class CameraFollow : MonoBehaviour
{
 	// Should the camera follow the target
	[SerializeField] bool follow = false;
	public bool Rotate;
	public bool Rotate1;

	public bool Follow { get { return follow; } set { follow = value; } }

	// Current target
	[SerializeField] Transform target;


	// Offset from the target position
	[SerializeField] Vector3 offset;
	[SerializeField] Vector3 offse1t;

	// Camera speeds
	[Range(0, 10)]
	[SerializeField] float lerpPositionMultiplier = 1f;
	[Range(0, 10)]
	[SerializeField] float lerpRotationMultiplier = 1f;

	// Speedometer
	[SerializeField] TMP_Text speedometer;


	CarController vehicle;

	void Start()
	{
 		vehicle = target.GetComponent<CarController>();
		if (vehicle != null)
		{
			vehicle.Handbrake = false;
		}
	}




	void FixedUpdate()
	{
		// If we don't follow or target is null return
		if (!follow || target == null) return;




		// Save transform localy
		Quaternion curRot = transform.rotation;
		Vector3 tPos = target.position + target.TransformDirection(offset);

		// Look at the target
		if (Rotate)
		{

			Vector3 targetPostition = new Vector3(target.position.x,
								   this.transform.position.y,
								   target.position.z);
			targetPostition += offse1t;

			transform.LookAt(targetPostition);

		}
		if (Rotate1)
		{
			transform.LookAt(target.position);

		}

		// Keep the camera above the target y position
		if (tPos.y < target.position.y)
		{

			tPos.y = target.position.y;

		}

		// Set transform with lerp
		transform.position = Vector3.Lerp(transform.position, tPos, Time.fixedDeltaTime * lerpPositionMultiplier);
		if (Rotate || Rotate1)
			transform.rotation = Quaternion.Lerp(curRot, transform.rotation, Time.fixedDeltaTime * lerpRotationMultiplier);

		// Keep camera above the y:0.5f to prevent camera going underground
		//if (transform.position.y < 0.5f)
		//{
		//	transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		//}

		// Update speedometer
		if (speedometer != null && vehicle != null)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(((int)(vehicle.Speed)).ToString());
			sb.Append(" Kph");

			speedometer.text = sb.ToString();
		}
		else if (speedometer.text != "")
		{
			speedometer.text = "";
		}

	}
}
/*
 * save the defaul car ID
 * and assing it to the selected car ID
 * load car the car from the ID
 * if the player selects new car update the selected car ID 
 * requirements//
 * list to store all the cars data
 * 
 */
