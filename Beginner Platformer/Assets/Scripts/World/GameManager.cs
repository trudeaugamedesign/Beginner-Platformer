using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    private GameObject mainCamera;
    private Animator anim;

    [Header("Settings")]
    public string currentRespawnPoint;  
    public float fadeOutTime;
    public float deathTime;
    public bool changingScenes;
    public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)){
            
            if (!changingScenes && player.health <= 0){
                StartCoroutine(ResetGame());
            }

            PauseMenu();
        }
    }

    void PauseMenu(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (paused){
                paused = false;
                anim.SetBool("Paused", false);
                Time.timeScale = 1;
            }   else {
                paused = true;
                anim.SetBool("Paused", true);
                Time.timeScale = 0;
            }
        }
    }
    IEnumerator ResetGame(){
        // Set bool
        changingScenes = true;

        // Wait for player death animation to finish
        yield return new WaitForSeconds(deathTime);

        // Start Animation
        anim.SetBool("FadeOut", true);

        // Wait for fadeout animation to finish
        yield return new WaitForSeconds(fadeOutTime);

        // Load the sync and wait for the scene to load
        AsyncOperation load = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!load.isDone){
            yield return null;
        }

        SetReferences();

        // Move player to respawn point position and freeze movement
        player.transform.position = GameObject.Find(currentRespawnPoint).transform.position;
        mainCamera.transform.position = player.transform.position;
        player.movementLocked = true;

        // Start animation
        anim.SetBool("FadeOut", false);

        // Wait for animation to finish
        yield return new WaitForSeconds(fadeOutTime);

        // Set bool
        changingScenes = false;
        player.movementLocked = false;
    }

    void SetReferences(){
        
        // Set references
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void OnChangeRespawnPoint(string point){
        // Find the current respawn point and disable the animation
        GameObject.Find(currentRespawnPoint).GetComponent<Animator>().SetBool("Active", false);

        // Find the next respawn point and enable the animaton
        GameObject.Find(point).GetComponent<Animator>().SetBool("Active", true);

        // Record new point
        currentRespawnPoint = point;
    }

    // For the play button
    public void PlayButton(){
        if (!changingScenes){
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame(){
        // Set bool
        changingScenes = true;

        // Start Animation
        anim.SetBool("FadeOut", true);

        // Wait for animation to finish
        yield return new WaitForSeconds(fadeOutTime);

        // Load the sync and wait for the scene to load
        AsyncOperation load = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!load.isDone){
            yield return null;
        }

        SetReferences();

        // Start animation
        anim.SetBool("FadeOut", false);

        // Wait for animation to finish
        yield return new WaitForSeconds(fadeOutTime);

        // Set bool
        changingScenes = false;
        player.movementLocked = false;

    }

    // For the return to main menu button
    public void ReturnToMainMenu(){
        if (!changingScenes){
            StartCoroutine(Reset());
        }
    }  


    // Resets the game and returns the game to the main menu
    IEnumerator Reset(){
        // Set bool
        changingScenes = true;

        // Start animation
        anim.SetBool("FadeOut", true);

        // Stop pause
        Time.timeScale = 1;

        // Wait for animation to finish
        yield return new WaitForSeconds(fadeOutTime);

        // Load the sync and wait for the scene to load
        AsyncOperation load = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        while (!load.isDone){
            yield return null;
        }

        // Destroy game object
        Destroy(gameObject);

    }                                      

}
