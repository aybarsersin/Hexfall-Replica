using UnityEngine;
using UnityEngine.UI;

public class UserInputManager : ConstantClass
{
    private bool validTouch;
    private GridManager GridManagerObject;
    private Vector2 touchStartCoordinates;
    private HexagonTile selectecHexagonTile;

    [SerializeField] Text scoreText1;
    [SerializeField] Text scoreText2;
    [SerializeField] Text scoreText3;
    [SerializeField] Text scoreText4;

    void Start()
    {
        GridManagerObject = GridManager.instance;
    }

    void Update()
    {
        if (GridManagerObject.ReadyForInput() && Input.touchCount > _zero)
        {

            Vector3 worldPositionOfTouch = Camera.main.ScreenToWorldPoint(Input.GetTouch(_zero).position);
            Vector2 touchPosition = new Vector2(worldPositionOfTouch.x, worldPositionOfTouch.y);
            Collider2D collider = Physics2D.OverlapPoint(touchPosition);
            selectecHexagonTile = GridManagerObject.GettingSelectedHexagonTile();

            DetectingTouch();
            IsHexagonTrioSelected(collider);
            DetectingRotation();
        }
    }

    private void DetectingTouch()
    {
        if (Input.GetTouch(_zero).phase == TouchPhase.Began)
        {
            validTouch = true;
            touchStartCoordinates = Input.GetTouch(_zero).position;
        }
    }

    private void IsHexagonTrioSelected(Collider2D collider)
    {
        if (collider != null && collider.transform.tag == _hexagon)
        {
            scoreText1.text = Input.GetTouch(_zero).phase.ToString();
            scoreText3.text = TouchPhase.Ended.ToString();
            if (Input.GetTouch(_zero).phase == TouchPhase.Ended && validTouch) //burda sıkıntı var
            {
                scoreText2.text = "girdi 2";
                validTouch = false;
                GridManagerObject.SelectHexagonTrio(collider);
            }
        }
    }

    private void DetectingRotation()
    {
        if (Input.GetTouch(_zero).phase == TouchPhase.Moved && validTouch)
        {
            Vector2 afterTouchCoordinates = Input.GetTouch(_zero).position;
            float distX = afterTouchCoordinates.x - touchStartCoordinates.x;
            float distY = afterTouchCoordinates.y - touchStartCoordinates.y;

            if ((Mathf.Abs(distX) > _hexagonRotateSlideDistance || Mathf.Abs(distY) > _hexagonRotateSlideDistance) && selectecHexagonTile != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(selectecHexagonTile.transform.position);

                bool horizontalSwipe = Mathf.Abs(distX) > Mathf.Abs(distY);
                bool swipeDirection = horizontalSwipe ? distX > _zero : distY > _zero;
                bool touchCoordinatesVsHexagonCoordinates = horizontalSwipe ? afterTouchCoordinates.y > screenPosition.y : afterTouchCoordinates.x > screenPosition.x;
                bool rotateHexagonTrioClockwise = horizontalSwipe ? swipeDirection == touchCoordinatesVsHexagonCoordinates : swipeDirection != touchCoordinatesVsHexagonCoordinates;

                validTouch = false;
                GridManagerObject.RotateSelectionOutline(rotateHexagonTrioClockwise);
            }
        }
    }
}
