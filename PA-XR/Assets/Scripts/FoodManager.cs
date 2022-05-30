using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public GameObject[] foodToSpawn;

    public Food existingFood;

    public void SpawnFoodSource (int index) 
    {
        DestroyFood();

        var foodCopy = Instantiate(foodToSpawn[index], new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
        existingFood = foodCopy.AddComponent<Food>();
    }

    public void DestroyFood() 
    {
        if (existingFood != null)
        {
            Destroy(existingFood.gameObject);
        }
    }

}
