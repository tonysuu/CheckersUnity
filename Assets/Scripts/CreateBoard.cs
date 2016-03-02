using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateBoard : MonoBehaviour {
    public ArrayList cubes = new ArrayList();
    public const int BOARD_SIZE = 6;
    const int NUM_PLAYERS = 2;
    private Transform temp;
    private Color WOOD = new Color(0.51f,0.32f,0);
    private Vector3 pieceScale = new Vector3(0.6f, 0.1f, 0.6f);
    public GameObject[,] board;
    public GameObject text;
    GameObject[] playerA;
    GameObject[] playerB;


    // Use this for initialization
    void Start () {
        board = new GameObject[BOARD_SIZE,BOARD_SIZE];
        createPieces();
        foreach (Transform child in transform)
        {
            cubes.Add(child);
            child.gameObject.tag = "Cube";
        }
        for (int i = 0; i < BOARD_SIZE*BOARD_SIZE; i++)
        {
            temp = (Transform)cubes[i];
            if (((int)temp.position.x + (int)temp.position.z) % 2 == 0)
            {
                temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
            }
            else
            {
                temp.gameObject.GetComponent<MeshRenderer>().material.color = WOOD;
            }
        }
        text = GameObject.Find("WinText");
        text.transform.position = new Vector3(2, 1, -0.5f);
        text.transform.Rotate(new Vector3(25, 270, 0));
        text.GetComponent<TextMesh>().characterSize = 0.1f;
        text.GetComponent<TextMesh>().fontSize = 100;
        text.SetActive(false);
	}

    void Update()
    {
        //playerA[0].transform.position = Vector3.MoveTowards(playerA[0].transform.position, new Vector3(5, 0.3f, 5), Time.deltaTime * 2);
    }

    void createPieces()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                board[i, j] = null;
            }
        }


        playerA = new GameObject[NUM_PLAYERS];
        playerB = new GameObject[NUM_PLAYERS];
        Vector3 positionA = new Vector3(0, 0.3f, 1);
        Vector3 positionB = new Vector3(5, 0.3f, 2);


        for (int i = 0; i < NUM_PLAYERS; i++)
        {
            playerA[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            playerB[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            playerA[i].transform.position = positionA;
            playerA[i].transform.localScale = pieceScale;
            playerA[i].GetComponent<MeshRenderer>().material.color = Color.red;
            playerA[i].tag = "Player A";

            playerB[i].transform.position = positionB;
            playerB[i].transform.localScale = pieceScale;
            playerB[i].GetComponent<MeshRenderer>().material.color = Color.blue;
            playerB[i].tag = "Player B";

            board[(int)positionA.x, (int)positionA.z] = playerA[i];
            board[(int)positionB.x, (int)positionB.z] = playerB[i];
            positionA.z += 2;
            positionB.z += 2;
        }
    }
    public class SortByIndex : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            Transform a = x as Transform;
            Transform b = y as Transform;
            float indexA = a.position.x * BOARD_SIZE + a.position.z;
            float indexB = b.position.x * BOARD_SIZE + b.position.z;
            if (indexA > indexB)
                return 1;
            else
                return 0;
        }
    }
    public void printCube(ArrayList cube)
    {
        for (int i = 0; i < cube.Count; i++)
        {
            print(cube[i]);
        }
    }
    public void displayText(string toDisplay)
    {
        text.SetActive(true);
        text.GetComponent<TextMesh>().text = toDisplay;
        if (EditorUtility.DisplayDialog("GAME OVER", "Rematch?", "YES", "NO"))
        {
            reset();
        }
        else
        {
            Application.Quit();
        }
    }
    void reset()
    {
        print("game reset");
    }
}