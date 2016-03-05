using UnityEngine;
using System;
using System.Collections;

public class GameControl : MonoBehaviour {
    Queue queue = new Queue();
    private const int PLAYER_A = 0;
    private const int PLAYER_B = 1;
    private int activePlayer;
    private bool swithchPlayer;
    private bool jump;
    private bool rotating;
    private double rotation;
    private double toTarget;
    private bool moving;
    private Vector3 start;
    private Vector3 destination;

    private Vector3 cameraPosition = new Vector3(-1, 5, BOARD_SIZE/2);
    private Vector3 cameraRotation = new Vector3(60, 90, 0);
    private Vector3 point = new Vector3(BOARD_SIZE / 2, 0, BOARD_SIZE / 2);


    private enum State
    {
        IDLE, PIECE_SELECTED, END_TURN, END_OF_GAME
    };

    private enum Show
    {
        NORMAL, KILL, KING
    }

    private State state;
    Ray ray;
    RaycastHit hitInfo;
    Camera camera;
    Color temp;
    GameObject curSelected;
    private CreateBoard board;
    private const int BOARD_SIZE = 6;

	// Use this for initialization
	void Start () {
        state = State.IDLE;
        activePlayer = PLAYER_A;
        board = gameObject.GetComponent<CreateBoard>();
        jump = false;
        swithchPlayer = true;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.transform.position = cameraPosition;
        camera.transform.Rotate(cameraRotation);
        rotating = false;
        rotation = 0;
        toTarget = 0;
        //printBoard(board.board);
    }
	
