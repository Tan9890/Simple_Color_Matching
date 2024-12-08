using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SurroundingSPawner : MonoBehaviour
{
    public Transform SurroundingHolder;
    public float minRadR, maxRadR, minRadT, maxRadT, TreeSizeMin, TreeSizeMax;
    public int nRocks, nTrees, maxRetryCount;

    public List<GameObject> RockPrefabs, TreePrefabs;

    public bool SpawnRocksandTrees;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnRocksandTrees)
        {
            List<MeshRenderer> tempG = GetComponentsInChildren<MeshRenderer>().ToList();
            if (tempG.Count > 0)
            {
                foreach (MeshRenderer m in tempG)
                    DestroyImmediate(m.gameObject);
            }
            for (int i = 0; i < nRocks; i++)
            {
                GameObject rock = Instantiate(RockPrefabs[Random.Range(0, RockPrefabs.Count)], SurroundingHolder);
                rock.AddComponent<BoxCollider>();
                rock.layer = LayerMask.NameToLayer("Ignore Raycast");
                rock.transform.localPosition = RandomPointCircle(minRadR, maxRadR);
                rock.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                int retryCount = 0;
                while (IsOverlappingAnything(rock) && retryCount < maxRetryCount)
                {
                    rock.transform.localPosition = RandomPointCircle(minRadR, maxRadR);
                    rock.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                    retryCount++;
                    //Debug.Log("Overlapped rock");
                }
                if (retryCount >= maxRetryCount)
                    Debug.Log("Couldnt place");
            }
            for (int i = 0; i < nTrees; i++)
            {
                GameObject tree = Instantiate(TreePrefabs[Random.Range(0, TreePrefabs.Count)], SurroundingHolder);
                tree.AddComponent<BoxCollider>();
                tree.layer = LayerMask.NameToLayer("Ignore Raycast");
                float scale = Random.Range(TreeSizeMin, TreeSizeMax);
                tree.transform.localScale = new Vector3(scale, scale, scale);
                tree.transform.localPosition = RandomPointCircle(minRadT, maxRadT);
                tree.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                int retryCount = 0;
                while (IsOverlappingAnything(tree) && retryCount < maxRetryCount)
                {
                    tree.transform.localPosition = RandomPointCircle(minRadT, maxRadT);
                    tree.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                    retryCount++;
                    //Debug.Log("Overlapped tree");
                }
                if (retryCount >= maxRetryCount)
                    Debug.Log("Couldnt place");


            }
            SpawnRocksandTrees = false;
        }
    }

    Vector3 RandomPointCircle(float minRad, float maxRad)
    {
        float angle = Random.Range(0,360f);
        Vector3 spawnpoint = Vector3.zero;
        spawnpoint.x = Mathf.Sin(angle * Mathf.Deg2Rad) * Random.Range(minRad, maxRad);
        spawnpoint.z = Mathf.Cos(angle * Mathf.Deg2Rad) * Random.Range(minRad, maxRad);
        return spawnpoint;
    }

    bool IsOverlappingAnything(GameObject _obj)
    {
        BoxCollider bc = _obj.GetComponent<BoxCollider>();
        return Physics.OverlapBox(bc.center, bc.size / 2f, _obj.transform.rotation, LayerMask.NameToLayer("IgnoreRaycast")).Length > 0 ? true : false;

    }


}
