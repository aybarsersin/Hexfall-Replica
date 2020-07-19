using UnityEngine;

public class HexagonTile : ConstantClass
{
    GridManager GridManagerObject;
    public int coordinateX;
    public int coordinateY;
    public Color colour;
    public Vector2 linearInterpolationCoordinate;
    public bool linearInterpolation;
    public Vector2 velocity;
    private bool bombHexagon;
    private int bombHexagonTimer;
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

    public void SettingHexagonX(int value)
    {
        coordinateX = value;
    } 

    public void SettingHexagonY(int value)
    {
        coordinateY = value;
    }

    public void SettingHexagonColour(Color newColour)
    {
        GetComponent<SpriteRenderer>().color = newColour;
        colour = newColour;
    }

    public void BombHexagonTick()
    {
        --bombHexagonTimer;
        text.text = bombHexagonTimer.ToString();
    }

    public int GettingHexagonX()
    {
        return coordinateX;
    }

    public int GettingHexagonY()
    {
        return coordinateY;
    }

    public int GettingBombhexagonTimer()
    {
        return bombHexagonTimer;
    }

    public Color GettingHexagonColour()
    {
        return GetComponent<SpriteRenderer>().color;
    }

    public void Rotate(int newX, int newY, Vector2 newCoordinates)
    {
        linearInterpolationCoordinate = newCoordinates;
        SettingHexagonX(newX);
        SettingHexagonY(newY);
        linearInterpolation = true;
    }

    public bool IsHexagonRotating()
    {
        return linearInterpolation;
    }

    public bool IsHexagonMoving()
    {
        return !(GetComponent<Rigidbody2D>().velocity == Vector2.zero);
    }

    public void IsBombHexagonScored()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public NeighbouringHexagons GetNeighbouringHexagons()
    {
        NeighbouringHexagons neighbouringHexagons;
        bool onColumn = GridManagerObject.OnColumn(GettingHexagonX());

        neighbouringHexagons.up = new Vector2(coordinateX, coordinateY + 1);
        neighbouringHexagons.down = new Vector2(coordinateX, coordinateY - 1);
        neighbouringHexagons.upLeft = new Vector2(coordinateX - 1, onColumn ? coordinateY + 1 : coordinateY);
        neighbouringHexagons.upRight = new Vector2(coordinateX + 1, onColumn ? coordinateY + 1 : coordinateY);
        neighbouringHexagons.downLeft = new Vector2(coordinateX - 1, onColumn ? coordinateY : coordinateY - 1);
        neighbouringHexagons.downRight = new Vector2(coordinateX + 1, onColumn ? coordinateY : coordinateY - 1);

        return neighbouringHexagons;
    }

    public void AlterWorldPosition(Vector2 newCoordinate)
    {
        linearInterpolationCoordinate = newCoordinate;
        linearInterpolation = true;
    }

    public void AlterHexagonGridPosition(Vector2 newCoordinate)
    {
        coordinateX = (int)newCoordinate.x;
        coordinateY = (int)newCoordinate.y;
    }

    public void SetBombHexagon()
    {
        text = new GameObject().AddComponent<TextMesh>();
        text.alignment = TextAlignment.Center;
        text.anchor = TextAnchor.MiddleCenter;
        text.transform.position = new Vector3(transform.position.x, transform.position.y, -4);
        text.transform.localScale = transform.localScale;
        text.color = Color.black;
        text.transform.parent = transform;
        bombHexagonTimer = _constantBombTimer;
        text.text = bombHexagonTimer.ToString();
    }

}
