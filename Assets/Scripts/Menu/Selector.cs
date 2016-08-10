using UnityEngine;
using System.Collections;
using System.Linq;

public class Selector : MonoBehaviour
{
	public float speed = 10f;
	public SelectableButton firstButton;

    public Transform top, bottom, left, right;
    public Transform frame;

	private SelectableButton currentItem;
	private Vector3 oldMouseLocation;

	private bool canControl = true;

	[HideInInspector] public Vector3 position;

	void Awake()
	{
		currentItem = firstButton;
		position = firstButton.renderer.bounds.center;
        Highlight();
	}

	void Update()
	{
		CheckForInput();

        //transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);

        transform.position = position;

		if(canControl)
		{
			bool controlsEntered = false;
			if(Player.One.input.GetAxis("Vertical") > 0 && currentItem.up != null)
			{
				controlsEntered = true;
				position = currentItem.up.renderer.bounds.center;
				currentItem = currentItem.up.GetComponent<SelectableButton>();
			}
			else if(Player.One.input.GetAxis("Vertical") < 0 && currentItem.down != null)
			{
				controlsEntered = true;
				position = currentItem.down.renderer.bounds.center;
				currentItem = currentItem.down.GetComponent<SelectableButton>();
			}
			else if(Player.One.input.GetAxis("Horizontal") > 0 && currentItem.right != null)
			{
				controlsEntered = true;
				position = currentItem.right.renderer.bounds.center;
				currentItem = currentItem.right.GetComponent<SelectableButton>();
			}
			else if(Player.One.input.GetAxis("Horizontal") < 0 && currentItem.left != null)
			{
				controlsEntered = true;
				position = currentItem.left.renderer.bounds.center;
				currentItem = currentItem.left.GetComponent<SelectableButton>();
			}
			else if(Player.One.input.GetButtonDown("Select"))
			{
				controlsEntered = true;
				SendActivate();
			}
			if(controlsEntered)
			{
				canControl = false;
				Invoke("RestoreControl", 0.25f);
			}
		}

		if(oldMouseLocation != Input.mousePosition)
		{
			oldMouseLocation = Input.mousePosition;
			RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
			if(hit.collider)
			{
				SelectableButton sb = hit.collider.gameObject.GetComponent<SelectableButton>();
				if(sb != null && sb.mouseable)
				{
					currentItem = sb;
					position = sb.renderer.bounds.center;
				}
			}
		}
	}

    void Highlight()
    {
        //frame.position = currentItem.transform.position;

        Vector3 width = new Vector3(currentItem.collider2D.bounds.size.x + 0.5f, 0.5f, 0.5f);
        Vector3 height = new Vector3(0.5f, currentItem.collider2D.bounds.size.y + 0.5f, 0.5f);

        Vector3 newScale = transform.localScale;

        newScale.y = height.y;
        newScale.x = width.x;

        transform.localScale = newScale;

        newScale = left.localScale;
        newScale.y = 0.06f / transform.localScale.y;
        left.localScale = newScale;
        right.localScale = newScale;

        Vector3 newpos = currentItem.collider2D.bounds.center;
        newpos.z -= 6.5f;
        newpos.y += 0.75f;
        frame.position = newpos;

        //newpos.y = currentItem.collider2D.bounds.max.y;
        //top.position = newpos;
        //top.localScale = width;

        //newpos.y = currentItem.collider2D.bounds.min.y - 0.25f;
        //bottom.position = newpos;
        //bottom.localScale = width;

        //newpos = transform.position;
        //newpos.z -= 4f;

        //newpos.x = currentItem.collider2D.bounds.max.x;
        //right.position = newpos;
        //right.localScale = height;

        //newpos.x = currentItem.collider2D.bounds.min.x;
        //left.position = newpos;
        //left.localScale = height;
    }

	void CheckForInput()
	{
		Player p = Player.All.FirstOrDefault(item => item.input.GetAxis("Horizontal") != 0);
		if(p != null)
			HandlePlayerInputHorizontal(p);
		else
		{
			p = Player.All.FirstOrDefault(item => item.input.GetAxis("Vertical") != 0);
			if(p != null)
				HandlePlayerInputVertical(p);
		}
	}

	IEnumerator RestoreControls(Player player)
	{
		yield return new WaitForSeconds(0.18f);
		player.menuControlsEnabled = true;
	}

	void RestoreControl()
	{
		canControl = true;
	}

	void HandlePlayerInputHorizontal(Player player)
	{
		if(MainMenu.Instance.currentMenu == MainMenu.MenuArea.Controls && player.menuControlsEnabled)
		{
            player.menuControlsEnabled = false;
            StartCoroutine("RestoreControls", player);
            MenuControllerSwitcher.instance.UpdateDisplay();
		}
	}

	void HandlePlayerInputVertical(Player player)
	{
		Debug.Log("I see vertical input");
		if(FindObjectOfType<MainMenu>().currentMenu == MainMenu.MenuArea.CharacterSelect && player.menuControlsEnabled)
		{
			Debug.Log("Handleing vertical input");
			player.menuControlsEnabled = false;
			StartCoroutine("RestoreControls", player);
			if(player.input.GetAxis("Vertical") > 0)
				MainMenuCharacterSelector.GetCharacterSelectorByIndex(player.index).Next();
			else
				MainMenuCharacterSelector.GetCharacterSelectorByIndex(player.index).Previous();
		}
		else if(MainMenu.Instance.currentMenu == MainMenu.MenuArea.LevelSelect && player.menuControlsEnabled)
		{
			player.menuControlsEnabled = false;
			StartCoroutine("RestoreControls", player);
			if(player.input.GetAxis("Horizontal") > 0)
				LevelSwitcher.instance.Next();
			else
				LevelSwitcher.instance.Previous();
		}
	}

	public void SendActivate()
	{
		Debug.Log("sending activate");
		FindObjectOfType<MainMenu>().currentMenu = currentItem.areaToChangeTo;
		currentItem.Activate();
		if(currentItem.activated != null)
		{
			position = currentItem.activated.renderer.bounds.center;
			currentItem = currentItem.activated.GetComponent<SelectableButton>();
		}
        transform.position = position;
        Highlight();
	}
}
