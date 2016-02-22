using UnityEngine;
using System.Collections;

public class CreateBoard : MonoBehaviour {
    public ArrayList cubes = new ArrayList();
    public const int BOARD_SIZE = 4;
    private Transform temp;
    private Color WOOD = new Color(0.51f,0.32f,0);
    private Vector3 pieceScale = new Vector3(0.6f, 0.1f, 0.6f);
    public GameObject[,] board;
    public int test = 5;

	// Use this for initialization
	void Start () {
        board = new GameObject[BOARD_SIZE,BOARD_SIZE];
        createPieces();
        foreach (Transform child in transform)
        {
            cubes.Add(child);
            child.gameObject.tag = "Cube";
        }
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                temp = (Transform)cubes[i * 4 + j];
                if ((i + j) % 2 == 0)
                {
                    temp.gameObject.GetComponent<MeshRenderer>().material.color = Color.black; //= (Material)materials[BLACK];
                }
                else
                {
                    temp.gameObject.GetComponent<MeshRenderer>().material.color = WOOD; //= (Material)materials[WOOD];
                }
            }
        }
	}

    void Update()
    {
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


        GameObject[] playerA = new GameObject[2];
        GameObject[] playerB = new GameObject[2];
        Vector3 positionA = new Vector3(0, 0.3f, 0);
        Vector3 positionB = new Vector3(3, 0.3f, 1);


        for (int i = 0; i < 2; i++)
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
}
