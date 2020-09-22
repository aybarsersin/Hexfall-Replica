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
	public GameObject settingsScreen;
	public GameObject colourSelectionParent;
	public GameObject gameOverScreen;
    public GameObject uniqueColourScreen;
	public List<Color> Colours;
	public bool check;
    public bool areColoursUnique;

	private GridManager GridManagerObject;
	private int colourCount;
	private int scoredHexagons;
	private int bombHexagonsCount;


    public GameObject hexagon1;
    public GameObject hexagon2;
    public GameObject hexagon3;
    public GameObject hexagon4;
    public GameObject hexagon5;
    public GameObject hexagon6;
    public GameObject hexagon7;

    Image img;
    Image img1;
    Image img2;
    Image img3;
    Image img4;
    Image img5;
    Image img6;
    Image img7;

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

    public void ChangeHexagonTileColour(GameObject gameObject)
    {
        List<Color> colours = new List<Color>();

        Color orange = new Color(1, 0.419516f, 0);

        colours.Add(Color.yellow);
        colours.Add(Color.red);
        colours.Add(Color.blue);
        colours.Add(Color.cyan);
        colours.Add(Color.green);
        colours.Add(Color.magenta);
        colours.Add(orange);

        img = gameObject.GetComponent<Image>();

        if (img.color == colours[0])
        {
            img.color = colours[1];
        }
        else if (img.color == colours[1])
        {
            img.color = colours[2];
        }
        else if (img.color == colours[2])
        {
            img.color = colours[3];
        }
        else if (img.color == colours[3])
        {
            img.color = colours[4];
        }
        else if (img.color == colours[4])
        {
            img.color = colours[5];
        }
        else if (img.color == colours[5])
        {
            img.color = colours[6];
        }
        else if (img.color == colours[6])
        {
            img.color = colours[0];
        }

    }

    public void Score(int n)
    {
		scoredHexagons += n;
		scoreText.text = (_constantScore * scoredHexagons).ToString();//alt satırda daha güzel bir logic bulunabilir.
        if (Int32.Parse(scoreText.text) >= _bombScoreLimit * bombHexagonsCount + _bombScoreLimit)
        {
			++bombHexagonsCount;
			GridManagerObject.SettingBombsAway(); // verim sorunu mevcut score kontrolü method çağırmadan burada yapılabilirdi eğer score uygun ise method çağrılabilirdi.
        }
    }

    public void Retry(string sceneName)
    {
        gameOverScreen.SetActive(false);
        settingsScreen.SetActive(true);
        SceneManager.LoadScene(sceneName);
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

    public void StartGame()
    {
        areColoursUnique = true;

        settingsScreen.SetActive(false);
        GridManagerObject.SettingGridWidth((int)(gridWidthSlider.value-_minGridWidth)*_double + _minGridWidth);
        GridManagerObject.SettingGridHeight((int)gridHeightSlider.value);

        img1 = hexagon1.GetComponent<Image>();
        img2 = hexagon2.GetComponent<Image>();
        img3 = hexagon3.GetComponent<Image>();
        img4 = hexagon4.GetComponent<Image>();
        img5 = hexagon5.GetComponent<Image>();
        img6 = hexagon6.GetComponent<Image>();
        img7 = hexagon7.GetComponent<Image>();

        List<Color> colours = new List<Color>();

        colours.Add(img1.color);
        colours.Add(img2.color);
        colours.Add(img3.color);
        colours.Add(img4.color);
        colours.Add(img5.color);
        colours.Add(img6.color);
        colours.Add(img7.color);

        for (int m = 0; m < colourCountSlider.value; m++)
        {
            for (int l = 0; l < colourCountSlider.value; l++)
            {
                if (colours[m] == colours[l] && m != l)
                {
                    areColoursUnique = false;
                }
            }
        }

        List<Color> coloursToAdd = new List<Color>();

        if (areColoursUnique)
        {
            for (int n = 0; n < colourCountSlider.value; n++)
            {
                coloursToAdd.Add(colours[n]);
            }

            GridManagerObject.SettingColourList(coloursToAdd);
            GridManagerObject.InitializationOfGameGrid(); 
        }
        else
        {
            uniqueColourScreen.SetActive(true);
        }
    }

    public void UniqueColourScreenOK()
    {
        uniqueColourScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    private void InitializationOfUI()
    {
        Default();

        for (int n = 0; n < colourSelectionParent.transform.childCount - colourCount; n++)
        {
            colourSelectionParent.transform.GetChild(colourSelectionParent.transform.childCount - n - 1).gameObject.SetActive(false);
        }
    }

    public void Default() 
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
