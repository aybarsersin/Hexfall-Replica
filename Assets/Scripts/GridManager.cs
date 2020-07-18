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
    private bool hexagonScoringStatus;
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
        hexagonScoringStatus = false;
        bombHexagons = new List<HexagonTile>();
        selectedTrio = new List<HexagonTile>();
        gameGrid = new List<List<HexagonTile>>();
    }

    public void InitializationOfGameGrid()
    {
        List<int> minusCells = new List<int>();

        for (int n = 0; n < GettingGridWidth(); n++)
        {
            for (int m = 0; m < GettingGridHeight(); m++)
                minusCells.Add(n);

            gameGrid.Add(new List<HexagonTile>());
        }

        StartCoroutine(CreateHexagons(minusCells, CreateGridWithColour()));
    }

    public void SelectHexagonTrio(Collider2D collider)
    {
        if (selectedHexagonTile == null || !selectedHexagonTile.GetComponent<Collider2D>().Equals(collider))
        {
            selectedHexagonTile = collider.gameObject.GetComponent<HexagonTile>();
            selectedCoordinates.x = selectedHexagonTile.GettingHexagonX();
            selectedCoordinates.y = selectedHexagonTile.GettingHexagonY();
            selectionStatus = 0;
        }
        else
        {
            selectionStatus = (++selectionStatus) % _selectionStatusCount;
        }

        DestroyOutline();
        CreateOutline();
    }

    public void RotateSelectionOutline(bool CW)
    {
        DestroyOutline();
        StartCoroutine(RotationCoroutine(CW));
    }

    public bool OnColumn(int n)
    {
        int middleColumnIndex = GettingGridWidth() / _half;
        return (middleColumnIndex % 2 == n % 2);
    }

    public bool ReadyForInput() // burayı kontrol edicez input almazsa.
    {
        return !hexagonCreationStatus && !hexagonRotateStatus && !hexagonScoringStatus && !gameOver;
    }

    private float CoordinateXOfFirstColumn()
    {
        return gridWidth / _half * -_horizontalHexagonDistance;
    }

    private bool IsPositionGameGridValid(Vector2 position)
    {
        return position.x >= _zero && position.x < GettingGridWidth() && position.y >= _zero && position.y < GettingGridHeight();
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

            GameObject newGameObject = Instantiate(hexagonPrefab, _hexagonInitialPosition, Quaternion.identity, hexagonParent.transform);

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

            newHexagonTile.AlterHexagonGridPosition(new Vector2(n, gameGrid[n].Count));
            newHexagonTile.AlterWorldPosition(initialPosition);
            gameGrid[n].Add(newHexagonTile);
        }

        hexagonCreationStatus = false;
    }

    private List<List<Color>> CreateGridWithColour() //burayı da kontrol et!
    {
        List<List<Color>> colour = new List<List<Color>>();
        List<Color> checkColourList = new List<Color>();
        bool exit;

        for (int n = 0; n < GettingGridWidth(); n++)
        {
            colour.Add(new List<Color>());
            for (int m = 0; m < GettingGridHeight(); m++)
            {
                colour[n].Add(colourList[(int)(Random.value * _seed) % colourList.Count]);
                do
                {
                    exit = true;
                    colour[n][m] = colourList[(int)(Random.value * _seed) % colourList.Count];
                    if (n - 1 >= _zero && m - 1 >= _zero)
                    {
                        if (colour[n][m - 1] == colour[n][m] || colour[n - 1][m] == colour[n][m])
                        {
                            exit = false;
                        }
                    }
                } while (!exit);
            }
        }

        return colour;
    }

    private void FindHexagonTrio()
    {
        List<HexagonTile> value = new List<HexagonTile>();
        Vector2 firstPosition, secondPosition;

        selectedHexagonTile = gameGrid[(int)selectedCoordinates.x][(int)selectedCoordinates.y];

        FindNeighbouringHexagonsofSelectedHexagon(out firstPosition, out secondPosition);

        selectedTrio.Clear();
        selectedTrio.Add(selectedHexagonTile);
        selectedTrio.Add(gameGrid[(int)firstPosition.x][(int)firstPosition.y].GetComponent<HexagonTile>());
        selectedTrio.Add(gameGrid[(int)secondPosition.x][(int)secondPosition.y].GetComponent<HexagonTile>());
    }

    private void FindNeighbouringHexagonsofSelectedHexagon(out Vector2 first, out Vector2 second)
    {
        HexagonTile.NeighbouringHexagons neighbours = selectedHexagonTile.GetNeighbouringHexagons();
        bool breakTheLoop = false;

        do
        {
            switch (selectionStatus)
            {
                case 0: first = neighbours.up; second = neighbours.upRight; break;
                case 1: first = neighbours.upRight; second = neighbours.downRight; break;
                case 2: first = neighbours.downRight; second = neighbours.down; break;
                case 3: first = neighbours.down; second = neighbours.downLeft; break;
                case 4: first = neighbours.downLeft; second = neighbours.upLeft; break;
                case 5: first = neighbours.upLeft; second = neighbours.up; break;
                default: first = Vector2.zero; second = Vector2.zero; break;
            }

            if (first.x < _zero || first.x >= gridWidth || first.y < _zero || first.y >= gridHeight || second.x < _zero || second.x >= gridWidth || second.y < _zero || second.y >= gridHeight)
            {
                selectionStatus = (++selectionStatus) % _selectionStatusCount;
            }
            else
            {
                breakTheLoop = true;
            }

        } while (breakTheLoop);
    }

    private void DestroyOutline()
    {
        if (outlineParent.transform.childCount > _zero)
        {
            foreach (Transform child in outlineParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void CreateOutline()
    {
        FindHexagonTrio();

        foreach (HexagonTile outlinedHexagon in selectedTrio)
        {
            GameObject obj = outlinedHexagon.gameObject;
            GameObject outline = new GameObject("Outline");
            GameObject innerOutline = new GameObject("Inner Object");

            outline.transform.parent = outlineParent.transform;

            outline.AddComponent<SpriteRenderer>();
            outline.GetComponent<SpriteRenderer>().sprite = outlineSprite;
            outline.GetComponent<SpriteRenderer>().color = Color.white;
            outline.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -1);
            outline.transform.localScale = _hexagonOutlineDimensions;

            innerOutline.AddComponent<SpriteRenderer>();
            innerOutline.GetComponent<SpriteRenderer>().sprite = hexagonSprite;
            innerOutline.GetComponent<SpriteRenderer>().color = obj.GetComponent<SpriteRenderer>().color;
            innerOutline.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -2);
            innerOutline.transform.localScale = obj.transform.localScale;
            innerOutline.transform.parent = outline.transform;
        }
    }

    private void TurnHexagons(bool CW)
    {
        int x1, y1, x2, y2, x3, y3;
        Vector2 position1, position2, position3;
        HexagonTile first, second, third;

        first = selectedTrio[0];
        second = selectedTrio[1];
        third = selectedTrio[2];

        x1 = first.GettingHexagonX();
        y1 = first.GettingHexagonY();

        x2 = second.GettingHexagonX();
        y2 = second.GettingHexagonY();

        x3 = third.GettingHexagonX();
        y3 = third.GettingHexagonY();

        position1 = first.transform.position;
        position2 = second.transform.position;
        position3 = third.transform.position;

        if (CW)
        {
            first.Rotate(x2, y2, position2);
            gameGrid[x2][y2] = first;

            second.Rotate(x3, y3, position3);
            gameGrid[x3][y3] = second;

            third.Rotate(x1, y1, position1);
            gameGrid[x1][y1] = third;
        }
        else
        {
            first.Rotate(x3, y3, position3);
            gameGrid[x3][y3] = first;

            second.Rotate(x1, y1, position3);
            gameGrid[x1][y1] = second;

            third.Rotate(x2, y2, position2);
            gameGrid[x2][y2] = third;
        }
    }

    private List<HexagonTile> CheckScoringHexagons(List<List<HexagonTile>> checkList)
    {
        List<HexagonTile> neighbouringHexagonsList = new List<HexagonTile>();
        List<HexagonTile> scoringHexagonsList = new List<HexagonTile>();
        HexagonTile hexagon;
        HexagonTile.NeighbouringHexagons neighbouringHexagons;
        Color colour;

        for (int n = 0; n < checkList.Count; n++)
        {
            for (int m = 0; m < checkList[n].Count; m++)
            {
                hexagon = checkList[n][m];
                colour = hexagon.GettingHexagonColour();
                neighbouringHexagons = hexagon.GetNeighbouringHexagons();

                if (IsPositionGameGridValid(neighbouringHexagons.up))
                {
                    neighbouringHexagonsList.Add(gameGrid[(int)neighbouringHexagons.up.x][(int)neighbouringHexagons.up.y]);
                }
                else
                {
                    neighbouringHexagonsList.Add(null);
                }

                if (IsPositionGameGridValid(neighbouringHexagons.upRight))
                {
                    neighbouringHexagonsList.Add(gameGrid[(int)neighbouringHexagons.upRight.x][(int)neighbouringHexagons.upRight.y]);
                }
                else
                {
                    neighbouringHexagonsList.Add(null);
                }

                if (IsPositionGameGridValid(neighbouringHexagons.downRight))
                {
                    neighbouringHexagonsList.Add(gameGrid[(int)neighbouringHexagons.downRight.x][(int)neighbouringHexagons.downRight.y]);
                }
                else
                {
                    neighbouringHexagonsList.Add(null);
                }

                for (int l = 0; l < neighbouringHexagonsList.Count - 1; l++)
                {
                    if (neighbouringHexagonsList[l] != null && neighbouringHexagonsList[l + 1] != null)
                    {
                        if (neighbouringHexagonsList[l].GettingHexagonColour() == colour && neighbouringHexagonsList[l + 1].GettingHexagonColour() == colour)
                        {
                            if (!scoringHexagonsList.Contains(neighbouringHexagonsList[l]))
                            {
                                scoringHexagonsList.Add(neighbouringHexagonsList[l]);
                            }
                            if (!scoringHexagonsList.Contains(neighbouringHexagonsList[l + 1]))
                            {
                                scoringHexagonsList.Add(neighbouringHexagonsList[l + 1]);
                            }
                            if (!scoringHexagonsList.Contains(hexagon))
                            {
                                scoringHexagonsList.Add(hexagon);
                            }
                        }
                    }
                }

                neighbouringHexagonsList.Clear();
            }
        }
        return scoringHexagonsList;
    }

    private List<int> ScoreHexagons(List<HexagonTile> list)
    {
        List<int> minusColumns = new List<int>();

        float coordinateX, coordinateY;

        foreach (HexagonTile bombHexagon in bombHexagons)
        {
            if (!list.Contains(bombHexagon))
            {
                bombHexagon.BombHexagonTick(); // mesela bu yanlış bunu düzelteceğiz.
                if (bombHexagon.GettingBombhexagonTimer() == _zero)
                {
                    gameOver = true;
                    UIManager.instance.GameOver();
                    StopAllCoroutines();
                    return minusColumns;
                }
            }
        }

        foreach (HexagonTile hexagon in list)
        {
            if (bombHexagons.Contains(hexagon))
            {
                bombHexagons.Remove(hexagon);
            }
            UIManager.instance.Score(1);
            gameGrid[hexagon.GettingHexagonX()].Remove(hexagon);
            minusColumns.Add(hexagon.GettingHexagonX());
            Destroy(hexagon.gameObject);
        }

        foreach (int n in minusColumns)
        {
            for (int m = 0; m < gameGrid[n].Count; m++)
            {
                coordinateX = GettingGridStartCoordinateX() + (_horizontalHexagonDistance * n);
                coordinateY = (_verticalHexagonDistance * m * _double) + _verticalGridOffset + (OnColumn(n) ? _verticalHexagonDistance : _zero);
                gameGrid[n][m].SettingHexagonX(n);
                gameGrid[n][m].SettingHexagonY(m);
                gameGrid[n][m].AlterHexagonGridPosition(new Vector3(coordinateX, coordinateY, _zero));
            }
        }

        hexagonScoringStatus = false;
        return minusColumns;
    }

    private float GettingGridStartCoordinateX()
    {
        return gridWidth / _half * -_horizontalHexagonDistance;
    }

    private IEnumerator RotationCoroutine(bool CW)
    {
        List<HexagonTile> scoringHexagons = null;
        bool check = true;

        hexagonRotateStatus = true;
        for (int n = 0; n < selectedTrio.Count; n++)
        {
            TurnHexagons(CW);
            yield return new WaitForSeconds(0.3f);

            scoringHexagons = CheckScoringHexagons(gameGrid);
            if (scoringHexagons.Count > _zero)
            {
                break;
            }
        }

        hexagonScoringStatus = true;
        hexagonRotateStatus = false;

        while (scoringHexagons.Count > _zero)
        {
            if (check)
            {
                hexagonCreationStatus = true;
                StartCoroutine(CreateHexagons(ScoreHexagons(scoringHexagons)));
                check = false;
            }
            else if (!hexagonCreationStatus)
            {
                scoringHexagons = CheckScoringHexagons(gameGrid);
                check = true;
            }

            yield return new WaitForSeconds(0.3f);
        }

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
