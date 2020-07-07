using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum Wall{
        Null, Top, Right, Bottom, Left
    }

    Enum.Dir _direction;
    [SerializeField] List<Enum.Dir> _directions = new List<Enum.Dir>();
    [SerializeField] GameObject[] _objects;
    [SerializeField] GameObject _wallTop;
    [SerializeField] GameObject _wallRight;
    [SerializeField] GameObject _wallBottom;
    [SerializeField] GameObject _wallLeft;
    [SerializeField] GameObject _spawn;
    [SerializeField] GameObject _end;
    [SerializeField] GameObject _intelPref;
    [SerializeField] GameObject _plankPref;
    [SerializeField] int _plankLevel = 1;
    [SerializeField] GameObject[] _intelSpawns;
    [SerializeField] GameObject[] _sawSpawns;
    [SerializeField] GameObject _sawPref;
    [SerializeField] int _sawLevel = 3;
    [SerializeField] GameObject[] _enemySpawns;
    [SerializeField] GameObject _enemyPref;
    [SerializeField] int _enemyLevel = 2;
    [SerializeField] GameObject[] _stomperSpawns;
    [SerializeField] GameObject _stompPref;
    [SerializeField] int _stomperLevel = 4;
    [SerializeField] Transform _spawner;

    List<Wall> _openings = new List<Wall>();

    public Enum.Dir Direction{
        get{
            return _direction;
        }
    }

    void Start(){
        StartCoroutine(Renderer());
    }
    
    public void Setup(Vector2 index, Room[][] map, Enum.Dir entrance, Enum.Path path){
        Wall[] openings = CreateOpenings(index, map, entrance, path);
        foreach(Wall opening in openings){
            _openings.Add(opening);
            switch(opening){
                case Wall.Top:
                    // _wallTop.SetActive(false);
                    Destroy(_wallTop);
                    break;
                case Wall.Right:
                    // _wallRight.SetActive(false);
                    Destroy(_wallRight);
                    break;
                case Wall.Bottom:
                    // _wallBottom.SetActive(false);
                    Destroy(_wallBottom);
                    break;
                case Wall.Left:
                    // _wallLeft.SetActive(false);
                    Destroy(_wallLeft);
                    break;
                default:
                    break;
            }
        }
    }

    Wall[] CreateOpenings(Vector2 index, Room[][] map, Enum.Dir entrance, Enum.Path path){
        //Declare paths
        Wall[] paths = new Wall[2];

        //Removes entrance from random choices
        List<Enum.Dir> tempsDirList = new List<Enum.Dir>();
        for(int i = _directions.Count-1; i >= 0; i--){
            tempsDirList.Add(_directions[i]);
        }
        for(int i = tempsDirList.Count-1; i >= 0; i--){
            if(tempsDirList[i] != entrance){
                switch(tempsDirList[i]){
                    case Enum.Dir.North:
                    tempsDirList.Add(Enum.Dir.South);
                    break;
                    case Enum.Dir.South:
                    tempsDirList.Add(Enum.Dir.North);
                    break;
                    case Enum.Dir.Start:
                    case Enum.Dir.East:
                    tempsDirList.Add(Enum.Dir.West);
                    break;
                    case Enum.Dir.West:
                    tempsDirList.Add(Enum.Dir.East);
                    break;
                }
                tempsDirList.Remove(tempsDirList[i]);
            }
        }
        _directions = tempsDirList;
        //Create random path
        if(path == Enum.Path.Path){
            int randPath = Random.Range(0, _directions.Count-1);
            _direction = _directions[randPath];
            // _spawn.SetActive(false);
            Destroy(_spawn);
            Destroy(_end);
            int randObstacles = Random.Range(0, 100);
            int highDiff = (GameManager.instance.Level >= 7) ? 5 : 0;
            if(randObstacles <= 80){
                CreateIntels();
            }
            if(randObstacles <= (GameManager.instance.Level == _plankLevel ? 20 : 10) + highDiff * 1 && GameManager.instance.Level >= _plankLevel){//10%
                CreatePlankWall(entrance);
            }
            if(randObstacles <= (GameManager.instance.Level == _enemyLevel ? 25 : 15) + highDiff * 1 && GameManager.instance.Level >= _enemyLevel){//15%
                CreateEnemies();
            }
            else if(randObstacles <= 30 + highDiff * 2 && GameManager.instance.Level >= _sawLevel){//15%
                CreateSaws(entrance);
            }
            else if(entrance == _direction && randObstacles <= (GameManager.instance.Level == _stomperLevel ? 65 : 45) + highDiff * 3 && GameManager.instance.Level >= _stomperLevel){//15% when to walls face each other
                CreateStompers(entrance);
            }
        }
        //Create starting path
        else if(path == Enum.Path.Start) {
            _direction = Enum.Dir.Start;
            Destroy(_end);
            _spawn.SetActive(true);
        }
        //Close Path
        else if(path == Enum.Path.End) {
            _direction = Enum.Dir.End;
            Destroy(_spawn);
            _end.SetActive(true);
        }

        //Open entrance
        switch(entrance){
            case Enum.Dir.North:
                paths[0] = Wall.Bottom;
                break;
            case Enum.Dir.Start:
            case Enum.Dir.East:
                paths[0] = Wall.Left;
                break;
            case Enum.Dir.South:
                paths[0] = Wall.Top;
                break;
            case Enum.Dir.West:
                paths[0] = Wall.Right;
                break;
            case Enum.Dir.Null:
            case Enum.Dir.End:
            default:
                paths[0] = Wall.Null;
                break;
        }

        //Open exit
        switch(_direction){
            case Enum.Dir.North:
                paths[1] = Wall.Top;
                break;
            case Enum.Dir.Start:
            case Enum.Dir.East:
                paths[1] = Wall.Right;
                break;
            case Enum.Dir.South:
                paths[1] = Wall.Bottom;
                break;
            case Enum.Dir.West:
                paths[1] = Wall.Left;
                break;
            case Enum.Dir.Null:
            case Enum.Dir.End:
            default:
                paths[1] = Wall.Null;
                break;
        }
        return paths;
    }

    void CreatePlankWall(Enum.Dir entrance){
        List<int> sides = new List<int>(){0,1,2,3};
        List<int> available = new List<int>();
        for(int i = sides.Count-1; i >= 0; i--){
            if(sides[i] == 0){
                if(entrance == Enum.Dir.South || _direction == Enum.Dir.North){
                    available.Add(sides[i]);
                    // Debug.Log(0);
                }
            }
            else if(sides[i] == 1){
                if(entrance == Enum.Dir.West || _direction == Enum.Dir.East){
                    available.Add(sides[i]);
                    // Debug.Log(1);
                }
            }
            else if(sides[i] == 2){
                if(entrance == Enum.Dir.North || _direction == Enum.Dir.South){
                    available.Add(sides[i]);
                    // Debug.Log(2);
                }
            }
            else if(sides[i] == 3){
                if(entrance == Enum.Dir.East || _direction == Enum.Dir.West){
                    available.Add(sides[i]);
                    // Debug.Log(3);
                }
            }
        }
        GameObject plank = Instantiate(_plankPref, _sawSpawns[available[0]].transform.position, Quaternion.identity);
        plank.transform.SetParent(_spawner);
        plank.GetComponent<PlankWall>().SetAngle = (available[0] % 2 == 1) ? 90 : 0;
    }

    void CreateStompers(Enum.Dir entrance){
        List<int> sides = new List<int>(){0,1,2,3};
        for(int i = sides.Count-1; i >= 0; i--){
            if(sides[i] == 0){
                if(entrance == Enum.Dir.South || _direction == Enum.Dir.North){
                    sides.Remove(i);
                    // Debug.Log(0);
                }
            }
            else if(sides[i] == 1){
                if(entrance == Enum.Dir.West || _direction == Enum.Dir.East){
                    sides.Remove(i);
                    // Debug.Log(1);
                }
            }
            else if(sides[i] == 2){
                if(entrance == Enum.Dir.North || _direction == Enum.Dir.South){
                    sides.Remove(i);
                    // Debug.Log(2);
                }
            }
            else if(sides[i] == 3){
                if(entrance == Enum.Dir.East || _direction == Enum.Dir.West){
                    sides.Remove(i);
                    // Debug.Log(3);
                }
            }
        }

        GameObject stomper = Instantiate(_stompPref, _enemySpawns[0].transform.position, Quaternion.identity);
        stomper.transform.SetParent(_spawner);
        stomper.GetComponent<Stomper>().SetAngle = (entrance == Enum.Dir.West || entrance == Enum.Dir.East) ? 90 : 0;
    }
    void CreateSaws(Enum.Dir entrance){
        //Prevent saw from spawning in mid air, blocking the path
        List<int> sides = new List<int>(){0,1,2,3};
        for(int i = sides.Count-1; i >= 0; i--){
            if(sides[i] == 0){
                if(entrance == Enum.Dir.South || _direction == Enum.Dir.North){
                    sides.Remove(i);
                    // Debug.Log(0);
                }
            }
            else if(sides[i] == 1){
                if(entrance == Enum.Dir.West || _direction == Enum.Dir.East){
                    sides.Remove(i);
                    // Debug.Log(1);
                }
            }
            else if(sides[i] == 2){
                if(entrance == Enum.Dir.North || _direction == Enum.Dir.South){
                    sides.Remove(i);
                    // Debug.Log(2);
                }
            }
            else if(sides[i] == 3){
                if(entrance == Enum.Dir.East || _direction == Enum.Dir.West){
                    sides.Remove(i);
                    // Debug.Log(3);
                }
            }
        }
        int randSpawn = Random.Range(0, sides.Count-1);
        GameObject saw = Instantiate(_sawPref, _sawSpawns[sides[randSpawn]].transform.position, Quaternion.identity);
        saw.transform.SetParent(_spawner);
    }
    void CreateEnemies(){
        GameObject enemy = Instantiate(_enemyPref, _enemySpawns[0].transform.position, Quaternion.identity);
        enemy.transform.SetParent(_spawner);
    }

    void CreateIntels(){
        int randSpawn = Random.Range(0, _intelSpawns.Length-1);
        GameObject intel = Instantiate(_intelPref, _intelSpawns[randSpawn].transform.position, Quaternion.identity);
        intel.transform.SetParent(_spawner);
    }


    IEnumerator Renderer(){
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bool isActive = false;
        while(true){
            if(Vector2.Distance(this.transform.position, player.position) < 28){
                if(!isActive){
                    isActive = true;
                    Activate(isActive);
                }
            } else {
                if(isActive){
                    isActive = false;
                    Activate(isActive); 
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    void Activate(bool isActive = true){
        foreach(GameObject obj in _objects){
            if(obj){
                obj.SetActive(isActive);
            }
        }
    }
}
