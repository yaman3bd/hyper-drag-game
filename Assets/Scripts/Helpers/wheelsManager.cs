using UnityEngine;

public class wheelsManager : MonoBehaviour{
    [Header("wheels")]
    [Range(.8f,1.7f)]public float Friction ;
    private float SidewaysFriction ;
    private float ForwardFriction ;


    private controller controller;

    [HideInInspector]public GameObject[] wheelObjects;
    [HideInInspector]public WheelCollider[] wheels;


    //updateWheels
    private Vector3 wheelPosition;
	private Quaternion wheelRotation ;


    private WheelFrictionCurve forwardFriction , sidewaysFriction;


    void Start(){
        SidewaysFriction = ForwardFriction = Friction;
        findComponents();

    }

    void findComponents(){
        controller = GetComponent<controller>();
        
        setUpWheels();
    }

    void Update(){
        SidewaysFriction = ForwardFriction = Friction;
        controller.ForwardStifness = ForwardFriction;
        controller.SidewaysStifness = SidewaysFriction;
        animateWheels();   

    }

    private void animateWheels (){



		for (int i = 0; i < wheels.Length; i++) {
			wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelObjects[i].transform.position = wheelPosition;
            wheelObjects[i].transform.rotation = wheelRotation;

        }
    }


    void setUpWheels(){
        WheelFrictionCurve curve ;

        for (int i = 0; i < wheels.Length; i++){

            curve = wheels[i].forwardFriction;

            curve.asymptoteValue = 1;
            curve.extremumSlip = 0.065f;
            curve.asymptoteSlip = 0.8f;
            curve.stiffness =  ForwardFriction * 2;
            wheels[i].forwardFriction = curve;
            
            curve = wheels[i].sidewaysFriction;

            curve.asymptoteValue = 1;
            curve.extremumSlip = 0.065f;
            curve.asymptoteSlip = 0.8f;
            curve.stiffness =  SidewaysFriction * 2;
            wheels[i].sidewaysFriction = curve;

        }
    }
    
}
