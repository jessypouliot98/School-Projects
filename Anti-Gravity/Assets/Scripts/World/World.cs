using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    int _col = 100;
    int _row = 100;
    Room[][] _zones;
    [SerializeField] Room _room;
    [SerializeField] GameObject _playerPref;

    void Start(){
        Initialise();
    }

    void Initialise(){
        int mapWidth = Mathf.Max(10, GameManager.instance.Level * 3 + 10);
        int mapMaxWidth = 100;
        CreateMap(Mathf.Min(mapWidth, mapMaxWidth), 100);
    }

    void CreateMap(int cols, int rows){
        _zones = new Room[rows][];
        for(int r = 0; r < rows; r++){
            _zones[r] = new Room[cols];
        }
        CreateRooms(new Vector2((int)(rows/2), 0), Enum.Dir.Null, Enum.Path.Start);
    }

    //X = rows
    //Y = cols

    void CreateRooms(Vector2 aPos, Enum.Dir lastDir, Enum.Path path = Enum.Path.Path){
        //Create New Room
        Room room = Instantiate(_room, new Vector2(aPos.y * 8, aPos.x * 8), Quaternion.identity);
        room.Setup(aPos, _zones, lastDir, path);

        //Place New Room in X/Y pos
        Vector2 nextZoneIndex;
        switch(room.Direction){
            case Enum.Dir.Start:
            case Enum.Dir.End:
            case Enum.Dir.East:
                nextZoneIndex = new Vector2(aPos.x, aPos.y+1);
                break;
            case Enum.Dir.West:
                nextZoneIndex = new Vector2(aPos.x, aPos.y-1);
                break;
            case Enum.Dir.North:
                nextZoneIndex = new Vector2(aPos.x+1, aPos.y);
                break;
            case Enum.Dir.South:
                nextZoneIndex = new Vector2(aPos.x-1, aPos.y);
                break;
            default:
                nextZoneIndex = new Vector2(99999, 99999);
                break;
        }

        //Create an other room if not reached the end
        if(_zones.Length-1 > (int)nextZoneIndex.x && _zones[0].Length > (int)nextZoneIndex.y && (int)nextZoneIndex.y >= 0 && (int)nextZoneIndex.x >= 0){
            if(_zones[0].Length-1 != (int)nextZoneIndex.y && _zones.Length-1 != (int)nextZoneIndex.x && (int)nextZoneIndex.y >= 0 && (int)nextZoneIndex.x >= 0){
                CreateRooms(nextZoneIndex, room.Direction);
            } else {
                CreateRooms(nextZoneIndex, room.Direction, Enum.Path.End);
            }
        }
        else {
            Vector2 spawn = GameObject.Find("Spawn").transform.position;
            Instantiate(_playerPref, spawn, Quaternion.identity);
        }
    }
}

