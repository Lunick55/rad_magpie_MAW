using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutAttachmentReference : MonoBehaviour
{
	public List<GameObject> bodyAttachments;
	public List<GameObject> legAttachments;

	public int GetAttachmentCount()
	{
		int num = bodyAttachments.Count;

		num += (int)(legAttachments.Count * 0.5f);

		return num;
	}

	public void SetAttachmentStatus(int index, bool status)
	{
		if(index < bodyAttachments.Count)
		{
			bodyAttachments[index].SetActive(status);
			return;
		}

		index -= bodyAttachments.Count;
		index *= 2;

		legAttachments[index].SetActive(status);
		legAttachments[index+1].SetActive(status);
	}
}
