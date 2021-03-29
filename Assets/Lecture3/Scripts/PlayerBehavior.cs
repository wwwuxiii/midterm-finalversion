using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float speed;
    public float jumpHeight;
    public float gravityMutiplier;
    public float jumpMultiplier;

    public bool escaped = false;

    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    SpriteRenderer myRenderer;

    public Sprite jumpSprite;
    public Sprite walkSprite;

    float moveDir = 1;
    bool onFloor = true;
    float bubbleNum = 0;
    
    public enum directionState{
        up,
        down,
        left,
        right,
        none
    }
    public directionState currentState = directionState.none;


    public static bool faceRight = true;

  
    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        myCollider = gameObject.GetComponent<BoxCollider2D>();

        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        if(escaped){
            myBody.gravityScale = 0;
            speed = 8;
            CheckKey();
            MoveMe();
        }
        else{
            if(onFloor && myBody.velocity.y > 0){
               onFloor = false;
             //Debug.Log("Set Onfloor to false!");
            }
            CheckKeys();
            HandleMovement();
            JumpPhysics();
        }
    }

    void CheckKeys(){
        if(Input.GetKey(KeyCode.D)){
            moveDir = 1;
            faceRight = true;
             myRenderer.flipX = true;
        }
        else if(Input.GetKey(KeyCode.A)){
            moveDir = -1;
            faceRight = false;
            myRenderer.flipX = false;
        }
        else{
            moveDir = 0;
        }
        if(Input.GetKey(KeyCode.W) && onFloor){
            myRenderer.sprite = jumpSprite;
            myBody.velocity = new Vector3(myBody.velocity.x, jumpHeight);
            //Debug.Log("Jump!");
        }
        else if(!Input.GetKey(KeyCode.W) && myBody.velocity.y > 0){
            myBody.velocity += Vector2.up * Physics.gravity.y * (jumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    void JumpPhysics(){
        if(myBody.velocity.y < 0){
            myBody.velocity += Vector2.up * Physics.gravity.y * (gravityMutiplier - 1f) * Time.deltaTime;
        }
    }

    void HandleMovement(){
        myBody.velocity = new Vector3(moveDir * speed, myBody.velocity.y);
    }

    void CheckKey(){
        if(Input.GetKey(KeyCode.W)){
            currentState = directionState.up;
        }
        else if(Input.GetKey(KeyCode.S)){
            currentState = directionState.down;
        }
        else if(Input.GetKey(KeyCode.A)){
            currentState = directionState.left;
            
            myRenderer.flipX = false;
        }  
        else if(Input.GetKey(KeyCode.D)){
            currentState = directionState.right;
            
             myRenderer.flipX = true;
        }
        else{
            currentState = directionState.none;
        }
    }

    void MoveMe(){
        switch (currentState){
            case directionState.up:
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                Debug.Log("W key pressed");
                break;
            case directionState.down:
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                Debug.Log("S key pressed");
                break;
            case directionState.left:
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                Debug.Log("A key pressed");
                break;
            case directionState.right:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                Debug.Log("D key pressed");
                break;
            default:
                Debug.Log("No key pressed");
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo){
        if(collisionInfo.gameObject.tag == "Floor"){
            myRenderer.sprite = walkSprite;
            onFloor = true;
            //Debug.Log("Set onfloor to true!");
        }

        if(collisionInfo.gameObject.tag == "Trigger" && (bubbleNum == 1)){
            Debug.Log("bubble =1");
            Destroy(GameObject.Find("Square"));
        }
        if(collisionInfo.gameObject.tag == "Bubble"){
            Destroy(GameObject.Find("Bubble"));
            bubbleNum = bubbleNum + 1; 
        }
        if(collisionInfo.gameObject.tag == "Trigger1" && (bubbleNum == 2)){
            Debug.Log("bubble = 2");
            Destroy(GameObject.Find("Square1"));
        }
        if(collisionInfo.gameObject.tag == "Bubble1"){
            Destroy(GameObject.Find("Bubble1"));
            bubbleNum = bubbleNum + 1; 
        }
        if(collisionInfo.gameObject.tag == "Trigger2" && (bubbleNum == 3)){
            Debug.Log("bubble = 3");
            Destroy(GameObject.Find("Square2"));
            escaped = true;
        }
        if(collisionInfo.gameObject.tag == "Bubble2"){
            Destroy(GameObject.Find("Bubble2"));
            bubbleNum = bubbleNum + 1; 
        }        
    }

    
}
