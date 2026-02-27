using System.Collections;
using UnityEngine;

public class Companionscript : MonoBehaviour
{
    private int _damage =2;
    public int Damage { get { return _damage; } set { _damage = value; } }
    private EnemyController enemyController;
    private void Start()
    {
        StartCoroutine(Autoattack());
    }
    
    private IEnumerator Autoattack()
    {
        if(enemyController== null)
        {
            enemyController = Object.FindAnyObjectByType<EnemyController>();
        }
        yield return new WaitForSeconds(3f);
        enemyController.TakeDamage(Damage);

        StartCoroutine(Autoattack());
    }
}
