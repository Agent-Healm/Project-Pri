using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    public GameObject[] objects;
    public int length;
    public int width;

    private Vector2 _center;
    // Start is called before the first frame update
    void Start()
    {

        _center = transform.position;
        // int rand = Random.Range(0, objects.Length);
        // Instantiate(objects[rand], _center + Vector2.up, Quaternion.identity, transform);

        // Instantiate(objects[0], _center + Vector2.up, Quaternion.identity, transform);
        int i;
        Vector2Int pos;
        int halfLength = length / 2;
        int halfWidth = width / 2;

        for (i = 0 ; i <= length ; i++){
            pos = new Vector2Int(i - halfLength, halfWidth + 1);
            Instantiate(objects[0], _center + pos, Quaternion.identity, transform);
        }
        for (i = 0 ; i <= width ; i++){
            pos = new Vector2Int(halfLength + 1, halfWidth - i);
            Instantiate(objects[0], _center + pos, Quaternion.identity, transform);
        }
        for (i = 0 ; i <= length ; i++){
            pos = new Vector2Int(halfLength - i, -1 - halfWidth);
            Instantiate(objects[0], _center + pos, Quaternion.identity, transform);
        }
        for (i = 0 ; i <= width ; i++){
            pos = new Vector2Int(-1 - halfLength, i - halfWidth);
            Instantiate(objects[0], _center + pos, Quaternion.identity, transform);
        }
        // i = 17;
        // Debug.Log(i / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
