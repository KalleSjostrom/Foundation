using UnityEngine;
using System.Collections;

namespace Foundation {
	public interface IActivatable
	{
		void SetActive(bool active);
		bool IsActive();
	}
}
