using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private static int ingrInLevel = 3; 
    public GameObject Slice;
    public List<Material> allMaterials;
    private static float sliceScale = 0;
    private static Vector3 occupiedDirection;

    private Vector3 RandomDirection()
    {
        Vector3[] directions = { Vector3.forward, -Vector3.forward, Vector3.right, Vector3.left};
        return directions[Random.Range(0, directions.Length)];
    }

    private void ChangeSlicePosition(GameObject obj, int j)
    {
        if(slicesInLevel[j] != null)
        {
            //Ensure no overlapping between slices excluding the last occupied position
            Vector3 RandomDir = RandomDirection();
            
            while(RandomDir == occupiedDirection)
            {
                RandomDir = RandomDirection();
            }

            obj.transform.position = slicesInLevel[j].transform.position + RandomDir * sliceScale;
            occupiedDirection = -RandomDir;
        }
    }
    
    private void GenerateBreadSlices()
    {
        for(int i = 0; i < 2; i++)
        {
            GameObject bread = Instantiate(Slice, Vector3.zero, Quaternion.identity);
            bread.name = "Bread";
            bread.GetComponent<MeshRenderer>().material = allMaterials[0];
            if(i > 0)
            {
                ChangeSlicePosition(bread, i-1);
            }
            slicesInLevel[i] = bread;
        }
    }


    private void GenerateLevel()
    {
        GenerateBreadSlices();
        
        for(int i = 0; i < ingrInLevel; i++)
        {
            GameObject ingr = Instantiate(Slice, Vector3.zero, Quaternion.identity);
            ingr.name = "Ingredient";
            int randomIndex = Random.Range(0, 1 + i);
            ChangeSlicePosition(ingr, randomIndex);
            slicesInLevel[i + 2] = ingr;
        }
    }

    private static GameObject[] slicesInLevel;
    private void ClearLevel()
    {
        for (int i = 0; i < slicesInLevel.Length; i++)
        {
            if (slicesInLevel[i] != null)
            {
                Destroy(slicesInLevel[i]);
            }
        }
        
        slicesInLevel = new GameObject[ingrInLevel + 2];   
        occupiedDirection = new Vector3();
    }

    void Awake()
    {
        slicesInLevel = new GameObject[ingrInLevel + 2];   
        occupiedDirection = new Vector3();
    }

    public bool GenerateLev;
    public bool ClearLev;
    private void Update()
    {
        if(GenerateLev)
        {
            sliceScale = Slice.transform.localScale.x;
            GenerateLevel();
            GenerateLev = false;
        }

        if(ClearLev)
        {
            ClearLevel();
            ClearLev = false;
        }
    }
}