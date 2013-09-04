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
     
        if (Input.GetKeyDown("space")) {   
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
        int leftx = (int)(Screen.width / 4.0 - 0.1 * Screen.width);
        int rightx = (int)(3.0 * Screen.width / 4.0 - 0.08 * Screen.width - 0.1 * Screen.width);
        int sizey = 30;
        int y = (int)(Screen.height / 2.0 - sizey / 2.0 - 0.2 * Screen.height);
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        y = (int)(Screen.height / 2.0 - sizey / 2.0);
        
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        y = (int)(Screen.height / 2.0 - sizey / 2.0 + 0.2 * Screen.height);
        
        if (GUI.Button(new Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(new Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        // NEXT ELEMENT
        
        leftx = (int)(Screen.width / 4.0);
        rightx = (int)(3.0 * Screen.width / 4.0 - 0.08 * Screen.width);
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
     
       /* GUI.SetNextControlName ("Exit");
        if (GUI.Button(Rect(leftx,Screen.height / 2.0,170,30), "Button 4 (exit, id:3)")) {
            selectedIndex = 3;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,Screen.height / 2.0,170,30), "Button 4 (exit, id:3)")) {
            selectedIndex = 3;
            handleSelection();
        }*/
     
        GUI.FocusControl (menuOptions[selectedIndex]);
    }
}