// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LayoutGeneration : MonoBehaviour
// {
//     private int _length;
//     private int _width;
//     private Vector2 _center;
//     private FloorGeneration _floorGen;

//     // Start is called before the first frame update
//     void Start()
//     {

//         Vector2 pos;

//         _floorGen = this.GetComponent<FloorGeneration>();
//         _length = _floorGen._length;
//         _width = _floorGen._width;
//         _center = transform.position;

//         // offset for even sized room
//         float halfLength = (_length - 1) / 2.0f;
//         float halfWidth = (_width - 1) / 2.0f;

//         for (int w = 0 ; w < _width ; w++){
//             for (int l = 0 ; l < _length ; l++){
//                 if (Mathf.Abs(w - halfWidth) <= 1){continue;}
//                 if (Mathf.Abs(l - halfLength) <= 1){continue;}
//                 // if((w + l) % 2 !=1){continue;}
//                 pos = new Vector2(l - halfLength, halfWidth - w);
//                 Instantiate(TextureTheme.instance.wallTile, _center + pos, 
//                                 Quaternion.identity, transform);
                
//             }
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
