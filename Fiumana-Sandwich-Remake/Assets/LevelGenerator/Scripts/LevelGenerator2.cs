using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator2 : MonoBehaviour
{
    [SerializeField] private static int ingrInLevel = 3; 
    public GameObject Slice;
    public List<Material> allMaterials;
    private static float sliceScale = 0;
    private static Vector3 occupiedDirection;

    private struct Level
    {
        public Vector3[] positions;
        public Level(int i)
        {
            positions = new Vector3[i + 2];
        }
    }

    private Vector3 RandomDirection()
    {
        Vector3[] directions = { Vector3.forward, -Vector3.forward, Vector3.right, Vector3.left};
        return directions[Random.Range(0, directions.Length)];
    }

    private Vector3 ChangeSlicePosition(int j)
    {
        //Ensure no overlapping between slices excluding the last occupied position
        Vector3 RandomDir = RandomDirection();
            
        while(RandomDir == occupiedDirection)
        {
            RandomDir = RandomDirection();
        }

        occupiedDirection = -RandomDir;
        return currLevel.positions[j] += RandomDir * sliceScale;
    }

    private bool PosOverlaps(Vector3 curPoint)
    {
        bool state = false;
        for(int i = 0; i < currLevel.positions.Length; i++)
        {
            if(currLevel.positions[i] == curPoint)
            {
                state = true;
            }
            else
            {
                state = false;
            }
        }
        return state;
    }

    private static Level currLevel;
    private void GenerateLevel()
    {
        currLevel = new Level(ingrInLevel);

        for (int i = 0; i < currLevel.positions.Length; i++)
        {
            currLevel.positions[i] = new Vector3(0, 0, 0);
            if(i > 0)
            {
                currLevel.positions[i] = ChangeSlicePosition(i-1);
            }
            else if (i > 1)
            {
                int randomIndex = Random.Range(0, ingrInLevel);

                currLevel.positions[i] = currLevel.positions[randomIndex];

                while(PosOverlaps(currLevel.positions[i]))
                {
                    currLevel.positions[i] = ChangeSlicePosition(i);
                }
            }
        }
    }

    private float GetSliceScale()
    {
        return Slice.transform.localScale.x;
    }

    void Awake()
    {   
        occupiedDirection = new Vector3();
    }

    public bool GenerateLev;
    private void Update()
    {
        if(GenerateLev)
        {
            GetSliceScale();
            GenerateLevel();
        }
    }
}
