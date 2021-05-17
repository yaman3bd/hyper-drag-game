using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using DG.Tweening;
public class CameraFollow : MonoBehaviour
{
 	// Should the camera follow the target
	[SerializeField] bool follow = false;
	public bool Rotate;
	public bool Rotate1;
	public float minY;
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
	[Range(0, 10)]
	[SerializeField] float lerpPositionMultiplierNew = 1f;
	[Range(0, 10)]
	[SerializeField] float lerpRotationMultiplierNew = 1f;

	// Speedometer


	CarController vehicle;

	void Start()
	{
		LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
		target = GameObject.FindObjectOfType<PlayerCarController>().transform;
		vehicle = target.GetComponent<CarController>();
		if (vehicle != null)
		{
			vehicle.Handbrake = false;
		}
	}

	private void OnRaceStarted()
	{
		LoadedLevelManager.Instance.OnRaceEnded -= OnRaceStarted;
		
		offset = new Vector3(8.41f, 8.0f, -11);
		offse1t = new Vector3(1, -3f, 0);

		DOTween.To(() => lerpPositionMultiplier, x => lerpPositionMultiplierNew = x, 10, 1f);
		DOTween.To(() => lerpRotationMultiplier, x => lerpRotationMultiplier = x, 10, 1f);

	}
	public void SwitchView(bool raceStart)
	{
		if (raceStart)
		{

			offset = new Vector3(8.41f, 8.0f, -11);
			offse1t = new Vector3(1, -3f, 0);
			DOTween.To(() => lerpPositionMultiplier, x => lerpPositionMultiplierNew = x, 10, 0.5f);
			DOTween.To(() => lerpRotationMultiplier, x => lerpRotationMultiplier = x, 10, 0.5f);

			//lerpPositionMultiplier = lerpPositionMultiplierNew;
			//lerpRotationMultiplier = lerpRotationMultiplierNew;
        }
        else
        {
			offset = new Vector3(14.1f, 11.45f, 0);
			offse1t = new Vector3(0, -6, 0);
			lerpPositionMultiplier = 5;
			lerpRotationMultiplier = 3;
		}
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
			SwitchView(true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
			SwitchView(false);

		}
    }

    void FixedUpdate()
	{
		// If we don't follow or target is null return
		if (!follow || target == null) return;




		// Save transform localy
		Quaternion curRot = transform.rotation;
		  var tPos = target.position + target.TransformDirection(offset);

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
		if (tPos.y < minY)
		{
			tPos.y = minY;
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
