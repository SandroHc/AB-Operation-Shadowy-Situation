using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	public RectTransform mainPanel;
	public RectTransform optionsPanel;

	public Image bgImg;
	public Sprite[] bgTextures;

	public Image logoMain;
	public Image logoOver;

	public float logoFactor = 5;
	public float logoMaxDiff = .05f;
	public float logoCurrentDiff;

	public float timer;

	private bool loading = false;

	public Button[] btnList;

	private AsyncOperation ao;

	void Awake() {
		bgImg.sprite = bgTextures[Random.Range(0, bgTextures.Length)];
	}

	void Update() {
		if(!loading) {
			timer += Time.deltaTime * 1.5f;
			timer %= 1;

			logoCurrentDiff = Mathf.Lerp(logoCurrentDiff, (Mathf.Min(Mathf.Cos(timer), Mathf.Sin(timer))) / 6, Time.deltaTime * 10);

			logoMain.transform.localScale = Vector3.one * (1 - logoCurrentDiff);
			logoOver.transform.localScale = Vector3.one * (1 + logoCurrentDiff);
		}
	}

	void OnGUI() {
		if(ao != null) {
			if(ao.isDone) {
				ao = null;
				loading = false;
			} else {
				logoOver.fillAmount = Mathf.Lerp(logoOver.fillAmount, 1 - ao.progress, Time.deltaTime * 1.6f);
			}
		}
	}

	public void showOptions() {
		mainPanel.gameObject.SetActive(false);
		optionsPanel.gameObject.SetActive(true);
	}

	public void showMainPanel() {
		mainPanel.gameObject.SetActive(true);
		optionsPanel.gameObject.SetActive(false);
	}

	public void btnClickPlay() {
		if(ao == null) {
			ao = Application.LoadLevelAsync(1);
			ao.priority = 10;

			logoOver.fillAmount = 1;
			logoOver.color = Color.gray;
			logoOver.transform.localScale = Vector2.one;
			loading = true;

			foreach(Button btn in btnList)
				btn.gameObject.SetActive(false);
		}
	}

	public void btnClickExit() {
		Application.Quit();
	}

	/*		OPTIONS		*/
	public void resetChanges() {
		// TODO Rollback changes to PlayersPrefs
		showMainPanel();
	}

	public void applyChanges() {
		PlayerPrefs.Save();
		showMainPanel();
	}
}