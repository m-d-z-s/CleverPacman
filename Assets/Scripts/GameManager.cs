using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    //public Pacman pacman; // for non-ai pacman
    public PacmanAI pacman;
    public Transform pellets;
    public int countOfPellet = 0;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
         gameOverText.enabled = false;
        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
            countOfPellet++;
        }
        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }
        pacman.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;
        for (int i = 0; i < ghosts.Length; i++) ghosts[i].gameObject.SetActive(false);
        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        livesText.enabled = true;
        this.lives = lives;
        livesText.text = "Your lives:\n x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        scoreText.enabled = true;
        this.score = score;
        scoreText.text = "Your score:\n " + score.ToString();
    }
 
    public void PacmanEaten()
    {
		pacman.gameObject.SetActive(false);
        pacman.DeathSequence();
        SetLives(lives - 1);
        if (lives > 0) Invoke(nameof(ResetState), 3.0f);
        else GameOver();
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        countOfPellet--;

        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }


    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) { // enable frightened mode for all ghosts
            ghosts[i].frightened.Enable(pellet.duration);
        }
        
        PelletEaten(pellet); // add points for eating the power pellet
        countOfPellet--;
        CancelInvoke();
        // CancelInvoke(nameof(ResetGhostMultiplier)); // cancel any previous calls to ResetGhostMultiplier
        Invoke(nameof(ResetGhostMultiplier), pellet.duration); // reset the ghost multiplier after the power pellet duration
    }


    private bool HasRemainingPellets()
    {
		var hasRemainingPellets = false; // assume there are no remaining pellets
        foreach (Transform pellet in pellets) // check each pellet
        {
            if (pellet.gameObject.activeSelf) { // if the pellet is active (not eaten) then there are remaining pellets
                hasRemainingPellets = true; // set the flag to true
				break; // break out of the loop
            }
        }

        return hasRemainingPellets; // return the flag
    }

    private void ResetGhostMultiplier() // reset the ghost multiplier
    {
        ghostMultiplier = 1; // reset the multiplier to 1
    }

}