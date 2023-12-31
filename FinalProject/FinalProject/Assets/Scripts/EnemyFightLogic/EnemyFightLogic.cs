using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyFightLogic : MonoBehaviour
{
    public int enemyEnergy;
    private GameObject player;
    private playerGridMovement _playerGridMovement;
    private Vector2 initialPosition, lastPosition;
    public Animator enemyAnimator;
    public Animator playerAnimator;
    private bool PlayerDeath = false;
    private bool EnemyDeath = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        _playerGridMovement = player.GetComponent<playerGridMovement>();
        initialPosition = transform.position;
        lastPosition = initialPosition;
    }
    private void Update()
    {
        enemyEnergy = GetComponent<EnemyGridMovemtn>().recall;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EnemyDeath = false;
            if (_playerGridMovement.recall > enemyEnergy)
            {
                PlayerWin();
            }

            if (_playerGridMovement.recall <= enemyEnergy)
            { 
                PlayerLost();
            }
        }
    }
    public void CheckCollisionWithPlayer()
    {
        lastPosition = transform.position;
       if (initialPosition == player.GetComponent<playerGridMovement>().lastPosition && lastPosition == player.GetComponent<playerGridMovement>().initialPosition)
        {
            EnemyDeath = false;
            if (_playerGridMovement.recall > enemyEnergy)
            {
                PlayerWin();
            }

            if (_playerGridMovement.recall <= enemyEnergy)
            {
                PlayerLost();
            }
        }
        initialPosition = lastPosition;
    }
    public void PlayerWin()
    {
        Debug.Log("player win");
        GetComponent<EnemyGridMovemtn>().isItAlive= false;
        EnemyDeath = true;
        StartCoroutine(WaitForDeathAnimation());
    }
    public void PlayerLost()
    {
        PlayerDeath = true;
        _playerGridMovement.DisableControls();
        _playerGridMovement.restartEnemyDices?.Invoke();
        _playerGridMovement.moveEnemies?.Invoke();
        StartCoroutine(WaitForDeathAnimation());
        Finish.lastSceneName = SceneManager.GetActiveScene().name;
    }
    private IEnumerator WaitForDeathAnimation()
    {
        if (EnemyDeath == true)
        {
            enemyAnimator.SetTrigger("IsDeath");
            AudioManager.Instance.PlaySFX("DeathM");
            float tiempoEsperaE = 2f;
            yield return new WaitForSecondsRealtime(tiempoEsperaE);
            gameObject.SetActive(false);
        }
        if (PlayerDeath == true)
        {
            playerAnimator.SetTrigger("IsDeath");
            AudioManager.Instance.PlaySFX("DeathP");
            float tiempoEsperaP = 2.0f;
            yield return new WaitForSecondsRealtime(tiempoEsperaP);
            SceneManager.LoadScene("GameOver");
        }
    }
}