using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultibossCoordinator: MonoBehaviour
{
	//This exists bc I wanted a single event that would control a whole pack
	public UnityEvent<int> comboEvent = new UnityEvent<int>();
}