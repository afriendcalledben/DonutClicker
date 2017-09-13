using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {

    // Holds the bounds of the background.
    Vector3 size;
    // Prefabs of all the food items definte in Unity UI.
    public GameObject donut;
    public GameObject cake;
    public GameObject hamburger;
    public GameObject hamegg;
    public GameObject icecream;
    // Gameobjects that hold GUI text for the score and time.
    public GameObject scoreHolder;
    public GameObject timeHolder;
    // Number of seconds between spawning food items.
    float spawnTime = 1.0f;
    // Current score.
    int score = 0;
    // Current number of seconds left.
    int timeLeft = 10;
    // Titles cards for Title and Game Over screen.
    public GameObject title;
    public GameObject gameOver;
    // Current game state.
    string gameState = "start";
    // This integer is used to create a pause between when game over occurs and when the user can click to start again.
    int gameOverCount = 0;

    // Use this for initialization
    void Start () {
        // Nothing needed here.
    }
	
	// Update is called once per frame
	void Update () {
        // If the game is on the Title screen or the Game Over screen and the user clicks, start the game.
        if (Input.GetMouseButtonDown(0) && (gameState == "start" || gameState == "gameover"))
        {
            // Hide the Title graphic.
            title.SetActive(false);
            // Hide the Game Over graphic.
            gameOver.SetActive(false);
            // Set the game state to playing.
            gameState = "playing";
            // Set the score to zero.
            score = 0;
            // Set the time left in seconds to 10.
            timeLeft = 10;
            // Update the text of the on-screen Timer
            updateTimeText(timeLeft);
            // Update the text of the on-screen Score.
            scoreHolder.GetComponent<GUIText>().text = "SCORE: " + score;
            // Start the InvokeRepeating to call spawnFood() every 1 second (as defined by spawnTime).
            InvokeRepeating("spawnFood", spawnTime, spawnTime);
        // If the game is over, set the gamestate to "gameoverblock" so that the game is not running but the user cannot click to restart yet.
        } else if (gameState == "gameoverblock")
        {
            // Increment a count to 500 at which point the user can then click to restart.
            gameOverCount++;
            if (gameOverCount > 500)
            {
                gameState = "gameover";
            }
        }
    }

    void FixedUpdate()
    {
        
    }

    // Function to spawn the food items randomly.
    void spawnFood ()
    {
        // Define that we will be referring to a GameObject as food.
        GameObject food;
        // Select a random index of the food type between 0 and 4.
        int foodType = (int) Mathf.Floor(Random.Range(0.0f, 4.9f));
        // Randomly select an x and y position on the screen with a padding of 2m from the edge of the screen to keep things central.
        float x = -Camera.main.orthographicSize + 2.0f + Random.Range(0.0f, (Camera.main.orthographicSize * 2) - 4.0f);
        float y = -Camera.main.orthographicSize + 2.0f + Random.Range(0.0f, (Camera.main.orthographicSize * 2) - 4.0f);
        // Create a Vector3 to pass as the position above.
        Vector3 spawnPoint = new Vector3(x, y, -5.0f);
        // Create a random rotation to start the food time at. A Quaternion is like a Vector3 for rotation with additional funcionality.
        Quaternion randomRotation = Random.rotation;
        // This switch statement instantiates different food prefabs depending on the foodType integer defined above.
        switch (foodType)
        {
            case 0:
                food = Instantiate(donut, spawnPoint, randomRotation) as GameObject;
                break;
            case 1:
                food = Instantiate(cake, spawnPoint, randomRotation) as GameObject;
                break;
            case 2:
                food = Instantiate(hamburger, spawnPoint, randomRotation) as GameObject;
                break;
            case 3:
                food = Instantiate(hamegg, spawnPoint, randomRotation) as GameObject;
                break;
            case 4:
                food = Instantiate(icecream, spawnPoint, transform.rotation) as GameObject;
                break;
            default:
                food = Instantiate(donut, spawnPoint, transform.rotation) as GameObject;
                break;
        }
        // Tags the gameobject as "Food" so I can identify it later.
        food.tag = "Food";
        // Places the food item of the Food sorting layer. I don't actually know what this does.
        food.GetComponent<Renderer>().sortingLayerName = "Food";
        // Sets the parent of the food gameobject to the Foods gameobject placed in Unity.
        food.transform.SetParent(GameObject.Find("Foods").GetComponent<Transform>());

        // As this function is called every second, I've put code here to deduct a second from the on-screen timer.
        // If I wanted a varying spawnTime I'd have to move this code into it's own coroutine.
        timeLeft--;
        updateTimeText(timeLeft)
        // If the timer reaches zero...
        if (timeLeft == 0)
        {
            // Set the count that blocks restart to zero.
            gameOverCount = 0;
            // Change the gamestate to the one which increments the above value.
            gameState = "gameoverblock";
            // Cancels the InvokeRepeating that spawns food and updates the timer.
            CancelInvoke();
            // Show the Game Over graphic on the stage.
            gameOver.SetActive(true);
            // Returns an array of all Gameobjects on the stage with the tag "Food".
            GameObject[] currentFoods = GameObject.FindGameObjectsWithTag("Food");
            // This for loop goes through and destroys any food items left on the stage.
            for (int i = 0; i < currentFoods.Length; i++)
            {
                GameObject.Destroy(currentFoods[i]);
            }
        }
    }

    // Function to add to the on-screen score counter.
    public void AddToScore()
    {
        score++;
        timeLeft++;
        updateTimeText(timeLeft);
        scoreHolder.GetComponent<GUIText>().text = "SCORE: " + score;
    }

    // Function to remove from the on-screen score counter.
    public void RemoveFromScore()
    {
        score--;
        scoreHolder.GetComponent<GUIText>().text = "SCORE: " + score;
    }

    // Function to update the on-screen timer.
    void updateTimeText(int t)
    {
        timeHolder.GetComponent<GUIText>().text = "TIME: " + t;
    }
}
