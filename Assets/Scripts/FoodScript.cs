using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    //Update is called every frame
    void Update()
    {
        // Rotate the food item around the X and Y axis in a loop
        transform.Rotate(new Vector3(45, 45, 0) * Time.deltaTime);
        // Slowly shrink the the food item over time
        transform.localScale -= new Vector3(0.05F, 0.05F, 0.05F);
        // If the food item's scale is less tha zero, remove the GameObject and deduct from the score.
        if (transform.localScale.x < 0.0f)
        {
            // Find the MainScript and call the function which removes from the score.
            GameObject.Find("MainManager").GetComponent<MainScript>().RemoveFromScore();
            // Destroy the object.
            GameObject.Destroy(this.gameObject);
            
        }
    }

    private void OnMouseDown()
    {
        // If the food item is clicked, add to the score and remove from the object.
        GameObject.Find("MainManager").GetComponent<MainScript>().AddToScore();
        GameObject.Destroy(this.gameObject);
    }
}
