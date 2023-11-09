using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
   public float power = 20f;
   public Rigidbody2D drag;
   public Vector2 minPower;
   public Vector2 maxPower;

   public float maxSpeed = 12f;
   public Transform Diamond;

   [SerializeField]
   public float rotationSpeed = 10.0f;

   Trajectory tl;

   Camera cam;
   Vector3 force;
   Vector3 startPoint;
   Vector3 endPoint;


   private void Start(){
    cam = Camera.main;
    tl = GetComponent<Trajectory>();
   }

   

   private void Update(){
    
    if (Input.GetMouseButtonDown(0)){
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("Start Point: "+ startPoint);
        startPoint.z = 15;
        maxSpeed = 0;
        
    }

    if (Input.GetMouseButton(0)){
      // This section gets the mouse position to render the line at the back of the spaceship
      Vector3 SpaceShipStartPoint =  GameObject.Find("Diamond").transform.position;
      Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
      currentPoint.z = 15;
      Vector3 SpaceShipEndPoint = currentPoint + (SpaceShipStartPoint - startPoint);
      tl.RenderLine(SpaceShipEndPoint, SpaceShipStartPoint);


      // This section handles spaceship rotation during the targeting phase

      Vector3 targetDirection = SpaceShipStartPoint - SpaceShipEndPoint;
      float singleStep = rotationSpeed * Time.deltaTime;
      float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle-90f, Vector3.forward) ;
    }

    if (Input.GetMouseButtonUp(0)){
      endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
      endPoint.z = 15;
      Debug.Log("End Point: " + endPoint);
      maxSpeed = 12f;

      force = new Vector3(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
      drag.AddForce(force * power, ForceMode2D.Impulse);

      Debug.Log("Force: " + force);

      tl.EndLine();
      FuelCounter.fuelCount -= 1;


      // This code is meant to rotate the Diamond/ship as its direction changes due to gravitational forces, but does not work
      float singleStep = rotationSpeed * Time.deltaTime;
      float angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle-90f, Vector3.forward) ;
    }
    
  }
  void FixedUpdate(){

    if(GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed){

        GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
    }
  }
}
