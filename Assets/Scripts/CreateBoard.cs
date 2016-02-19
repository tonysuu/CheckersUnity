using UnityEngine;
using System.Collections;

public class CreateBoard : MonoBehaviour {
    private ArrayList cubes = new ArrayList();
    private const int BOARD_SIZE = 4;
    private Transform temp;
    private ArrayList materials = new ArrayList();
    private const int BLACK = 0;
    private const int WOOD = 1;
    private const int RED = 2;

	// Use this for initialization
	void Start () {
        loadMaterials();
        foreach (Transform child in transform)
        {
            cubes.Add(child);
        }
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                temp = (Transform)cubes[i * 4 + j];
                if ((i + j) % 2 == 0)
                {
                    temp.gameObject.GetComponent<MeshRenderer>().material = (Material)materials[BLACK];
                }
                else
                {
                    temp.gameObject.GetComponent<MeshRenderer>().material = (Material)materials[WOOD];
                }
            }
        }
	}
    private void loadMaterials()
    {
        materials.Add((Material)Resources.Load("black"));
        materials.Add((Material)Resources.Load("wood"));
        materials.Add((Material)Resources.Load("red"));
    }
}