	// Ulayerdate is called once per frame
	void Update () {
        if (!rotating  && !moving)
        {
            switch (state)
            {
                case State.IDLE:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ray = camera.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (activePlayer == PLAYER_A)
                            {
                                if (hitInfo.collider.gameObject.CompareTag("Player A") || hitInfo.collider.gameObject.CompareTag("King A"))
                                {
                                    pieceSelect(hitInfo.collider.gameObject, true);
                                    state = State.PIECE_SELECTED;
                                    if (hitInfo.collider.gameObject.CompareTag("King A"))
                                        showOptions(Show.KING);
                                    else
                                        showOptions(Show.NORMAL);
                                }

                            }
                            else
                            {
                                if (hitInfo.collider.gameObject.CompareTag("Player B") | hitInfo.collider.gameObject.CompareTag("King B"))
                                {
                                    pieceSelect(hitInfo.collider.gameObject, true);
                                    state = State.PIECE_SELECTED;
                                    if (hitInfo.collider.gameObject.CompareTag("King B"))
                                        showOptions(Show.KING);
                                    else
                                        showOptions(Show.NORMAL);
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
                                if (hitInfo.collider.gameObject.CompareTag("Player A") || hitInfo.collider.gameObject.CompareTag("King A"))
                                {
                                    if (isSelected(hitInfo.collider.gameObject) && swithchPlayer)
                                    {
                                        pieceSelect(hitInfo.collider.gameObject, false);
                                        restoreBoard();
                                        state = State.IDLE;
                                    }
                                    else if (swithchPlayer)
                                    {
                                        pieceSelect(curSelected, false);
                                        pieceSelect(hitInfo.collider.gameObject, true);
                                        restoreBoard();
                                        if (hitInfo.collider.gameObject.CompareTag("King A"))
                                            showOptions(Show.KING);
                                        else
                                            showOptions(Show.NORMAL);
                                    }
                                }
                                if (hitInfo.collider.gameObject.CompareTag("Cube"))
                                {
                                    if (isValidMove(hitInfo.collider.gameObject))
                                    {
                                        movePiece(hitInfo.collider.gameObject);
                                        /*if (jump)
                                        {
                                            jump = false;
                                            state = State.PIECE_SELECTED;
                                            restoreBoard();
                                            print(curSelected.transform.position);
                                            showOptions(Show.KILL);
                                            print(queue.Count);
                                            if (queue.Count == 0)
                                            {
                                                pieceSelect(curSelected, false);
                                                state = State.END_TURN;
                                            }
                                        }
                                        else
                                        {*/
                                        pieceSelect(curSelected, false);
                                        state = State.END_TURN;
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                if (hitInfo.collider.gameObject.CompareTag("Player B") || hitInfo.collider.gameObject.CompareTag("King B"))
                                {
                                    if (isSelected(hitInfo.collider.gameObject) && swithchPlayer)
                                    {
                                        pieceSelect(hitInfo.collider.gameObject, false);
                                        restoreBoard();
                                        state = State.IDLE;
                                    }
                                    else if (swithchPlayer)
                                    {
                                        pieceSelect(curSelected, false);
                                        pieceSelect(hitInfo.collider.gameObject, true);
                                        restoreBoard();
                                        if (hitInfo.collider.gameObject.CompareTag("King B"))
                                            showOptions(Show.KING);
                                        else
                                            showOptions(Show.NORMAL);
                                    }
                                }
                                if (hitInfo.collider.gameObject.CompareTag("Cube"))
                                {
                                    if (isValidMove(hitInfo.collider.gameObject))
                                    {
                                        movePiece(hitInfo.collider.gameObject);
                                        /*if (jump)
                                        {
                                            jump = false;
                                            state = State.PIECE_SELECTED;
                                            restoreBoard();
                                            showOptions(Show.KILL);
                                            print(queue.Count);
                                            if (queue.Count == 0)
                                            {
                                                pieceSelect(curSelected, false);
                                                state = State.END_TURN;
                                            }
                                        }
                                        else
                                        {*/
                                        pieceSelect(curSelected, false);
                                        state = State.END_TURN;
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    break;
                case State.END_TURN:
                    if (jump)
                    {
                        jump = false;
                        state = State.PIECE_SELECTED;
                        restoreBoard();
                        print(curSelected.transform.position);
                        showOptions(Show.KILL);
                        print(queue.Count);
                        if (queue.Count == 0)
                        {
                            pieceSelect(curSelected, false);
                            state = State.END_TURN;
                        }
                    }
                    else
                    {
                        curSelected = null;
                        swithchPlayer = true;
                        state = State.IDLE;
                        restoreBoard();
                        checkWinner();

                        //disable rotating camera
                        if (activePlayer == PLAYER_A)
                        {
                            activePlayer = PLAYER_B;
                            //rotating = true;
                        }
                        else
                        {
                            activePlayer = PLAYER_A;
                            //rotating = true;
                        }
                    }
                    break;
                case State.END_OF_GAME:
                    break;
                default:
                    break;
            }
        }
        else
        {
            /*camera.transform.RotateAround(point, Vector3.up, Time.deltaTime * 150);
            rotation += Time.deltaTime * 150;
            if (rotation >= 180 + toTarget)
            {
                toTarget = 180 - rotation;
                rotating = false;
                rotation = 0;
            }*/
            curSelected.transform.position = Vector3.MoveTowards(curSelected.transform.position,destination,Time.deltaTime * 4);
            if (curSelected.transform.position == destination)
                moving = false;
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
            //curSelected = null;
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
            /*renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.DisableKeyword("_ALPHABLEND_ON");
            renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;*/
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
        Option toMove = new Option(dest.GetComponent<MeshRenderer>().material.GetColor("_Color"),
            ((int)dest.transform.position.x) * BOARD_SIZE + (int)dest.transform.position.z);
        return queue.Contains(toMove);
    }
    bool isSelected(GameObject piece)
    {
        return (piece.GetComponent<MeshRenderer>().material.color.a == 0.4f);
    }
    void movePiece(GameObject dest)
    {
        destination = dest.transform.position;
        start = curSelected.transform.position;

        //check for kill
        if (destination.x - start.x == 2 || destination.x - start.x == -2)
        {
            float x = (destination.x + start.x) / 2;
            float z = (destination.z + start.z) / 2;
            board.board[(int)x, (int)z].SetActive(false);
            board.board[(int)x, (int)z] = null;
            jump = true;
            swithchPlayer = false;
        }
        //check for king condition
        if (destination.x == 5 && curSelected.CompareTag("Player A"))
        {
            curSelected.tag = "King A";
        }
        if (destination.x == 0 && curSelected.CompareTag("Player B"))
        {
            curSelected.tag = "King B";
        }


        board.board[(int)destination.x, (int)destination.z] = curSelected;
        board.board[(int)start.x, (int)start.z] = null;

        destination.y = start.y;
        moving = true;
        /*start.x = destination.x;
        start.z = destination.z;
        curSelected.transform.position = start;*/
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
    void showOptions(Show mode)
    {
        int x, z;
        if (curSelected != null)
        {
            x = (int)curSelected.transform.position.x;
            z = (int)curSelected.transform.position.z;
            if (curSelected.CompareTag("Player A") || curSelected.CompareTag("King A"))
            {
                //left and down
                if (x + 1 < BOARD_SIZE && z - 1 >= 0)
                {
                    //regular move
                    if (board.board[x + 1, z - 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                    {
                        Transform temp = (Transform)board.cubes[(x + 1) * BOARD_SIZE + z - 1];
                        Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 1) * BOARD_SIZE + z - 1);
                        queue.Enqueue(opt);
                        temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                        //board.cubes[(x + 1) * 4 + z - 1] = temp;
                        //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                    }
                    //kill
                    else if (board.board[x + 1, z - 1] != null && (board.board[x+1,z-1].CompareTag("Player B") || board.board[x + 1, z - 1].CompareTag("King B")))
                    {
                        if (x + 2 < BOARD_SIZE && z-2 >= 0 && board.board[x+2,z-2] == null)
                        {
                            Transform temp = (Transform)board.cubes[(x + 2) * BOARD_SIZE + z - 2];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 2) * BOARD_SIZE + z - 2);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                    }
                }
                //right and down
                if (x + 1 < BOARD_SIZE && z + 1 < BOARD_SIZE)
                {
                    //regular move
                    if (board.board[x + 1, z + 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                    {
                        Transform temp = (Transform)board.cubes[(x + 1) * BOARD_SIZE + z + 1];
                        Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 1) * BOARD_SIZE + z + 1);
                        queue.Enqueue(opt);
                        temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                        //board.cubes[(x + 1) * 4 + z - 1] = temp;
                        //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                    }
                    //kill
                    else if (board.board[x + 1, z + 1] != null && (board.board[x + 1, z + 1].CompareTag("Player B") || board.board[x + 1, z + 1].CompareTag("King B")))
                    {
                        if (x + 2 < BOARD_SIZE && z + 2 < BOARD_SIZE && board.board[x + 2, z + 2] == null)
                        {
                            Transform temp = (Transform)board.cubes[(x + 2) * BOARD_SIZE + z + 2];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 2) * BOARD_SIZE + z + 2);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                    }
                }
                if (curSelected.CompareTag("King A"))
                {
                    //left and up
                    if (x - 1 >= 0 && z - 1 >= 0)
                    {
                        //regular move
                        if (board.board[x - 1, z - 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                        {
                            Transform temp = (Transform)board.cubes[(x - 1) * BOARD_SIZE + z - 1];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 1) * BOARD_SIZE + z - 1);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //board.cubes[(x + 1) * 4 + z - 1] = temp;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                        //kill
                        else if (board.board[x - 1, z - 1] != null && (board.board[x - 1, z - 1].CompareTag("Player B") || board.board[x - 1, z - 1].CompareTag("King B")))
                        {
                            if (x - 2 >= 0 && z - 2 >= 0 && board.board[x - 2, z - 2] == null)
                            {
                                Transform temp = (Transform)board.cubes[(x - 2) * BOARD_SIZE + z - 2];
                                Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 2) * BOARD_SIZE + z - 2);
                                queue.Enqueue(opt);
                                temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                                //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                            }
                        }
                    }
                    //right and up
                    if (x - 1 >= 0 && z + 1 < BOARD_SIZE)
                    {
                        //regular move
                        if (board.board[x - 1, z + 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                        {
                            Transform temp = (Transform)board.cubes[(x - 1) * BOARD_SIZE + z + 1];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 1) * BOARD_SIZE + z + 1);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //board.cubes[(x + 1) * 4 + z - 1] = temp;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                        //kill
                        else if (board.board[x - 1, z + 1] != null && (board.board[x - 1, z + 1].CompareTag("Player B") || board.board[x - 1, z + 1].CompareTag("King B")))
                        {
                            if (x - 2 >= 0 && z + 2 < BOARD_SIZE && board.board[x - 2, z + 2] == null)
                            {
                                Transform temp = (Transform)board.cubes[(x - 2) * BOARD_SIZE + z + 2];
                                Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 2) * BOARD_SIZE + z + 2);
                                queue.Enqueue(opt);
                                temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                                //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                            }
                        }
                    }
                }
            }
            else if (curSelected.CompareTag("Player B") || curSelected.CompareTag("King B"))
            {
                //left and up
                if (x - 1 >= 0 && z - 1 >= 0)
                {
                    //regular move
                    if (board.board[x - 1, z - 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                    {
                        Transform temp = (Transform)board.cubes[(x - 1) * BOARD_SIZE + z - 1];
                        Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 1) * BOARD_SIZE + z - 1);
                        queue.Enqueue(opt);
                        temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                        //board.cubes[(x + 1) * 4 + z - 1] = temp;
                        //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                    }
                    //kill
                    else if (board.board[x - 1, z - 1] != null && (board.board[x - 1, z - 1].CompareTag("Player A") || board.board[x - 1, z - 1].CompareTag("King A")))
                    {
                        if (x - 2 >= 0 && z - 2 >= 0 && board.board[x - 2, z - 2] == null)
                        {
                            Transform temp = (Transform)board.cubes[(x - 2) * BOARD_SIZE + z - 2];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 2) * BOARD_SIZE + z - 2);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                    }
                }
                //right and up
                if (x - 1 >= 0 && z + 1 < BOARD_SIZE)
                {
                    //regular move
                    if (board.board[x - 1, z + 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                    {
                        Transform temp = (Transform)board.cubes[(x - 1) * BOARD_SIZE + z + 1];
                        Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 1) * BOARD_SIZE + z + 1);
                        queue.Enqueue(opt);
                        temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                        //board.cubes[(x + 1) * 4 + z - 1] = temp;
                        //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                    }
                    //kill
                    else if (board.board[x - 1, z + 1] != null && (board.board[x - 1, z + 1].CompareTag("Player A") || board.board[x - 1, z + 1].CompareTag("King A")))
                    {
                        if (x - 2 >= 0 && z + 2 < BOARD_SIZE && board.board[x - 2, z + 2] == null)
                        {
                            Transform temp = (Transform)board.cubes[(x - 2) * BOARD_SIZE + z + 2];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x - 2) * BOARD_SIZE + z + 2);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                    }
                }
                if (curSelected.CompareTag("King B"))
                {
                    //left and down
                    if (x + 1 < BOARD_SIZE && z - 1 >= 0)
                    {
                        //regular move
                        if (board.board[x + 1, z - 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                        {
                            Transform temp = (Transform)board.cubes[(x + 1) * BOARD_SIZE + z - 1];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 1) * BOARD_SIZE + z - 1);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //board.cubes[(x + 1) * 4 + z - 1] = temp;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                        //kill
                        else if (board.board[x + 1, z - 1] != null && (board.board[x + 1, z - 1].CompareTag("Player A") || board.board[x + 1, z - 1].CompareTag("King A")))
                        {
                            if (x + 2 < BOARD_SIZE && z - 2 >= 0 && board.board[x + 2, z - 2] == null)
                            {
                                Transform temp = (Transform)board.cubes[(x + 2) * BOARD_SIZE + z - 2];
                                Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 2) * BOARD_SIZE + z - 2);
                                queue.Enqueue(opt);
                                temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                                //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                            }
                        }
                    }
                    //right and down
                    if (x + 1 < BOARD_SIZE && z + 1 < BOARD_SIZE)
                    {
                        //regular move
                        if (board.board[x + 1, z + 1] == null && (mode == Show.NORMAL || mode == Show.KING))
                        {
                            Transform temp = (Transform)board.cubes[(x + 1) * BOARD_SIZE + z + 1];
                            Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 1) * BOARD_SIZE + z + 1);
                            queue.Enqueue(opt);
                            temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                            //board.cubes[(x + 1) * 4 + z - 1] = temp;
                            //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                        }
                        //kill
                        else if (board.board[x + 1, z + 1] != null && (board.board[x + 1, z + 1].CompareTag("Player A") || board.board[x + 1, z + 1].CompareTag("King A")))
                        {
                            if (x + 2 < BOARD_SIZE && z + 2 < BOARD_SIZE && board.board[x + 2, z + 2] == null)
                            {
                                Transform temp = (Transform)board.cubes[(x + 2) * BOARD_SIZE + z + 2];
                                Option opt = new Option(temp.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color"), (x + 2) * BOARD_SIZE + z + 2);
                                queue.Enqueue(opt);
                                temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                                //setTransparent(temp.gameObject.GetComponent<MeshRenderer>(), true);
                            }
                        }
                    }
                }
            }
        }
    }
    void restoreBoard()
    {
        while (queue.Count != 0)
        {
            Option opt = (Option)queue.Dequeue();
            Transform temp = (Transform)board.cubes[opt.getIndex()];
            temp.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", opt.getColor());
            board.cubes[opt.getIndex()] = temp;
        }
    }
    void checkWinner(){
        int playerA = 0;
        int playerB = 0;
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (board.board[i, j] != null)
                {
                    if (board.board[i, j].CompareTag("Player A") || board.board[i, j].CompareTag("King A"))
                    {
                        playerA++;
                    }
                    else if (board.board[i, j].CompareTag("Player B") || board.board[i, j].CompareTag("King B"))
                    {
                        playerB++;
                    }
                }
            }
        }
        if (playerA == 0)
        {
            board.displayText("PLAYER B WON");
            state = State.END_OF_GAME;
        }
        if (playerB == 0)
        {
            board.displayText("PLAYER A WON");
            state = State.END_OF_GAME;
        }
    }
    void rotate()
    {
        /*rotating = true;
        DateTime startTime = DateTime.Now;
        float travelTime = 2; //2 seconds
        float degree = 180 / (travelTime * 10000000); //degree/tick
        long elapsedTicks = 0;
        while (elapsedTicks < travelTime * 10000000)
        {
            camera.transform.RotateAround(point, Vector3.up, degree);
            elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
        }
        rotating = false;
        /*Vector3 position = camera.transform.position;
        position.x--;
        position.z--;
        camera.transform.position = position;*/
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
    public override bool Equals(System.Object other)
    {
        if (other == null)
        {
            return true;
        }
        Option that = other as Option;
        return (index == that.getIndex());
    }
    public override string ToString()
    {
        return "Cube at "+index.ToString();
    }
}
