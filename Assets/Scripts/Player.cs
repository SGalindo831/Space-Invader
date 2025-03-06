using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
  [FormerlySerializedAs("bullet")]
  public GameObject bulletPrefab;
  public Transform shottingOffset;

//  void Start()
//   {
//     Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
//   }

//   void OnDestroy()
//   {
//     Enemy.OnEnemyDied -= EnemyOnOnEnemyDied;
//   }

//   void EnemyOnOnEnemyDied(int points)
//   {
//     Debug.Log($"I know about dead enemy, points: {points}");
//   }
    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        GameObject shot = Instantiate(bulletPrefab, shottingOffset.position, Quaternion.identity);
        Debug.Log("Bang!");

        // Destroy(shot, 3f);

      }
    }
}
