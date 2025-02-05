using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using System;

using Random = UnityEngine.Random;

[System.Serializable]
public class Room
{
    public string name;
    [Header("Number of rooms able to spawn (inclusive)")]
    public int min;
    public int max = 1;
    public bool isIncluded; 
    public GameObject[] roomVariance;
}


public static class RoomUtility {
    public enum LayoutType{
        Path = 0,
        Room = 1,
    }
    public enum RoomType {
        MainRoom = 0,
        SideRoom = 1
    }
    public static IEnumerable<Vector3> adjacentDirection(){
        yield return Vector3.up;
        yield return Vector3.right;
        yield return Vector3.down;
        yield return Vector3.left;
    }
    
    /// <summary>
    /// scanning empty spaces adjacent to the current tile position
    /// </summary>
    public static void getAdjacentVec3(ref Vector3[] emptySpace, Vector3 currentPos, Vector2[] _roomPos, float tileSize = 1.0f){
        // ArrayUtility.contains method is still bugged
        foreach (Vector3 vec3 in adjacentDirection()){
            if ((ArrayUtility.FindIndex(_roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 
                ){
                ArrayUtility.Add(ref emptySpace, vec3);
            }
        }
    }
    
    /// <summary>
    /// Shuffles an array of Vector3
    /// </summary>
    public static void Vec3Shuffle(ref Vector3[] arr){
        int rand;
        Vector3 temp;

        for (int i = 0 ; i < arr.Length ; i++){
            rand = Random.Range(0, arr.Length);
            temp = arr[rand];
            arr[rand] = arr[i];
            arr[i] = temp;
        }
    }
    public static void Vec3Reduce(ref Vector3[] arr, bool needExtra = true){
        if (!needExtra){
            arr = new Vector3[]{arr[0]};
            // Debug.Log("returned single vec3");
            return;
        }

        Vector3[] temp = arr;
        foreach(Vector3 vec3 in temp){
            if(arr.Length == 1){return;}
            else if (Random.Range(0, 3) == 0){
                ArrayUtility.Remove(ref arr, vec3);
            }
        }
    }
    public static T PopArray<T>(ref T[] arr, int index){
        if (index == -1){
            index = Random.Range(0, arr.Length);
        }
        T temp = arr[index];
        ArrayUtility.RemoveAt(ref arr, index);
        return temp;
    }
}

public static class RoomDebug {

    public static void ShowRoomWorldPositions(Vector2[] roomWorldCoordinates, bool verbose = true){
        Vector2[] tempArray = new Vector2[0];
        foreach(Vector2 vec2 in roomWorldCoordinates){
            if (verbose){
                Debug.Log("Vector 2 world position at : " + vec2);
            }
            if (ArrayUtility.FindIndex(tempArray, x => x == vec2) == -1){
                ArrayUtility.Add(ref tempArray, vec2);
            }
            else {
                Debug.Log("Rooms overlapping at " + vec2);
                ArrayUtility.Clear(ref tempArray);
                return;
            }
        }
        Debug.Log("Room Generated Successfully");
        if(verbose){Debug.Log("No overlapping occured");}
    }

    public static void StoreRoom(ref string[] arr, string roomName){
        ArrayUtility.Add(ref arr, roomName);
        return;
    }
    
    public static void ShowAllRooms(string[] arr, bool verbose = true){
        
        string[] tempArr = new string[0];

        foreach(string roomName in arr){
            string _temp = roomName;
            if(ArrayUtility.IndexOf(tempArr, _temp) == -1){
                ArrayUtility.Add(ref tempArr, _temp);
            }
        }

        if(verbose){
            foreach(string roomName in tempArr){
                Debug.Log(string.Format("{1} of {0} instantiated.", roomName, Count(arr, roomName)));
            }
        }
        
        Debug.Log(string.Format("In total, this map has {0} of rooms", arr.Length));
    }
    public static int Count(string[] strArr, string str){
        int i = 0;
        foreach(string s in strArr){
            if(s == str){
                i++;
            }
        }
        return i;
    }
}
