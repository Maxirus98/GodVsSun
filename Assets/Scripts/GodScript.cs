using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GodScript : MonoBehaviour
{
    private GameObject selectedPlanet;

    // God Power
    [SerializeField]
    private Slider godPowerSlider;
    float godPower;
    float godPowerTimestamp = 0f;
    float godPowerRate = 1f;

    // Spell Cost
    float movingPlanetCostPerDistance = 0.25f;
    float warmUpPlanetCost = 25f;
    float coolDownPlanetCost = 25f;

    // Toggles
    public bool planetMoveable = true;
    public bool religionCollectable = false;
    public bool coolDownActive = false;
    public bool warmUpActive = false;

    private void Start()
    {
        godPower = 25f;
        godPowerSlider.value = godPower;
        godPowerSlider.minValue = 0f;
        godPowerSlider.maxValue = 100f;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            if(planetMoveable)
                MoveSelectedPlanet();
            if(coolDownActive)
                HelpPlanet("Cooler");
            if (warmUpActive)
                HelpPlanet("Heater");
            if (religionCollectable)
                CollectReligion();

        }

        if(Input.GetMouseButton(1))
        {
            selectedPlanet = null;
        }

        IncrementGodPower();
    }

    public void OnClick(int buttonId)
    {
        planetMoveable = buttonId == 0;
        religionCollectable = buttonId == 1;
        coolDownActive = buttonId == 2;
        warmUpActive = buttonId == 3;
    }

    private void HelpPlanet(string helperName)
    {
        var hit = GetSelectedPlanet();
        if (hit && !hit.collider.CompareTag("Sun"))
        {
            selectedPlanet = hit.collider.gameObject;

            if (selectedPlanet != null)
            {
                var cooldownGo = selectedPlanet.transform.Find(helperName);
                if (!cooldownGo.gameObject.activeInHierarchy)
                {
                    cooldownGo.gameObject.SetActive(true);
                    godPower -= coolDownPlanetCost;
                    godPowerSlider.value = godPower;
                } 
            }
        }
    }

    private void IncrementGodPower()
    {
        godPowerTimestamp += Time.deltaTime;

        if (godPowerTimestamp >= godPowerRate)
        {
            godPowerTimestamp = 0f;
            godPower++;
            godPowerSlider.value = godPower;
        }
    }

    private void CollectReligion()
    {
        RaycastHit2D hit = GetSelectedPlanet();
        if (hit && !hit.collider.CompareTag("Sun"))
        {
            selectedPlanet = hit.collider.gameObject;

            if (selectedPlanet != null)
            {
                var planetScript = selectedPlanet.GetComponent<PlanetScript>();
                godPower += planetScript.ReligionLevel;
                godPowerSlider.value = godPower;
                planetScript.ReligionLevel = 0;
            }
        }
    }

    private void MoveSelectedPlanet()
    {
        RaycastHit2D hit = GetSelectedPlanet();
        if (hit && !hit.collider.CompareTag("Sun"))
        {
            selectedPlanet = hit.collider.gameObject;

            if (selectedPlanet != null)
            {
                hit.transform.position = hit.point;
            }
        }
    }

    private RaycastHit2D GetSelectedPlanet()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        return Physics2D.Raycast(mousePos, Vector2.zero);
    }
}
