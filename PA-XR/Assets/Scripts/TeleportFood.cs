using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFood : MonoBehaviour
{
    public GameObject[] foodToSpawn;
    public int[] cloanCounter;
    public bool foodOnPlate;
   


    public void SpawnFoodSource (int index) {
        if(index >= 0 && index <= foodToSpawn.Length -1 ) {
            for(int i = 0; i < foodToSpawn.Length; i++) {
                if(index == i && cloanCounter[i] < 1 && !foodOnPlate) {
                        var foodCopy = Instantiate(foodToSpawn[i], new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
                        foodCopy.tag = "copy";
                        cloanCounter[i]++;
                        foodOnPlate = true;
                      }
            }
        }
    }


}
