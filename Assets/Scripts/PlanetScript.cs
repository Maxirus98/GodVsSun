using UnityEngine;
using UnityEngine.UI;

public class PlanetScript : MonoBehaviour
{

    // Constants
    // TODO: To balance
    private readonly float MAX_HEAT_DANGER_DISTANCE = 20f;
    private readonly float MAX_COLD_DANGER_DISTANCE = 150f;

    private Color ICY_BLUE_COLOR = new(176, 224, 230);
    private Color NEUTRAL_COLOR = new Color(245, 245, 245);
    private Color HEAT_WARNING = new Color(255, 171, 64);
    private Color HEAT_DANGER = new Color(165, 1, 74);

    // Properties
    public float HeatRate { get { return this.heatRate; } set { this.heatRate = value; } }

    // Variables
    float maxHeatLevel = 100f;
    [SerializeField]
    float startPopulationCountInMillions = 8000f;
    float populationCountInMillions;

    [SerializeField]
    Slider heatSlider;
    [SerializeField]
    Image heatSliderFill;

    [SerializeField]
    Slider populationSlider;

    [SerializeField]
    Transform sun;

    float heatRate = 0.0f;
    int religionRate = 1;
    float heatLevel;
    

    private void Start()
    {
        // Heat
        heatSlider.minValue = 0.0f;
        heatLevel = 25f;
        heatSlider.value = heatLevel;
        heatSlider.maxValue = maxHeatLevel;

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


        // Population
        LosePopulation();
    }


    // TODO: FIX COLOR CHANGE ON SLIDER WITH VALUE. Not important
    public void UpdateColor()
    {
        if(heatLevel >= 75f)
        {
            heatSliderFill.color = HEAT_DANGER;

        } else if (heatLevel >= 40f)
        {
            heatSliderFill.color = HEAT_WARNING;
        } else if (heatLevel <= 10f)
        {
            heatSliderFill.color = ICY_BLUE_COLOR;
        } else
        {
            heatSliderFill.color = NEUTRAL_COLOR;
        }
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
}
