using System.Collections.Generic;
using UnityEngine;

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
        positions = new Vector3[IngrInLevel + 2];
        slices = new GameObject[IngrInLevel + 2];
        spacing = Slice.transform.localScale.x;
        IngrInLevel += 2;
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
                Debug.Log("Position Occupied! Choose another one!");
                return true;
            }
        }

        return false;
    }

    private Vector3 GeneratePosition(Vector3 startPosition)
    {
        List<Vector3> possibleDirections = new List<Vector3>(DIRECTIONS); 

        while (possibleDirections.Count > 0) 
        {
            Vector3 randomDir = possibleDirections[Random.Range(0, possibleDirections.Count)]; 
            Vector3 newPos = startPosition + randomDir * spacing; 

            if (!CheckForOccupiedPos(newPos)) 
            {
                return newPos;
            }

            Debug.Log($"Direction {randomDir} was already occupied!");
            possibleDirections.Remove(randomDir);
        }

        return startPosition; 
    }


    private void GenerateLevel()
    {
        positions = new Vector3[IngrInLevel + 2];

        positions[0] = Vector3.zero;
        if(slices[0] == null)
        {
            slices[0] = Instantiate(Slice, positions[0], Quaternion.identity);
        }
        else
        {
            slices[0].transform.position = positions[0];
        }

        positions[1] = positions[0] + DIRECTIONS[Random.Range(0, DIRECTIONS.Count)] * spacing;
        if(slices[1] == null)
        {
            slices[1] = Instantiate(Slice, positions[1], Quaternion.identity);
        }
        else
        {
            slices[1].transform.position = positions[1];
        }

        for (int i = 2; i < IngrInLevel; i++)
        {
            Vector3 referencePos = positions[Random.Range(0, i)];
            positions[i] = GeneratePosition(referencePos);
            if(slices[i] == null)
            {
                slices[i] = Instantiate(Slice, positions[i], Quaternion.identity);
            }
            else
            {
                slices[i].transform.position = positions[i];
            }
        }
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
