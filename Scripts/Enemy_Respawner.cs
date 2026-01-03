using UnityEngine;

public class Enemy_Respawner : MonoBehaviour
{
    [SerializeField]private GameObject enemyPrefab;
    [SerializeField]private Transform[] respawnPoints;
    [SerializeField]private float cooldown = 2f;
    [Space]
    [SerializeField]private float cooldownDecreaseRate = .05f;
    [SerializeField]private float cooldownCap = .7f;
    private float timer;
    private Transform player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = cooldown;
            CreateNewEnemy();

            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        bool createdOnTheRight = newEnemy.transform.position.x > player.position.x; //verifica se o inimigo foi criado à direita do jogador

        if (createdOnTheRight)//Pode ser escrito aqui "if (newEnemy.transform.position.x > player.position.x)"
            newEnemy.GetComponent<Enemy>().Flip();
    }
}
