using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
    private const int PLAYER_A = 0;
    private const int PLAYER_B = 1;
    private int activePlayer;
    private enum State
    {
        IDLE, PIECE_SELECTED, END_TURN
    };
    private State state;
    Ray ray;
    RaycastHit hitInfo;
    Camera camera;

	// Use this for initialization
	void Start () {
        state = State.IDLE;
        activePlayer = PLAYER_A;
	}
	
	// Update is called once per frame
	void Update () {
        switch (state){
            case State.IDLE:
                camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

                if (Input.GetMouseButtonDown(0))
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (activePlayer == PLAYER_A)
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player A"))
                            {
                                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                                state = State.PIECE_SELECTED;
                            }
                        }
                        else
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player B"))
                            {
                                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                                state = State.PIECE_SELECTED;
                            }
                        }
                    }
                }
                break;
            case State.PIECE_SELECTED:
                if (Input.GetMouseButtonDown(0))
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (activePlayer == PLAYER_A)
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player A"))
                            {
                                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().material.co
                            }
                            if (hitInfo.collider.gameObject.CompareTag("Cube"))
                            {
                                isValidMove();
                            }
                        }
                        else 
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player B"))
                            {

                            }
                            if (hitInfo.collider.gameObject.CompareTag("Cube"))
                            {
                                isValidMove();
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
        
	}

    void pieceSelected(GameObject selected)
    {

    }
    void isValidMove()
    {

    }
}
