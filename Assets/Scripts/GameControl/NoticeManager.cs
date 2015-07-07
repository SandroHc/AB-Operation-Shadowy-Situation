using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class NoticeManager : MonoBehaviour {
	private static NoticeManager INSTANCE;

	private static Queue<NoticeDetail> list = new Queue<NoticeDetail>();
	private static float EXPIRE_TIME = 5; // Time, in seconds, to remove a notice

	public Text noticeUI;

	void Awake() {
		INSTANCE = this;
		updateUI();
    }

	void Update() {
		// Is the fist notice expired, update the UI
		if(checkQueue())
			updateUI();
	}

	public static void addNotice(string notice) {
		if(string.IsNullOrEmpty(notice))
			return;

		list.Enqueue(new NoticeDetail(notice));

		if(INSTANCE != null)
			INSTANCE.updateUI();

		// TODO: Play sound
	}

	private static bool checkQueue() {
		if(list.Count > 0) {
			NoticeDetail obj = list.Peek();
			if(obj.created + EXPIRE_TIME < Time.timeSinceLevelLoad) {
				list.Dequeue();
				return true;
			}
		}
		return false;
	}

	private void updateUI() {
		NoticeDetail[] arr = list.ToArray();
		StringBuilder sb = new StringBuilder(arr.Length * 2);
		foreach(NoticeDetail obj in arr)
			sb.Append(obj.text).Append("\n");

		noticeUI.text = sb.ToString();
    }

	public class NoticeDetail {
		public string text { get; protected set; }
		public float created { get; protected set; }

		public NoticeDetail(string text) {
			this.text = text;
			this.created = Time.timeSinceLevelLoad;
		}
	}
}
