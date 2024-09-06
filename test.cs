using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class test : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(getRandomInt(5));
        // foreach (int i in testMethod.getRandomInt(4)){
        //     Debug.Log(i);
        // }
        // for (int i = 0 ; i < 4 ; i++){
        //     Debug.Log(getRandomInt(4));
        // }
        // string[] strings = new string[0];
        // testMethod.populateList(ref strings);

        // foreach(string s in strings){
        //     Debug.Log(s);
        // }

        // int[] listInt = new int[]{
        //     1,2,3,4
        // };

        // listInt.Shuffle();
        // foreach(int i in listInt){
        //     Debug.Log(i);
        // }

        // Debug.Log("Num : " + testMethod.num());
        // Debug.Log("Num : " + testMethod.num(5));
        
    }
}

public static class testMethod
{

    public static IEnumerable<int> getRandomInt(int max){
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

    public static void populateList(ref string[] st){
        ArrayUtility.Add(ref st, "ok");
        ArrayUtility.Add(ref st, "and");
        ArrayUtility.Add(ref st, "hi");
        Debug.Log("done");
    }

    public static void Shuffle(this int[] arr){
        int rand;
        int temp;

        for (int i = 0 ; i < arr.Length ; i++){
            rand = Random.Range(0, arr.Length);
            temp = arr[rand];
            arr[rand] = arr[i];
            arr[i] = temp;
        }
    }

    public static int num(int x = 1){
        return x;
    }
}
