using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Coco Stuff")]
public class CoconutAttachments : ScriptableObject
{
    public List<GameObject> attachments;
    public List<GameObject> bodies;

    public int GetIDFromAccessory(GameObject accessory)
    {
        if(attachments.Contains(accessory))
		{
            return attachments.IndexOf(accessory);
		}

        return -1;
    }

    public int GetIDFromBody(GameObject body)
    {
        if(bodies.Contains(body))
		{
            return bodies.IndexOf(body);
		}

        return -1;
    }

 //   public GameObject GetAccessoryFromID(int ID)
	//{
 //       if(attachments.Count <= ID)
	//	{
 //           return attachments[ID];
 //       }

 //       return null;
	//}

 //   public GameObject GetBodyFromID(int ID)
	//{
 //       if(bodies.Count <= ID)
	//	{
 //           return bodies[ID];
 //       }

 //       return null;
	//}
}
