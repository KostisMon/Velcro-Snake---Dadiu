using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
public class PauseButton : BaseObject, IPauseEnter, IPauseExit{
	#region IPauseEvents implementation
	public void OnPauseEnter()
	{
		image.sprite = unpause;
	}
	public void OnPauseExit()
	{
		image.sprite = gear;
	}
	#endregion
	public Sprite gear, unpause;
	private Image image;

	protected override void EarlyAwake()
	{
		image = GetComponent<Image>();
	}
}
