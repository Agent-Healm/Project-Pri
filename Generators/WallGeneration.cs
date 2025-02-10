using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    public bool gateEast = false;
    public bool gateSouth = false;
    public bool gateWest = false;
    public bool gateNorth = false;
    
    private int _gateWidth;
    private GameObject _wallTile;
    private GameObject _gateTile;
    private GameObject _nullTile;
    private float _halfLength;
    private float _halfWidth;
    private int _length;
    private int _width;
    private RoomUtility.LayoutType _layoutType;
    private Vector2 _center;
    private FloorGeneration _floorGen;
    private SpriteRenderer _srWall ;
    private SpriteRenderer _srGate ;
    void Start()
    {

        _floorGen = this.GetComponent<FloorGeneration>();
        _length = _floorGen._length;
        _width = _floorGen._width;
        _layoutType = _floorGen.layoutType;
        
        _wallTile = TextureTheme.instance.wallTile;
        _srWall = _wallTile.GetComponent<SpriteRenderer>();

        _gateTile = TextureTheme.instance.gateTile;
        _srGate = _gateTile.GetComponent<SpriteRenderer>();

        _nullTile = TextureTheme.instance.nullTile;
        _center = transform.position;
        _gateWidth = RoomConfig.instance.path.roomVariance[0]
                    .GetComponent<FloorGeneration>()
                    .width; 

        _halfLength = (_length + 1.0f) / 2;
        _halfWidth = (_width + 1.0f) / 2;

        if (_layoutType == RoomUtility.LayoutType.Room){

            _GenerateCorners();
            _GenerateGate(Vector2.right, gateEast);
            _GenerateGate(Vector2.left, gateWest);
            _GenerateGate(Vector2.down, gateSouth);
            _GenerateGate(Vector2.up, gateNorth);
        }
        
        else if (_layoutType == RoomUtility.LayoutType.Path){
            // if both direction is not a gate, generate wall
            if(!gateEast && !gateWest){
                _GenerateGate(Vector2.right, gateEast);
                _GenerateGate(Vector2.left, gateWest);
            }
            if(!gateSouth && !gateNorth){
                _GenerateGate(Vector2.down, gateSouth);
                _GenerateGate(Vector2.up, gateNorth);
            }
        }
    }
    private void _GenerateCorners(){
        SpriteRenderer sr = _wallTile.GetComponent<SpriteRenderer>();
        if(sr.drawMode == SpriteDrawMode.Tiled){
            sr.size = new Vector2(1, 1);
        }   
        _GenerateWall(_wallTile, - _halfLength, _halfWidth); // Top Left Corner
        _GenerateWall(_wallTile, _halfLength, _halfWidth); // Top Right Corner
        _GenerateWall(_wallTile, _halfLength, - _halfWidth); // Bottom Right Corner                        
        _GenerateWall(_wallTile, - _halfLength, - _halfWidth); // Bottom Left Corner
    }
    private void _GenerateWall(GameObject tile, float xPos, float yPos){
        Instantiate(tile, _center + new Vector2(xPos, yPos), 
                    Quaternion.identity, transform);
    }
    
    private void _GenerateGate(Vector2 vec2, bool hasGate = false){

        int _thisGateWidth = _gateWidth;

        if (vec2.y == 0.0f){
            // HORIZONTAL

            float __posX = 0.0f;
            if(vec2 == Vector2.right){   
                __posX = _halfLength;
            }
            else if(vec2 == Vector2.left){
                __posX = - _halfLength;
            }
            // for (int i = 1 ; i <= _width ; i++){
            //     _GenerateWall(__tile, __posX, i - _halfWidth);
            // }

            if ((_width + _thisGateWidth) % 2 != 0){
                _thisGateWidth += 1;
            }

            if (hasGate){
                _srWall.size = new Vector2(1, (_width - _thisGateWidth) / 2);
                _GenerateWall(_wallTile, __posX,   (_width + _thisGateWidth) / 4f);
                _GenerateWall(_wallTile, __posX, - (_width + _thisGateWidth) / 4f);
                _srGate.size = new Vector2(1, _thisGateWidth);
                _GenerateWall(_gateTile, __posX, 0);
            }
            else {
                _srWall.size = new Vector2(1, _width);
                _GenerateWall(_wallTile, __posX, 0);
            }
        }
        else if (vec2.x == 0.0f){
            // VERTICAL

            float __posY = 0.0f;
            if(vec2 == Vector2.up){
                __posY = _halfWidth;
            }
            else if (vec2 == Vector2.down){
                __posY = - _halfWidth;
            }
            // for (int i = 1 ; i <= _length ; i++){
            //     _GenerateWall(__tile, i - _halfLength, __posY);
            // }

            if ((_length + _gateWidth) % 2 != 0){
                _thisGateWidth += 1;
            }

            if (hasGate){
                _srWall.size = new Vector2((_length - _thisGateWidth) / 2, 1);
                _GenerateWall(_wallTile,   (_length + _thisGateWidth) / 4f, __posY);
                _GenerateWall(_wallTile, - (_length + _thisGateWidth) / 4f, __posY);
                _srGate.size = new Vector2(_thisGateWidth, 1);
                _GenerateWall(_gateTile, 0, __posY);
            }
            else {
                _srWall.size = new Vector2(_length, 1);
                _GenerateWall(_wallTile, 0, __posY);
            }
        }
    }
    public void GateReset(){
        gateEast = false;
        gateSouth = false;
        gateWest = false;
        gateNorth = false;
    }
    public void setGate(Vector2 vec2, bool isGate){
        if (vec2 == Vector2.up){gateNorth = isGate; return;}
        if (vec2 == Vector2.right){gateEast = isGate; return;}
        if (vec2 == Vector2.down){gateSouth = isGate; return;}
        if (vec2 == Vector2.left){gateWest = isGate; return;}
    }

}
