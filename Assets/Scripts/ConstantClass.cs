using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantClass : MonoBehaviour
{
    protected const int _zero = 0;
    protected const int _double = 2;
    protected const int _half = 2;
    protected const int _minGridWidth = 5;
    protected const int _factoryGridWidth = 5;
    protected const int _factoryGridHeight = 8;
    protected const int _factoryColourCount = 5;
    protected const int _selectionStatusCount = 6;
    protected const int _hexagonRotateSlideDistance = 5;
    protected const int _constantHexagonRotate = 9;
    protected const int _constantScore = 5;
    protected const int _seed = 76142;
    protected const int _constantBombTimer = 6;
    protected const int _verticalGridOffset = -3;
    protected const int _bombScoreLimit = 1000;

    protected const float _verticalHexagonDistance = 0.445f;
    protected const float _horizontalHexagonDistance = 0.23f;
    protected const float _hexagonRotationLimit = 0.05f;
    protected const float _hexagonSendDelay = 0.025f;

    protected const string _hexagon = "Hexagon";

    protected readonly Vector3 _hexagonOutlineDimensions = new Vector3(0.685f, 0.685f, 0.685f);
    protected readonly Vector2 _hexagonInitialPosition = new Vector3(0, 5.5f, 0);
}
