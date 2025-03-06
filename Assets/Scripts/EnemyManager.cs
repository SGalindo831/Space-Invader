using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [Header("Enemy Prefabs")]
    public GameObject basicEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject advancedEnemyPrefab;
    public GameObject eliteEnemyPrefab;
    
    [Header("Formation Settings")]
    //Number of rows in formation
    public int rows = 5; 
    // Number of enemies per row
    public int enemiesPerRow = 11;
    public float horizontalSpacing = 1.0f;
    public float verticalSpacing = 0.8f;
    //Top left position of formation
    public Vector2 startPosition = new Vector2(-5f, 4f);
    
    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;
    public float moveDownDistance = 0.5f;
    public float boundaryLeft = -5.0f;
    public float boundaryRight = 5.0f;
    
    [Header("Gizmo Settings")]
    public Color boundaryColor = Color.red;
    public Color spawnAreaColor = Color.green;
    public bool showGizmos = true;
    
    // Point values for each enemy type
    public Dictionary<string, int> pointValues = new Dictionary<string, int>()
    {
        { "BasicEnemy", 10 },
        { "MediumEnemy", 20 },
        { "AdvancedEnemy", 30 },
        { "EliteEnemy", 50 }
    };
    
    private GameObject formationContainer;
    private bool movingRight = true;
    private int enemiesDestroyed = 0;
    private int totalEnemies = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        SpawnEnemyFormation();
    }
    
    void SpawnEnemyFormation()
    {
        formationContainer = new GameObject("EnemyFormation");
        
        enemiesDestroyed = 0;
        totalEnemies = 0;
        
        // Create rows of enemies
        for (int row = 0; row < rows; row++)
        {
            GameObject prefabToUse = GetPrefabForRow(row);
            
            for (int col = 0; col < enemiesPerRow; col++)
            {
                Vector2 position = new Vector2(startPosition.x + col * horizontalSpacing, startPosition.y - row * verticalSpacing);
                
                GameObject enemy = Instantiate(prefabToUse, position, Quaternion.identity);
                enemy.transform.parent = formationContainer.transform;
                
                if (enemy.GetComponent<Enemy>() == null)
                {
                    enemy.AddComponent<Enemy>();
                }
                
                totalEnemies++;
            }
        }
        
        Debug.Log($"Spawned {totalEnemies} enemies in formation");
    }
    
    GameObject GetPrefabForRow(int row)
    {
        // Map row to enemy type
        switch (row)
        {
            case 0:
                return eliteEnemyPrefab;
            case 1:
            case 2:
                return advancedEnemyPrefab;
            case 3:
                return mediumEnemyPrefab;
            default:
                return basicEnemyPrefab;
        }
    }
    
    void Update()
    {
        MoveFormation();
    }
    
    void MoveFormation()
    {
        if (formationContainer == null || formationContainer.transform.childCount == 0)
            return;
        
        //Calculate current speed based on enemies destroyed
        float speedMultiplier = 1.0f + (enemiesDestroyed * 0.05f);
        float currentSpeed = moveSpeed * speedMultiplier;
    
        if (movingRight)
        {
            formationContainer.transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
        }
        else
        {
            formationContainer.transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
        }
        
        float leftBound = 100f;
        float rightBound = -100f;
        
        foreach (Transform child in formationContainer.transform)
        {
            float posX = child.position.x;
            if (posX < leftBound) leftBound = posX;
            if (posX > rightBound) rightBound = posX;
        }
        
        //Check if formation reached a boundary
        if (rightBound >= boundaryRight || leftBound <= boundaryLeft)
        {
            movingRight = !movingRight;
            
            formationContainer.transform.Translate(Vector2.down * moveDownDistance);
        }
    }
    
    public void EnemyDestroyed(string enemyTag)
    {
        enemiesDestroyed++;
        int pointValue = GetPointValueForTag(enemyTag);
        
        GameManager.Instance.AddScore(pointValue);
        Debug.Log($"{enemyTag} destroyed for {pointValue} points. Total: {enemiesDestroyed}/{totalEnemies}");
        
        // Check if all enemies are gone
        if (formationContainer != null && formationContainer.transform.childCount <= 1)
        {
            Debug.Log("All enemies destroyed! Victory!");
        }
    }
    
    //Get point value for a specific enemy tag
    public int GetPointValueForTag(string tag)
    {
        if (pointValues.ContainsKey(tag))
        {
            return pointValues[tag];
        }
        return 10;
    }
    
    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
            
        Gizmos.color = boundaryColor;
        
        Gizmos.DrawLine(
            new Vector3(boundaryLeft, 10f, 0f), 
            new Vector3(boundaryLeft, -10f, 0f)
        );
        
        Gizmos.DrawLine(
            new Vector3(boundaryRight, 10f, 0f), 
            new Vector3(boundaryRight, -10f, 0f)
        );
        
        Gizmos.color = spawnAreaColor;
    
        float width = (enemiesPerRow - 1) * horizontalSpacing;
        float height = (rows - 1) * verticalSpacing;
        
        //Draw spawn area outline
        Vector3 topLeft = new Vector3(startPosition.x, startPosition.y, 0);
        Vector3 topRight = new Vector3(startPosition.x + width, startPosition.y, 0);
        Vector3 bottomLeft = new Vector3(startPosition.x, startPosition.y - height, 0);
        Vector3 bottomRight = new Vector3(startPosition.x + width, startPosition.y - height, 0);
        
        //Draw the four edges of the spawn area
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.color = new Color(spawnAreaColor.r, spawnAreaColor.g, spawnAreaColor.b, 0.5f);
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < enemiesPerRow; col++)
            {
                Vector3 position = new Vector3(
                    startPosition.x + col * horizontalSpacing,
                    startPosition.y - row * verticalSpacing,
                    0
                );
                
                Gizmos.DrawSphere(position, 0.1f);
            }
        }
    }
}