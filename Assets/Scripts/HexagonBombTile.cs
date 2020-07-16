using UnityEngine;

public class HexagonBombTile : HexagonTile
{
    public TextMesh output;
    private int timer;

    public void Tick()
    {
        --timer;
        output.text = timer.ToString();
    }

    public void SetTimer(int value)
    {
        timer = value;
        output.text = timer.ToString();
    }

    public int GettingTimer()
    {
        return timer;
    }
}
