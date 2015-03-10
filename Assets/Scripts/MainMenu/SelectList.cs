using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectList : MonoBehaviour {
	public static object List(ICollection list, object selected, GUIStyle defaultStyle, GUIStyle selectedStyle) {			
		foreach(object item in list) {
			if(GUILayout.Button(item.ToString(), (selected == item) ? selectedStyle : defaultStyle)) {
				if(selected == item) { // Clicked an already selected item. Deselect.
					selected = null;
				} else {
					selected = item;
				}
			}
		}
		
		return selected;
	}
	
	public delegate bool OnListItemGUI(object item, bool selected, ICollection list);
	public delegate void OnListItemClicked(object item, ICollection list);
	
	public static object List(ICollection list, object selected, OnListItemGUI itemHandler, OnListItemClicked clickHandler) {
		ArrayList itemList = new ArrayList(list);
		
		foreach(object item in itemList) {
			if(itemHandler(item, item == selected, list)) {
				selected = item;
				clickHandler(item, list);
			} else if(selected == item) { // If we *were* selected, but aren't any more then deselect
				selected = null;
			}
		}
		
		return selected;
	}
}
