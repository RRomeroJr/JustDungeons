using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultibossCoordinator: MonoBehaviour
{
	public UnityEvent<int> comboEvent = new UnityEvent<int>();
}