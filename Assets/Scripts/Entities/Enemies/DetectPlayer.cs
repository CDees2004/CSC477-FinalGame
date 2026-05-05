using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D c){
        if (c.CompareTag("Player")){
            SendMessageUpwards("DetectPlayer", SendMessageOptions.DontRequireReceiver);
        }
    }
}
