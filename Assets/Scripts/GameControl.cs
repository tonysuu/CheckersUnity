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
    Color temp;
    GameObject curSelected;
    private CreateBoard board;
    private const int BOARD_SIZE = 4;

	// Use this for initialization
	void Start () {
        state = State.IDLE;
        activePlayer = PLAYER_A;
        board = gameObject.GetComponent<CreateBoard>();
        Transform temp = (Transform)board.cubes[1];
        temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        //printBoard(board.board);
    }
	
	// Ulayerdate is called once per frame
	void Update () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        switch (state){
            case State.IDLE:
                if (Input.GetMouseButtonDown(0))
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (activePlayer == PLAYER_A)
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player A"))
                            {
                                pieceSelect(hitInfo.collider.gameObject);
                                showOptions();
                                state = State.PIECE_SELECTED;
                            }
                   
                        }
                        else
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player B"))
                            {
                                pieceSelect(hitInfo.collider.gameObject);
                                showOptions();
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
                                if (isSelected(hitInfo.collider.gameObject))
                                {
                                    pieceDeselect(hitInfo.collider.gameObject);
                                    state = State.IDLE;
                                }
                                else
                                {
                                    pieceDeselect(curSelected);
                                    pieceSelect(hitInfo.collider.gameObject);
                                }
                            }
                            if (hitInfo.collider.gameObject.CompareTag("Cube"))
                            {
                                if (isValidMove(hitInfo.collider.gameObject))
                                {
                                    movePiece(hitInfo.collider.gameObject);
                                    pieceDeselect(curSelected);
                                    state = State.END_TURN;
                                }
                            }
                        }
                        else 
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player B"))
                            {
                                if (isSelected(hitInfo.collider.gameObject))
                                {
                                    pieceDeselect(hitInfo.collider.gameObject);
                                    state = State.IDLE;
                                }
                                else
                                {
                                    pieceDeselect(curSelected);
                                    pieceSelect(hitInfo.collider.gameObject);
                                }
                            }
                            if (hitInfo.collider.gameObject.CompareTag("Cube"))
                            {
                                if (isValidMove(hitInfo.collider.gameObject))
                                {
                                    movePiece(hitInfo.collider.gameObject);
                                    pieceDeselect(curSelected);
                                    state = State.END_TURN;
                                }
                            }
                        }
                    }
                }
                break;
            case State.END_TURN:
                curSelected = null;
                state = State.IDLE;
                if (activePlayer == PLAYER_A)
                    activePlayer = PLAYER_B;
                else
                    activePlayer = PLAYER_A;
                break;
            default:
                break;
        }
        
	}

    void pieceSelect(GameObject selected)
    {
        MeshRenderer renderer = selected.GetComponent<MeshRenderer>();
        Color color = renderer.material.GetColor("_Color");
        color.a = 0.4f;
    
        renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.SetFloat("_Mode", 2);
        renderer.material.SetColor("_Color", color);
        curSelected = selected;
    }
    bool isValidMove(GameObject dest)
    {
        return true;
    }
    void pieceDeselect(GameObject selected)
    {
        MeshRenderer renderer = selected.GetComponent<MeshRenderer>();
        Color color = renderer.material.GetColor("_Color");
        color.a = 1;

        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.SetFloat("_Mode", 0);
        renderer.material.SetColor("_Color", color);
        curSelected = null;
    }
    bool isSelected(GameObject piece)
    {
        return (piece.GetComponent<MeshRenderer>().material.color.a == 0.4f);
    }
    void movePiece(GameObject dest)
    {
        Vector3 destination = dest.transform.position;
        Vector3 start = curSelected.transform.position;
        start.x = destination.x;
        start.z = destination.z;
        curSelected.transform.position = start;
    }
    void printBoard(GameObject[,] board)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == null)
                    print("null ");
                else
                    print(board[i, j].tag + "  ");
            }
            print("\n");
        }
    }
    void showOptions()
    {
        int x, z;
        if (curSelected != null)
        {
            x = (int)curSelected.transform.position.x;
            z = (int)curSelected.transform.position.z;
            if (curSelected.CompareTag("Player A"))
            {
                //left and down
                if (x+1 <= 3 && z-1 >= 0)
                {
                    if (board.board[x+1,z-1] == null)
                    {

                    }
                    else if (board.board[x+1,z-1].CompareTag("Player B"))
                    {

                    }
                }     
            }
            else
            {

            }
        }
    }
}
