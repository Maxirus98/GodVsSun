using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetScript : MonoBehaviour
{
    // Constants
    // TODO: To balance
    private readonly float MAX_HEAT_DANGER_DISTANCE = 20f;
    private readonly float MAX_COLD_DANGER_DISTANCE = 150f;
    private readonly float MAX_HEAT_LEVEL = 100f;

    // Properties
    public float HeatRate { get { return this.heatRate; } set { this.heatRate = value; } }
    public int ReligionLevel { get { return this.religionLevel; } set { this.religionLevel = value; } }
    // Variables
    Transform sun;
    
    // Heat
    public Gradient heatGradient;
    Slider heatSlider;
    Image heatFill;
    float heatRate = 0.0f;
    float heatLevel;

    // Population
    Slider populationSlider;
    [SerializeField]
    float startPopulationCountInMillions = 8000f;
    float populationCountInMillions;

    // Religion
    TextMeshProUGUI religionLevelText;
    int religionLevel = 1;
    float religionTimestamp = 0f;
    float religionDelay = 1f;

    private void Awake()
    {
        // Components
        var planetUi = transform.GetChild(0);
        heatSlider = planetUi.Find("Heat").GetComponent<Slider>();
        heatFill = heatSlider.transform.Find("Fill").GetComponent<Image>();
        populationSlider = planetUi.Find("Population").GetComponent<Slider>();
        religionLevelText = planetUi.Find("ReligionLevelText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // Sun
        sun = GameObject.Find("Sun").transform;

        // Heat
        heatSlider.minValue = 0.0f;
        heatLevel = 25f;
        heatGradient.Evaluate(heatLevel / 100f);
        heatSlider.value = heatLevel;
        heatSlider.maxValue = MAX_HEAT_LEVEL;

        // Population
        populationCountInMillions = startPopulationCountInMillions;
        populationSlider.minValue = 0.0f;
        populationSlider.maxValue = startPopulationCountInMillions;
        populationSlider.value = populationCountInMillions;
    }

    private void Update()
    {
        // Heat
        var distanceFromTheSun = (transform.position - sun.position).sqrMagnitude;
        if(distanceFromTheSun <= MAX_HEAT_DANGER_DISTANCE)
        {
            heatRate += MAX_HEAT_DANGER_DISTANCE / distanceFromTheSun * Time.deltaTime;
        }
        else if(distanceFromTheSun >= MAX_COLD_DANGER_DISTANCE)
        {
            heatRate -= MAX_COLD_DANGER_DISTANCE / distanceFromTheSun * Time.deltaTime;
        } else
        {
            heatRate = 0.0f;
        }
        heatLevel += heatRate * Time.deltaTime;
        heatSlider.value = heatLevel;
        heatFill.color = heatGradient.Evaluate(heatSlider.normalizedValue);
        

        // Population
        LosePopulation();

        // Religion
        IncrementReligion();
    }

    private void LosePopulation()
    {
        var populationLostRate = heatLevel * Time.deltaTime;

        if (heatLevel >= 75f)
        {
            populationCountInMillions -= populationLostRate * 7.5f;
        }
        else if (heatLevel >= 40f)
        {
            populationCountInMillions -= populationLostRate * 4f;
        }
        else if (heatLevel <= 10f)
        {
            populationCountInMillions -= populationLostRate  * 10f;
        }

        populationSlider.value = populationCountInMillions;
    }

    private void IncrementReligion()
    {
        religionTimestamp += Time.deltaTime;

        if (religionTimestamp >= religionDelay)
        {
            religionTimestamp = 0f;
            religionLevel++;
            religionLevelText.text = $"{religionLevel}";
        }
    }
}
