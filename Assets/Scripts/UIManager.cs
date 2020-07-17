using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : ConstantClass
{
	public Text scoreText;
	public Text gridWidthText;
	public Text gridHeightext;
	public Text colourCountText;
	public Slider gridWidthSlider;
	public Slider gridHeightSlider;
	public Slider colourCountSlider;
	public Dropdown colourblindDropdown;
	public GameObject settingsScreen;
	public GameObject colourSelectionParent;
	public GameObject gameOverScreen;
	public List<Color> Colours;
	public bool check;

	private GridManager GridManagerObject;
	private int colourCount;
	private int scoredHexagons;
	private int bombHexagonsCount;

	public static UIManager instance;

    void Awake()
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
		bombHexagonsCount = _zero;
		GridManagerObject = GridManager.instance;
		scoredHexagons = _zero;
		colourCount = 7;
        InitializationOfUI();
    }

    void Update()
    {
        if (check)
        {
			StartGame();
			check = false;
        }
    }

    public void Score(int n)
    {
		scoredHexagons += n;
		scoreText.text = (_constantScore * scoredHexagons).ToString();//alt satırda daha güzel bir logic bulunabilir.
        if (Int32.Parse(scoreText.text) >= _bombScoreLimit * bombHexagonsCount + _bombScoreLimit)
        {
			++bombHexagonsCount;
			GridManagerObject.SettingBombsAway();
        }
    }

    public void GameOver()
    {
		gameOverScreen.SetActive(true);
    }

    public void ReturnButton(string sceneName)
    {
		SceneManager.LoadScene(sceneName);
    }

    public void GridWidthSlider()
    {
		gridWidthText.text = ((gridWidthSlider.value - _minGridWidth) * _double + _minGridWidth).ToString();
    }

    public void GridHeightSlider()
    {
		gridHeightext.text = gridHeightSlider.value.ToString();
    }

    public void ColourCountSlider()
    {
		int childrenCount = colourSelectionParent.transform.childCount;
		int newChildrenCount = (int)colourCountSlider.value;
		colourCountText.text = newChildrenCount.ToString();

        if (newChildrenCount > colourCount)
        {
            for (int n = 0; n < childrenCount; n++)
            {
                if (!colourSelectionParent.transform.GetChild(n).gameObject.activeSelf)
                {
					colourSelectionParent.transform.GetChild(n).gameObject.SetActive(true);
					break;
                }
            }
        }
        else if (newChildrenCount < colourCount)
        {
            for (int n = 0; n < childrenCount; n++)
            {
                if (n+1 >= childrenCount)
                {
                    colourSelectionParent.transform.GetChild(n).gameObject.SetActive(false);
                    break;
                }
                else if (!colourSelectionParent.transform.GetChild(n+1).gameObject.activeSelf)
                {
                    colourSelectionParent.transform.GetChild(n).gameObject.SetActive(false);
                    break;
                }
            }
        }
        colourCount = newChildrenCount;
    }

    public void StartGame() //renk seçtirme burada yapılacak.
    {
        settingsScreen.SetActive(false);
        GridManagerObject.SettingGridWidth((int)(gridWidthSlider.value-_minGridWidth)*_double + _minGridWidth);
        GridManagerObject.SettingGridHeight((int)gridHeightSlider.value);
        GridManagerObject.SettingColourBlindMode(colourblindDropdown.value != _zero); // buna da bi bak derim.

        List<Color> colours = new List<Color>();

        colours.Add(Color.blue);
        colours.Add(Color.red);
        colours.Add(Color.yellow);
        colours.Add(Color.green);
        colours.Add(Color.cyan);

        GridManagerObject.SettingColourList(colours);
        GridManagerObject.InitializationOfGameGrid();
    }

    private void InitializationOfUI()
    {
        Default();

        for (int n = 0; n < colourSelectionParent.transform.childCount - colourCount; n++)
        {
            colourSelectionParent.transform.GetChild(colourSelectionParent.transform.childCount - n - 1).gameObject.SetActive(false);
        }
    }

    private void Default()
    {
        gridHeightSlider.value = _factoryGridHeight;
        gridWidthSlider.value = _factoryGridWidth;
        colourCountSlider.value = _factoryColourCount;
        colourCount = _factoryColourCount;
        gridWidthText.text = ((gridWidthSlider.value - _minGridWidth) * _double + _minGridWidth).ToString();
        gridHeightext.text = gridHeightSlider.value.ToString();
        scoreText.text = scoredHexagons.ToString();
    }
}
