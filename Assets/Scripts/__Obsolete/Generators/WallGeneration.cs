// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WallGeneration : MonoBehaviour
// {
//     public bool gateEast = false;
//     public bool gateSouth = false;
//     public bool gateWest = false;
//     public bool gateNorth = false;
    
//     private int _gateWidth;
//     private GameObject _wallTile;
//     private GameObject _gateTile;
//     private GameObject _nullTile;
//     private float _halfLength;
//     private float _halfWidth;
//     private int _length;
//     private int _width;
//     private RoomUtility.LayoutType _layoutType;
//     private Vector2 _center;
//     private FloorGeneration _floorGen;
//     private SpriteRenderer _srWall ;
//     private SpriteRenderer _srGate ;
//     void Start()
//     {

//         _floorGen = this.GetComponent<FloorGeneration>();
//         _length = _floorGen._length;
//         _width = _floorGen._width;
//         _layoutType = _floorGen.layoutType;
        
//         _wallTile = TextureTheme.instance.wallTile;
//         _srWall = _wallTile.GetComponent<SpriteRenderer>();

//         _gateTile = TextureTheme.instance.gateTile;
//         _srGate = _gateTile.GetComponent<SpriteRenderer>();

//         _nullTile = TextureTheme.instance.nullTile;
//         _center = transform.position;
//         _gateWidth = RoomConfig.GetInstance.GetPath.GetRoomVariance[0]
//                     .GetComponent<FloorGeneration>()
//                     .width; 

//         _halfLength = (_length + 1.0f) / 2;
//         _halfWidth = (_width + 1.0f) / 2;

//         if (_layoutType == RoomUtility.LayoutType.Room){

//             GenerateCorners();
//             GenerateGate(Vector2.right, gateEast);
//             GenerateGate(Vector2.left, gateWest);
//             GenerateGate(Vector2.down, gateSouth);
//             GenerateGate(Vector2.up, gateNorth);
//         }
        
//         else if (_layoutType == RoomUtility.LayoutType.Path){
//             // if both direction is not a gate, generate wall
//             if(!gateEast && !gateWest){
//                 GenerateGate(Vector2.right, gateEast);
//                 GenerateGate(Vector2.left, gateWest);
//             }
//             if(!gateSouth && !gateNorth){
//                 GenerateGate(Vector2.down, gateSouth);
//                 GenerateGate(Vector2.up, gateNorth);
//             }
//         }
//     }
//     private void GenerateCorners(){
//         SpriteRenderer sr = _wallTile.GetComponent<SpriteRenderer>();
//         if(sr.drawMode == SpriteDrawMode.Tiled){
//             sr.size = new Vector2(1, 1);
//         }   
//         GenerateWall(_wallTile, - _halfLength, _halfWidth); // Top Left Corner
//         GenerateWall(_wallTile, _halfLength, _halfWidth); // Top Right Corner
//         GenerateWall(_wallTile, _halfLength, - _halfWidth); // Bottom Right Corner                        
//         GenerateWall(_wallTile, - _halfLength, - _halfWidth); // Bottom Left Corner
//     }
//     private void GenerateWall(GameObject l_tile, float l_xPos, float l_yPos){
//         Instantiate(l_tile, _center + new Vector2(l_xPos, l_yPos), 
//                     Quaternion.identity, transform);
//     }
//     private void GenerateGate(Vector2 l_vec2, bool l_hasGate = false){

//         int _thisGateWidth = _gateWidth;

//         if (l_vec2.y == 0.0f){
//             // HORIZONTAL

//             float l__posX = 0.0f;
//             if(l_vec2 == Vector2.right){   
//                 l__posX = _halfLength;
//             }
//             else if(l_vec2 == Vector2.left){
//                 l__posX = - _halfLength;
//             }

//             if ((_width + _thisGateWidth) % 2 != 0){
//                 _thisGateWidth += 1;
//             }

//             if (l_hasGate){
//                 _srWall.size = new Vector2(1, (_width - _thisGateWidth) / 2);
//                 if (_srWall.size.y != 0){
//                     GenerateWall(_wallTile, l__posX,   (_width + _thisGateWidth) / 4f);
//                     GenerateWall(_wallTile, l__posX, - (_width + _thisGateWidth) / 4f);
//                 }
//                 _srGate.size = new Vector2(1, _thisGateWidth);
//                 GenerateWall(_gateTile, l__posX, 0);
//             }
//             else {
//                 _srWall.size = new Vector2(1, _width);
//                 GenerateWall(_wallTile, l__posX, 0);
//             }
//         }
//         else if (l_vec2.x == 0.0f){
//             // VERTICAL

//             float l__posY = 0.0f;
//             if(l_vec2 == Vector2.up){
//                 l__posY = _halfWidth;
//             }
//             else if (l_vec2 == Vector2.down){
//                 l__posY = - _halfWidth;
//             }

//             if ((_length + _gateWidth) % 2 != 0){
//                 _thisGateWidth += 1;
//             }

//             if (l_hasGate){
//                 _srWall.size = new Vector2((_length - _thisGateWidth) / 2, 1);
//                 if (_srWall.size.x != 0){
//                     GenerateWall(_wallTile,   (_length + _thisGateWidth) / 4f, l__posY);
//                     GenerateWall(_wallTile, - (_length + _thisGateWidth) / 4f, l__posY);
//                 }
//                 _srGate.size = new Vector2(_thisGateWidth, 1);
//                 GenerateWall(_gateTile, 0, l__posY);
//             }
//             else {
//                 _srWall.size = new Vector2(_length, 1);
//                 GenerateWall(_wallTile, 0, l__posY);
//             }
//         }
//     }
//     public void GateReset(){
//         gateEast = false;
//         gateSouth = false;
//         gateWest = false;
//         gateNorth = false;
//     }
//     public void setGate(Vector2 l_vec2, bool l_isGate){
//         if (l_vec2 == Vector2.up){gateNorth = l_isGate; return;}
//         if (l_vec2 == Vector2.right){gateEast = l_isGate; return;}
//         if (l_vec2 == Vector2.down){gateSouth = l_isGate; return;}
//         if (l_vec2 == Vector2.left){gateWest = l_isGate; return;}
//     }

// }
