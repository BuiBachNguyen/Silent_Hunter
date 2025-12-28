using UnityEngine;

public class EnemyHurt : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }    
    }

}
