using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuilding : MonoBehaviour {

    [System.Serializable]
    public class Buildings
    {
        public string name;
        public int maxNb;
        public int minNb;
        public GameObject spawnPrefab;
        public float maxDistance;
        public float minDistance;
        public int maxTry;
        [Range(0f,0.5f)]
        public float hieghtFromFloor;
    }

    public Buildings[] buildings;
    public GameObject tower;
    public GameObject wall;
    public float wallOffset;
    public List<Collider> colliderList;
    public List<Vector3> listPtBuildgs;
    public Vector3[] envelop;
    public float wallLength;


    void GeneratePointOnMap(Buildings building)
    {
        int indexer = 0;
        bool redo = true;
        while(redo && indexer< building.maxTry)
        {
            redo = false;
            float distance = Random.Range(building.minDistance, building.maxDistance);
            transform.localEulerAngles = GenerateVillage.GetYRandomRotation();
            Vector2 angle = Random.insideUnitCircle;
            Vector3 point = new Vector3(angle.x * distance, 0, angle.y * distance);

            point.y = GenerateVillage.FindHeight(point, transform);
            GameObject spawnedObjct = Instantiate(building.spawnPrefab, point, transform.rotation);
            Collider test = spawnedObjct.GetComponent<Collider>();
            colliderList.Add(test);
            test.gameObject.transform.position = test.gameObject.transform.position + new Vector3(0, test.bounds.size.y * building.hieghtFromFloor, 0);
            foreach (Collider collider in colliderList)
            {
                if (test != collider)
                    if (test.bounds.Intersects(collider.bounds))
                    {
                        Destroy(test.gameObject);
                        colliderList.Remove(test);
                        redo = true;
                        break;
                    }
            }
            point.y = 0;
            listPtBuildgs.Add(point);
            indexer++;
        }
    }

    public void GenrateVillage()
    {
        DeleteVillageNow();
        CrreateVillageNow();
    }

    public void CrreateVillageNow()
    {
        int buildingsNb;
        foreach(Buildings building in buildings)
        {
            buildingsNb = Random.Range(building.minNb, building.maxNb);
            for (int i =0; i < buildingsNb; i++)
            {
                GeneratePointOnMap(building);
            }
        }
        WallPath.GetConvexHull(listPtBuildgs);
        


        Vector3 towerPosition = WallPath.towersPos[0] + (WallPath.towersDir[0] * wallOffset);
        towerPosition.y = GenerateVillage.FindHeight(towerPosition, transform);
        GameObject towerObject = Instantiate(tower, towerPosition, Quaternion.identity);
        towerObject.transform.position += new Vector3(0, towerObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);
        Vector3 wallDirection = Vector3.ClampMagnitude((WallPath.towersPos[0+ 1]+(WallPath.towersDir[0] * wallOffset)) - towerPosition, 1); ;
        float wallDistance = Vector3.Distance(WallPath.towersPos[0+ 1], towerPosition);
        int nbWalls = (int)(wallDistance / wallLength)+1;
        Vector3 wallPosition = towerPosition;
        for (int j = 0; j < nbWalls; j++)
        {
                wallPosition += wallDirection * wallLength;
                wallPosition.y = GenerateVillage.FindHeight(wallPosition, transform);
                GameObject wallObject = Instantiate(wall, wallPosition, Quaternion.identity);
                wallObject.transform.LookAt(new Vector3(towerPosition.x, wallPosition.y, towerPosition.z));
                wallObject.transform.position += new Vector3(0, wallObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);

        }
        wallPosition += wallDirection * (wallLength / 2f);
        wallPosition.y = GenerateVillage.FindHeight(wallPosition, transform);
        towerPosition = wallPosition;


        for (int i = 1; i < WallPath.towersPos.Length-1; i++)
        {
            towerPosition.y = GenerateVillage.FindHeight(towerPosition, transform);
            towerObject = Instantiate(tower, towerPosition, Quaternion.identity);
            towerObject.transform.position += new Vector3(0, towerObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);
            wallDirection = Vector3.ClampMagnitude((WallPath.towersPos[i + 1] + (WallPath.towersDir[i] * wallOffset)) - towerPosition, 1); ;
            wallDistance = Vector3.Distance(WallPath.towersPos[i + 1], towerPosition);
            nbWalls = (int)(wallDistance / wallLength) + 1;
            wallPosition = towerPosition;
            for (int j = 0; j < nbWalls; j++)
            {
                    wallPosition += wallDirection * wallLength;
                    wallPosition.y = GenerateVillage.FindHeight(wallPosition, transform);
                    GameObject wallObject = Instantiate(wall, wallPosition, Quaternion.identity);
                    wallObject.transform.LookAt(new Vector3(towerPosition.x, wallPosition.y, towerPosition.z));
                    wallObject.transform.position += new Vector3(0, wallObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);
            }
            wallPosition += wallDirection * (wallLength / 2f);
            wallPosition.y = GenerateVillage.FindHeight(wallPosition, transform);
            towerPosition = wallPosition;
        }

        towerPosition.y = GenerateVillage.FindHeight(towerPosition, transform);
        towerObject = Instantiate(tower, towerPosition, Quaternion.identity);
        towerObject.transform.position += new Vector3(0, towerObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);
        wallDirection = Vector3.ClampMagnitude((WallPath.towersPos[0] + (WallPath.towersDir[0] * wallOffset)) - towerPosition, 1); ;
        wallDistance = Vector3.Distance(WallPath.towersPos[0], towerPosition);
        nbWalls = (int)(wallDistance / wallLength) + 1;
        wallPosition = towerPosition;
        for (int j = 0; j < nbWalls-2; j++)
        {
                wallPosition += wallDirection * wallLength;
                wallPosition.y = GenerateVillage.FindHeight(wallPosition, transform);
                GameObject wallObject = Instantiate(wall, wallPosition, Quaternion.identity);
                wallObject.transform.LookAt(new Vector3(towerPosition.x, wallPosition.y, towerPosition.z));
                wallObject.transform.position += new Vector3(0, wallObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);
        }
        wallPosition += wallDirection * (wallLength / 2f);
        wallPosition.y = GenerateVillage.FindHeight(wallPosition, transform);
        towerPosition = wallPosition;
        towerPosition.y = GenerateVillage.FindHeight(towerPosition, transform);
        Debug.Log(GenerateVillage.FindHeight(towerPosition, transform));
        Debug.Log(towerPosition.y);
        towerObject = Instantiate(tower, towerPosition, Quaternion.identity);
        Debug.Log(towerObject.transform.position.y);
        towerObject.transform.position += new Vector3(0, towerObject.GetComponent<Collider>().bounds.size.y * 0.3f, 0);

        GameObject[] buildingsFromVillage = GameObject.FindGameObjectsWithTag("VillageBuilding");
        foreach(GameObject building in buildingsFromVillage)
        {
            building.transform.parent = transform;
        }
    }

    public void DeleteVillageNow()
    {
        GameObject[] buildingsFromVillage = GameObject.FindGameObjectsWithTag("VillageBuilding");
        foreach (GameObject building in buildingsFromVillage)
        {
            Destroy(building);
        }
    }
}
