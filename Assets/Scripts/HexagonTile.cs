using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : ConstantClass
{
    GridManager GridManagerObject;
    public int x;
    public int y;
    public Color colour;
    public Vector2 linearInterpolationCoordinate;
    public bool linearInterpolation;
    public Vector2 velocity;
    public bool bomb; //unused?
    private int bombTimer;
    private TextMesh text;

    public struct NeighbouringHexagons
    {
        public Vector2 up;
        public Vector2 down;
        public Vector2 upLeft;
        public Vector2 downLeft;
        public Vector2 upRight;
        public Vector2 downRight;
    }

    void Start()
    {
        GridManagerObject = GridManager.instance;
        linearInterpolation = false;
    }


    void Update()
    {
        if (linearInterpolation)
        {
            float newX = Mathf.Lerp(transform.position.x, linearInterpolationCoordinate.x, Time.deltaTime * _constantHexagonRotate);
            float newY = Mathf.Lerp(transform.position.y, linearInterpolationCoordinate.y, Time.deltaTime * _constantHexagonRotate);

            transform.position = new Vector2(newX, newY);

            if (Vector3.Distance(transform.position, linearInterpolationCoordinate) < _hexagonRotationLimit)
            {
                transform.position = linearInterpolationCoordinate;
                linearInterpolation = false;
            }
        }
    }

    //burada kaldık devamı yarın yoruldum ya la
}
