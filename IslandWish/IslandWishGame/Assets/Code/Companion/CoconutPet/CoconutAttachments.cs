using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Coco Stuff")]
public class CoconutAttachments : ScriptableObject
{
    public List<int> nums;
    public List<GameObject> attachments;

    public int GetIDFromAccessory(GameObject accessory)
    {
        if(attachments.Contains(accessory))
		{
            return nums[attachments.IndexOf(accessory)];
		}

        return 0;
    }

    public GameObject GetAccessoryFromID(int ID)
	{
        if(nums.Contains(ID))
		{
            return attachments[nums.IndexOf(ID)];
		}

        return null;
	}
}
