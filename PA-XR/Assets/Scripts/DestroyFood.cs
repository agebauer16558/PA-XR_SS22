using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFood : MonoBehaviour
{
      private TeleportFood teleportFood; 
      
      public void DestroyCopiedGameObjects (string tag) {
        if ( GameObject.FindWithTag(tag)) {
                     
            foreach(int value in teleportFood.cloanCounter) {
                teleportFood.cloanCounter[value] = 0;
            }

            teleportFood.foodOnPlate = false; 
            
            Destroy(GameObject.FindWithTag(tag));

        }
    }
}
