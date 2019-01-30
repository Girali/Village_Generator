using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateVillage {

    public static Vector3 GetRandomRotation()
    {
        Quaternion rotationR = Random.rotation;
        Vector3 v = rotationR.eulerAngles;
        v.y = 0;
        return v;
    }

    public static Vector3 GetYRandomRotation()
    {
        Quaternion rotationR = Random.rotation;
        Vector3 v = new Vector3(0, rotationR.eulerAngles.y, 0);
        return v;
    }

    public static float FindHeight(Vector3 localPositionFromParent , Transform worldParent)
    {
        //Init
        RaycastHit hit;
        //cast ray to the top from the point where to spawn gameObject
        //check if hit exist and if hit is a chunk
        localPositionFromParent = new Vector3(localPositionFromParent.x, 0, localPositionFromParent. z);

        if (Physics.Raycast(localPositionFromParent+ worldParent.position, Vector3.down, out hit))
        {
            if (hit.collider.tag == "Chunk")
            {
                //out
                return hit.point.y;
            }
        }
        if (Physics.Raycast(localPositionFromParent+ worldParent.position, Vector3.up, out hit))
        {
            if (hit.collider.tag == "Chunk")
            {
                //out
                return hit.distance;
            }
        }
        return 0f;
    }

    static float Angle(Vector3 from , Vector3 to)
    {
        float ang = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.z > 0)
            ang = 360 - ang;
        return ang;
    }

    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.y;
    }


    public static Vector3[] JarivsConvexEnv (List<Vector3> listPoint)
    {
        List<Vector3> Emvelop = new List<Vector3>();
        Vector3 vectCurent = Vector3.forward;
        Vector3 vectNext = Vector3.zero;
        float maxLeft = float.MinValue;
        // recherche du point le plus bas,
        foreach(Vector3 pt in listPoint)
        {
            if (-pt.x > maxLeft)
            {
                maxLeft = -pt.x;
                vectNext = pt;
            }
        }
        Emvelop.Add(vectNext);
        // puis à chaque étape recherche du point qui forme le plus petit
        // angle, avec le segment des 2 points précédents de l'enveloppe
        int index = 0;
        do
        {
            Vector3 tempVect = Vector3.zero;
            float tempAngle = 0;

            foreach (Vector3 pt in listPoint)
            {
                if (pt != vectCurent)
                {
                    vectNext = Emvelop[index] - pt;
                    Debug.Log(CalculateAngle(vectCurent, vectNext));
                    if (CalculateAngle(vectCurent, vectNext) > tempAngle)
                    {
                        tempVect = pt;
                        tempAngle = Angle(vectCurent, vectNext);
                    }
                    else if (CalculateAngle(vectCurent, vectNext) == tempAngle)
                    {
                        if (Vector3.Distance(Emvelop[index], pt) > Vector3.Distance(Emvelop[index], tempVect))
                        {
                            tempVect = pt;
                            tempAngle = Angle(vectCurent, vectNext);
                        }
                    }
                }
            }
            Debug.Log(CalculateAngle(vectCurent, vectNext));
            vectCurent = Emvelop[index] - tempVect;
            Emvelop.Add(tempVect);
            index++;

        } while (Emvelop[Emvelop.Count - 1] != Emvelop[0] && index < 100 );

        // recherche du point le plus bas à droite

        // recherche du point suivant

        // points alignés : choisir le plus éloigné

        // on est retombé sur le point de départ
        return Emvelop.ToArray();
    }
}
