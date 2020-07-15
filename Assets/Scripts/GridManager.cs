using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : ConstantClass
{
    public static GridManager instance = null;

    public GameObject hexagonPrefab;
    public GameObject hexagonParent;
    public GameObject outlineParent;
    public Sprite outlineSprite;
    public Sprite hexagonSprite;

    private int gridHeight;
    private int gridWidth;
    private int selectionStatus;
    private bool colourBlindMode;
    private bool bombsAway;
    private bool gameOver;

    private Vector2 selectedCoordinates;
    private HexagonTile selectedHexagonTile;
    private List<List<HexagonTile>> gameGrid;
    private List<HexagonTile> selectedTrio;
    private List<HexagonTile> bombHexagons;
    private List<Color> colourList;

    private bool hexagonRotateStatus;
    private bool bombHexagonExplodeStatus;
    private bool hexagonCreationStatus;
    private bool gameStartStatus;

    public void SettingGridWidth(int width)
    {
        gridWidth = width;
    }
    public void SettingGridHeight(int height)
    {
        gridHeight = height;
    }
    public void SettingColourBlindMode(bool mode)
    {
        colourBlindMode = mode;
    }
    public void SettingColourList(List<Color> colors)
    {
        colourList = colors;
    }
    public void SettingBombsAway()
    {
        bombsAway = true;
    }
    public int GettingGridWidth()
    {
        return gridWidth;
    }
    public int GettingGridHeight()
    {
        return gridHeight;
    }
    public bool GettingColourBlindMode()
    {
        return colourBlindMode;
    }
    public HexagonTile GettingSelectedHexagonTile()
    {
        return selectedHexagonTile;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        gameOver = false;
        bombsAway = false;
        hexagonRotateStatus = false;
        hexagonCreationStatus = false;
        bombHexagonExplodeStatus = false;
        bombHexagons = new List<HexagonTile>();
        selectedTrio = new List<HexagonTile>();
        gameGrid = new List<List<HexagonTile>>();
    }

    private IEnumerator CreateHexagons(List<int> columns, List<List<Color>> colourSeed = null)
    {
        Vector3 initialPosition;
        float coordinateX;
        float coordinateY;
        bool columnStatus;
        float startCoordinateX = CoordinateXOfFirstColumn();

        hexagonCreationStatus = true;

        foreach (int n in columns)
        {
            columnStatus = OnColumn(n);
            coordinateX = startCoordinateX + (_horizontalHexagonDistance * n);
            coordinateY = (_verticalHexagonDistance * gameGrid[n].Count * _double) + _verticalGridOffset + (columnStatus ? _verticalHexagonDistance : _zero);

            initialPosition = new Vector3(coordinateX, coordinateY, _zero);

            GameObject newGameObject = Instantiate(hexagonPrefab,_hexagonInitialPosition, Quaternion.identity, hexagonParent.transform);

            HexagonTile newHexagonTile = newGameObject.GetComponent<HexagonTile>();
            yield return new WaitForSeconds(_hexagonSendDelay);

            if (bombsAway)
            {
                newHexagonTile.SetBombHexagon();
                bombHexagons.Add(newHexagonTile);
                bombsAway = false;
            }

            if (colourSeed == null)
            {
                newHexagonTile.SettingHexagonColour(colourList[(int)(Random.value * _seed) % colourList.Count]);
            }
            else
            {
                newHexagonTile.SettingHexagonColour(colourSeed[n][gameGrid[n].Count]);
            }

            newHexagonTile.ChangeHexagonGridPosition(new Vector2(n, gameGrid[n].Count));
            newHexagonTile.AlterWorldPosition(initialPosition);
            gameGrid[n].Add(newHexagonTile);
        }

        hexagonCreationStatus = false;
    }

    public void InitializationOfGameGrid()
    {
        List<int> minusCells = new List<int>();

        for (int n = 0; n < GettingGridWidth(); n++)
        {
            for (int m = 0; m < GettingGridHeight(); m++)
            {
                gameGrid.Add(new List<HexagonTile>());
            }
        }

        StartCoroutine(CreateHexagons(minusCells,col))// burdasın balım
    }

    public bool OnColumn(int n)
    {
        int middleColumnIndex = GettingGridWidth() / _half;
        return (middleColumnIndex % 2 == n % 2);
    }

    public bool ReadyForInput() // burayı kontrol edicez input almazsa.
    {
        if (hexagonCreationStatus == false && hexagonRotateStatus == false && bombHexagonExplodeStatus == false && gameOver == false)
        {
            return true;
        }
        return false;
    }

    private float CoordinateXOfFirstColumn()
    {
        return gridWidth / _half * -_horizontalHexagonDistance;
    }

    private bool IsPositionGameGrid(Vector2 position)
    {
        if (position.x >= _zero && position.y >= _zero && position.x < GettingGridWidth() && position.y < GettingGridWidth())
        {
            return true;
        }

        return false;
    }

    private void PrintGameGrid() // gerekli mi değil mi ?
    {
        string map = "";


        for (int i = GettingGridHeight() - 1; i >= 0; --i)
        {
            for (int j = 0; j < GettingGridWidth(); ++j)
            {
                if (gameGrid[j][i] == null)
                    map += "0 - ";
                else
                    map += "1 - ";
            }

            map += "\n";
        }

        print(map);
    }
}
