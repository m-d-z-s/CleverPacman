using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public int Score { get; private set; }
    public int Lives { get; private set; }

    private void Start() { NewGame(); }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach (var pellet in this.pellets) // Enable all pellets
            pellet.gameObject.SetActive(true);
        
        ResetState(); // Reset the state of the ghosts and pacman
    }

    private void ResetState()
    {
        for (int i = 0; i < this.ghosts.Length; i++) this.ghosts[i].gameObject.SetActive(true); // Enable all ghosts
        this.pacman.gameObject.SetActive(true); // Enable pacman
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.length; i++) this.ghosts[i].gameObject.SetActive(false); // Disable all ghosts
        this.pacman.gameObject.SetActive(false); // Disable pacman
    }
    
    private void SetScore(int score) { Score = score; }
    
    private void SetLives(int lives) { Lives = lives; }
    
    public void GhostIsEaten(Ghost ghost)
    {
        SetScore(Score + ghost.pointsForKill);
        /*ghost.gameObject.SetActive(false);
        if (AllGhostsAreEaten()) NewRound();*/
    }
    
    public void PacManIsEaten()
    {
        this.pacman.gameObject.SetActive(false);
        SetLives(this.Lives - 1);
        if (this.Lives == 0) GameOver();
        else Invoke(nameof(ResetState), 3.0f);
    }
}
