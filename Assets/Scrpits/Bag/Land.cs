using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public CropData cropData;

    public GameObject seedling;
    public GameObject halfGrown;
    public GameObject mature;

    private int stage = 0; // 0 = Seedling, 1 = HalfGrown, 2 = Mature
    private float time = 0f;
    public float growTime = 120f; // Time to fully grow in seconds
    public float speed = 1f; // Controls the speed of growth

    public float fert = 100f; // Soil fertility
    public float organic = 50f; // Organic matter in the soil
    public float condition = 75f; // General condition of the soil

    void Start()
    {
        // Initialize with seedling
        seedling.SetActive(true);
        halfGrown.SetActive(false);
        mature.SetActive(false);
    }

    void Update()
    {
        // Increment the growth time based on the time scale
        time += Time.deltaTime * speed;

        // Check if it's time to grow to the next stage
        if (stage == 0 && time >= growTime / 3)
        {
            NextStage();
        }
        else if (stage == 1 && time >= 2 * growTime / 3)
        {
            NextStage();
        }
        else if (stage == 2 && time >= growTime)
        {
            NextStage();
        }
    }

    // Function to handle growing to the next stage
    void NextStage()
    {
        stage++;
        switch (stage)
        {
            case 1:
                // Transition from seedling to half grown
                seedling.SetActive(false);
                halfGrown.SetActive(true);
                UpdateSoil();
                break;
            case 2:
                // Transition from half grown to mature
                halfGrown.SetActive(false);
                mature.SetActive(true);
                UpdateSoil();
                break;
            case 3:
                // Crop is fully grown
                Debug.Log("Fully grown!");
                UpdateSoil();
                break;
        }
    }

    // Function to set the time scale for growth
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Function to affect soil attributes based on crop growth
    void UpdateSoil()
    {
        // Example values for potato crop
        fert -= 1f; // Absorb 1 unit of fertility
        organic += 2f; // Release 2 units of organic matter
        condition += 1f; // Improve soil condition by 1 unit

        // Print new soil attributes for debugging
        Debug.Log($"Fertility: {fert}, Organic: {organic}, Condition: {condition}");
    }
}
