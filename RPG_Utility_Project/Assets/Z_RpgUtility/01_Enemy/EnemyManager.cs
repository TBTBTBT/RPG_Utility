using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviourCanDestroy<EnemyManager> {
    GameObject player;
    List<EnemyBase> _spawnEnemies = new List<EnemyBase>();
    int max = 8;
    float spawnRatio = 1f;
	// Use this for initialization
	void Start () {

        StartCoroutine(Spawner());
	}
    static public void Spawn(int num , Vector2 pos){
        if(Instance){
            if (Instance._spawnEnemies.Count < Instance.max)
            {
                string n = num.ToString();
                if (num < 10)
                {
                    n = "0" + num;
                }

                Instance._spawnEnemies.Add(((GameObject)Instantiate(Resources.Load("Prefab/Enemy/Enemy_" + n), pos, Quaternion.identity)).GetComponent<EnemyBase>());
            }
        }
    }
    void DeSpawn(float range){
        _spawnEnemies.ForEach((e) => {
            if (e != null)
            {
                if(player == null){
                    return;
                }
                if ((e.transform.position - player.transform.position).magnitude > range)
                {
                    Destroy(e.gameObject);
                    //Debug.Log("Despawn");
                }
            }else{
                Debug.Log("n");
            }
        });
    }
    IEnumerator Spawner(){
        while (true)
        {
            if (!player)
            {
                player = GameObject.FindWithTag("Player");
            }
            DeSpawn(12);
            _spawnEnemies.RemoveAll(item => item == null);
            //Debug.Log("enemies : " + _spawnEnemies.Count);
            if (player)
            {
                float len = 7;
                Vector2 pos = MathUtil.RandomAngle(len);
                pos += (Vector2)player.transform.position;
                if (FieldManager.Instance.IsFieldPassable(FieldManager.Instance.PositionToIndex(pos)))
                {
                    Spawn(0, pos);
                }
               // Debug.Log("enemies : " + _spawnEnemies.Count);
            }
            yield return new WaitForSeconds(spawnRatio);
        }
    }
}
