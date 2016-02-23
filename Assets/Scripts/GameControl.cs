using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
    Queue queue = new Queue();
    private const int PLAYER_A = 0;
    private const int PLAYER_B = 1;
    private int activePlayer;
    private bool show;
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
        show = false;
        
        //printBoard(board.board);
    }
	
	// Ulayerdate is called once per frame
	void Update () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        switch (state){
            case State.IDLE:
                restoreBoard();
                if (Input.GetMouseButtonDown(0))
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (activePlayer == PLAYER_A)
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player A"))
                            {
                                pieceSelect(hitInfo.collider.gameObject,true);
                                state = State.PIECE_SELECTED;
                                showOptions();
                            }
                   
                        }
                        else
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Player B"))
                            {
                                pieceSelect(hitInfo.collider.gameObject,true);
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
                                    pieceSelect(hitInfo.collider.gameObject,false);
                                    state = State.IDLE;
                                }
                                else
                                {
                                    pieceSelect(curSelected,false);
                                    pieceSelect(hitInfo.collider.gameObject,true);
                                }
                            }
                            if (hitInfo.collider.gameObject.CompareTag("Cube"))
                            {
                                if (isValidMove(hitInfo.collider.gameObject))
                                {
                                    movePiece(hitInfo.collider.gameObject);
                                    pieceSelect(curSelected,false);
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
                                    pieceSelect(hitInfo.collider.gameObject,false);
                                    state = State.IDLE;
                                }
                                else
                                {
                                    pieceSelect(curSelected,false);
                                    pieceSelect(hitInfo.collider.gameObject,true);
                                }
                            }
                            if (hitInfo.collider.gameObject.CompareTag("Cube"))
                            {
                                if (isValidMove(hitInfo.collider.gameObject))
                                {
                                    movePiece(hitInfo.collider.gameObject);
                                    pieceSelect(curSelected,false);
                                    state = State.END_TURN;
                                }
                            }
                        }
                    }
                }
                break;
            case State.END_TURN:
                curSelected = null;
                show = false;
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

    void pieceSelect(GameObject selected, bool select)
    {
        MeshRenderer renderer = selected.GetComponent<MeshRenderer>();
        if (select)
        {
            setTransparent(renderer, select);
            curSelected = selected;
        }
        else
        {
            setTransparent(renderer, select);
            curSelected = null;
        }
    }
    void setTransparent(MeshRenderer renderer, bool transparent)
    {
        Color color = renderer.material.GetColor("_Color");
        
        if (transparent)
        {
            color.a = 0.4f;
            renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.SetFloat("_Mode", 3);
            renderer.material.SetColor("_Color", color);
        }
        else
        {
            color.a = 1;
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.SetFloat("_Mode", 0);
            renderer.material.SetColor("_Color", color);
        }

    }

    bool isValidMove(GameObject dest)
    {
        return true;
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
                        Transform temp = (Transform)board.cubes[(x + 1) * 4 + z - 1];
                        Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 1) * 4 + z - 1);
                        queue.Enqueue(opt);
                        temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                        //board.cubes[(x + 1) * 4 + z - 1] = temp;
                        //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                    }
                    else if (board.board[x+1,z-1].CompareTag("Player B"))
                    {
                        if (x + 2 <= 3 && z-2 >= 0 && board.board[x+2,z-2] == null)
                        {
                            //Transform temp = (Transform)board.cubes[(x + 2) * 4 + z - 2];
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                    }
                }
                //right and down     
            }
            else if (curSelected.CompareTag("Player B"))
            {

            }
        }
    }
    void restoreBoard()
    {
        //print("funciton called");
        while (queue.Count != 0)
        {
            print("count "+queue.Count);
            Option opt = (Option)queue.Dequeue();
            print(opt.getIndex() + " " + opt.getColor());
            Transform temp = (Transform)board.cubes[opt.getIndex()];
            temp.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", opt.getColor());
            board.cubes[opt.getIndex()] = temp;
        }
    }
}
public class Option
{
    Color color;
    int index;
    public Option(Color color, int index)
    {
        this.color = color;
        this.index = index;
    }
    public Color getColor()
    {
        return color;
    }
    public int getIndex(){
        return index;
    }
}
