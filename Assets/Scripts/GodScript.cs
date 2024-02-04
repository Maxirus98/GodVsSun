using UnityEngine;

public class GodScript : MonoBehaviour
{
    private GameObject selectedPlanet;
    float godPower;

    public bool planetMoveable = true;
    public bool religionCollectable = false;
    public bool coolDownActive = false;
    public bool warmUpActive = false;

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            if(planetMoveable)
                MoveSelectedPlanet();
            if(coolDownActive)
                CoolPlanet();

        }

        if(Input.GetMouseButton(1))
        {
            selectedPlanet = null;
        }
    }

    public void OnClick(int buttonId)
    {
        planetMoveable = buttonId == 0;
        religionCollectable = buttonId == 1;
        coolDownActive = buttonId == 2;
        warmUpActive = buttonId == 3;
    }

    private void CoolPlanet()
    {
        // todo: spawn a Cooler object
    }

    private void WarmPlanet()
    {
        // todo: spawn a Warmer object
    }

    private void MoveSelectedPlanet()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit && !hit.collider.CompareTag("Sun"))
        {
            selectedPlanet = hit.collider.gameObject;

            if (selectedPlanet != null)
            {
                hit.transform.position = mousePos;
            }
        }
    }
}
