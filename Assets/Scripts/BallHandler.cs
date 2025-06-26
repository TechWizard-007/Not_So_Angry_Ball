using UnityEngine;
using UnityEngine.InputSystem;


public class BallHandler : MonoBehaviour
{
    private Camera MainCam; //why? bacuse we wanna convert our pixels( means touch input) to unity units(means worls space) for which unity prvides a maincamera property! so for using that property of main camera we have to take reference of the main camera
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] float timeToDetach;
    [SerializeField] private float respawnDelay;

    [SerializeField] private Rigidbody2D ballRigidbody; // ye bhi rakh hi lo nahi to agar start me hi call kiya to kuchh referencing error aa rha hai
    [SerializeField] private SpringJoint2D ballSpring;


    
    private bool isDragging;
    void Start()
    {
        MainCam = Camera.main;
        //SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballRigidbody == null) { return;  }

        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                Launch();
            }
            isDragging = false;

            
            return;
        }

        isDragging = true;
        ballRigidbody.bodyType = RigidbodyType2D.Kinematic;
        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 WorldPos = MainCam.ScreenToWorldPoint(touchPos); //conversion of pixels to unity units

        ballRigidbody.position = WorldPos;
        //Debug.Log(WorldPos);
    }

    private void Launch()
    {
        ballRigidbody.bodyType = RigidbodyType2D.Dynamic;
        ballRigidbody = null;

        Invoke("DetachBall",timeToDetach); //invokes a given function "function_name" after a the given duration of time.
        
    }

    private void DetachBall()
    {
        ballSpring.enabled = false;
        ballSpring = null;

       
        Invoke("SpawnNewBall", respawnDelay);
        
        

    }

    private void SpawnNewBall()
    {
        
        GameObject ballInstance = Instantiate(ballPrefab,pivot.position,Quaternion.identity);

        ballRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        ballSpring = ballInstance.GetComponent<SpringJoint2D>();

        ballSpring.connectedBody = pivot;
    }
}
