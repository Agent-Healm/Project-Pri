using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(getRandomInt(5));
        foreach (int i in getRandomInt(4)){
            Debug.Log(i);
        }
        // for (int i = 0 ; i < 4 ; i++){
        //     Debug.Log(getRandomInt(4));
        // }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    private IEnumerable<int> getRandomInt(int max){
        int num;
        for (int i = 1 ; i <= max ; i++){
            num = Random.Range(0, max);
            yield return num;
        }

        // yield return 1;
        // yield return 2;
        // yield return 3;
        // yield return 4;
    }

}
