using UnityEngine;
public class HMDGUI : MonoBehaviour
{
	// Array of menu item control names.
    string[] menuOptions = new string[]
	{
		"Tutorial",
		"Play",
		"High Scores",
		"Exit"
	};
     
    // Default selected menu item (in this case, Tutorial).
     
    int selectedIndex = 0;
     
	
	float IPD = 0.08f;
	float zdist = 0.0f;
	float screenvc = Screen.height/2.0f;
	
    // Function to scroll through possible menu items array, looping back to start/end depending on direction of movement.
     
    int menuSelection (string[] menuItems, int selectedItem, string direction) {
     
        if (direction == "up") {
            if (selectedItem == 0) {
                selectedItem = menuItems.Length - 1;
            } else {
                selectedItem -= 1;
            }
        }
       
        if (direction == "down") {
            if (selectedItem == menuItems.Length - 1) {
                selectedItem = 0;
            } else {
                selectedItem += 1;
            }
        }
     
        return selectedItem;
    }
     
     
     
    void Update ()
    {
        if (Input.GetKeyDown("down")) {
            selectedIndex = menuSelection(menuOptions, selectedIndex, "down");
        }
     
        if (Input.GetKeyDown("up")) {
            selectedIndex = menuSelection(menuOptions, selectedIndex, "up");
        }
		
		if (Input.GetKeyDown("left")) {
            zdist += 0.001f;
        }
     
        if (Input.GetKeyDown("right")) {
            zdist -= 0.001f;
        }
     
        if (Input.GetKeyDown("space") || Input.GetKeyDown("enter")) {   
            handleSelection();
        }
    }
     
    void handleSelection()
    {
        GUI.FocusControl (menuOptions[selectedIndex]);
       
        switch (selectedIndex)
        {
            case 0:  
                   Debug.Log("Selected name: " + GUI.GetNameOfFocusedControl () + " / id: " +selectedIndex);
				   TextMesh mesh = (TextMesh) GameObject.Find("Hearts").GetComponent<TextMesh>();
				   if(mesh != null)
						mesh.text += "<3";
                   break;
            case 1:
                   Debug.Log("Selected name: " + GUI.GetNameOfFocusedControl () + " / id: " +selectedIndex);
                   break;
            case 2:
                   Debug.Log("Selected name: " + GUI.GetNameOfFocusedControl () + " / id: " +selectedIndex);
                   break;
            case 3:
                   Debug.Log("Selected name: " + GUI.GetNameOfFocusedControl () + " / id: " +selectedIndex);
                   break;         
            default:
                   Debug.Log ("None of the above selected..");
                   break;
        }
    }
     
    void OnGUI ()
    {
        GUI.SetNextControlName ("Tutorial");
        int leftx = (int)(Screen.width / 4.0 - zdist*Screen.width);
        int rightx = (int)(3.0 * Screen.width / 4.0 - IPD * Screen.width + zdist*Screen.width);
        int sizey = 30;
        int y = (int)(screenvc - sizey / 2.0 - 0.2 * Screen.height);
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        y = (int)(screenvc - sizey / 2.0);
        
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        y = (int)(screenvc - sizey / 2.0 + 0.2 * Screen.height);
        
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        /*
        // NEXT ELEMENT
        
        leftx = (int)(Screen.width / 4.0);
        rightx = (int)(3.0 * Screen.width / 4.0 - IPD * Screen.width);
        GUI.SetNextControlName ("Play");
        
        y = (int)(Screen.height / 2.0 - sizey / 2.0 - 0.2 * Screen.height);
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        
        y = (int)(Screen.height / 2.0 - sizey / 2.0);
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        
        y = (int)(Screen.height / 2.0 - sizey / 2.0 + 0.2 * Screen.height);
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
     
       // NEXT ELEMENT
        leftx = Screen.width / 4.0 + 0.1 * Screen.width;
        rightx = 3.0 * Screen.width / 4.0 - IPD * Screen.width + 0.1 * Screen.width;
        GUI.SetNextControlName ("High Scores");
        
        y = Screen.height / 2.0 - sizey / 2.0 - 0.2 * Screen.height;
        if (GUI.Button(Rect(leftx,y,30,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
        y = Screen.height / 2.0 - sizey / 2.0;
         if (GUI.Button(Rect(leftx,y,30,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
        y = Screen.height / 2.0 - sizey / 2.0 + 0.2 * Screen.height;
         if (GUI.Button(Rect(leftx,y,30,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
		*/
     
        GUI.FocusControl (menuOptions[selectedIndex]);
    }
}