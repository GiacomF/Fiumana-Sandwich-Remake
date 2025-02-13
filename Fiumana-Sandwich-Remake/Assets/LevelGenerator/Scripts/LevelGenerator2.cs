using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class LevelGenerator2 : MonoBehaviour
{
    /*

    Il metodo di generazione è questo: quando un punto viene generato, esclusi il primo ed il secondo, viene creata una lista di posizioni disponibili; 
    se ne estrae una a sorte e si genera il punto; se il punto corrisponde ad un'altro, si esclude la direzione selezionata dalla lista e se ne tira
    a sorte una nuova. Così fino a quando il punto non viene generato. Se la lista si esaurisce, allora viene selezionato un nuovo punto.

    metodo SelectRandomDirection()
    {
        apriamo un array[] di vettori3 e li definiamo = { su, -su, destra, -destra };
        return directions[Randm.Range(0, directions.Length)];
    }

    metodo SpostaIlPunto()
    {
        seleziona una direzione randomica in cui piazzare il punto = SelectRandomDirection
    }

    array[] di vettori3 grandi come il numero di ingredienti + 2;
    Gameobject Slice;
    float Larghezza di Slice = slice.transform.localScale.x; Forse questo va messo in un metodo inizializzatore e poi in Start;
    metodo GeneraUnaPosizione()
    {
        ciclo for (i = 0; i < ingredients + 2; i++)
        {
            Istanzia un Vector3 in posizione 0 0 0;
            Se il ciclo è > 0
            {
                prendi il punto e spostalo in posizione adiacente al primo a distanza 
            }
        }
    }

    */

    private static List<Vector3> DIRECTIONS;
    Vector3[] positions;
    GameObject[] slices;
    [SerializeField] int IngrInLevel;
    public GameObject Slice;
    float spacing;
    private void Init()
    {
        DIRECTIONS = new List<Vector3>{ Vector3.forward, -Vector3.forward, Vector3.right, -Vector3.right };
        positions = new Vector3[IngrInLevel];
        slices = new GameObject[IngrInLevel];
        spacing = Slice.transform.localScale.x;
    }

    private void Start()
    {
        Init();   
    }

    private bool CheckForOccupiedPos(Vector3 posToCheck)
    {
        for(int i = 0; i < IngrInLevel; i++)
        {
            if(posToCheck == positions[i])
            {
                return true;
            }
        }

        return false;
    }

    private Vector3 GeneratePosition()
    {
        List<Vector3> possibleDirections = new List<Vector3>(DIRECTIONS);
        Vector3 startPosition = positions[Random.Range(0, positions.Length)]; 

        while (possibleDirections.Count > 0) 
        {
            Vector3 randomDir = possibleDirections[Random.Range(0, possibleDirections.Count)]; 
            Vector3 newPos = startPosition + randomDir * spacing; 

            if (!CheckForOccupiedPos(newPos)) 
            {
                return newPos;
            }

            possibleDirections.Remove(randomDir);
        }

        return GeneratePosition(); 
    }

    public Material[] materials;
    private void GenerateLevel()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        positions = new Vector3[IngrInLevel];

        if (slices == null || slices.Length != IngrInLevel)
        {
            foreach (var slice in slices)
            {
                if (slice != null) Destroy(slice);
            }

            slices = new GameObject[IngrInLevel];
        }

        for (int i = 0; i < IngrInLevel; i++)
        {
            bool isBread = i < 2;
            string sliceName = isBread ? "Bread" : "Ingredient";

            GameObject newSlice = slices[i] ?? Instantiate(Slice);
            newSlice.name = sliceName;

            if(i < 1)
            {
                positions[i] = Vector3.zero;
            }
            else if(i > 0 && i < 2)
            {
                positions[i] = positions[0] + DIRECTIONS[Random.Range(0, DIRECTIONS.Count)] * spacing;
            }
            else
            {
                positions[i] = GeneratePosition();
            }

            newSlice.transform.position = positions[i];

            if(isBread)
            {
                newSlice.GetComponent<MeshRenderer>().material = materials[0];
            }

            slices[i] = newSlice;
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log($"Execution time is: {stopwatch.ElapsedTicks} ticks");
    }

    public bool GenLev;
    private void Update()
    {
        if(GenLev)
        {
            GenerateLevel();
            GenLev = false;
        }   
    }
}