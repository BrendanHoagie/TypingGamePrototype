using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemiesList;
    [SerializeField] string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesList.Count <= 0)
        {
            SceneManager.LoadScene(nextScene);
        }
        enemiesList.RemoveAll(s => s == null);
    }
}
