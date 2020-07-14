using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
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
    private 

    void Start()
    {
        
    }



    void Update()
    {
        
    }
}
