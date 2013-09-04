    // Array of menu item control names.
    var menuOptions = new String[4];
     
    menuOptions[0] = "Tutorial";
    menuOptions[1] = "Play";
    menuOptions[2] = "High Scores";
    menuOptions[3] = "Exit";
     
    var IPD = 0.08;
    var zdist = 0.0;
    // Default selected menu item (in this case, Tutorial).
     
    var selectedIndex = 0;
     
    // Function to scroll through possible menu items array, looping back to start/end depending on direction of movement.
     
    function menuSelection (menuItems, selectedItem, direction) {
     
        if (direction == "up") {
            if (selectedItem == 0) {
                selectedItem = menuItems.length - 1;
            } else {
                selectedItem -= 1;
            }
        }
       
        if (direction == "down") {
            if (selectedItem == menuItems.length - 1) {
                selectedItem = 0;
            } else {
                selectedItem += 1;
            }
        }
     
        return selectedItem;
    }
     
     
     
    function Update ()
    {
        if (Input.GetKeyDown("down")) {
            selectedIndex = menuSelection(menuOptions, selectedIndex, "down");
        }
     
        if (Input.GetKeyDown("up")) {
            selectedIndex = menuSelection(menuOptions, selectedIndex, "up");
        }
        
        if (Input.GetKeyDown("left")) {
            zdist += 0.001;
        }
     
        if (Input.GetKeyDown("right")) {
            zdist -= 0.001;
        }
     
        if (Input.GetKeyDown("space")) {   
            handleSelection();
        }
    }
     
    function handleSelection()
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
     
    function OnGUI ()
    {
        GUI.SetNextControlName ("Tutorial");
        var leftx = Screen.width / 4.0 - zdist*Screen.width;
        var rightx = 3.0 * Screen.width / 4.0 - IPD * Screen.width + zdist*Screen.width;
        var sizey = 30;
        var sizex = 30;
       /* leftx -= sizex / 2.0;
        rightx -= sizex / 2.0;*/
        var y = Screen.height / 2.0 - sizey / 2.0 - 0.2 * Screen.height;
        if (GUI.Button(Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        y = Screen.height / 2.0 - sizey / 2.0;
        
        if (GUI.Button(Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        y = Screen.height / 2.0 - sizey / 2.0 + 0.2 * Screen.height;
        
        if (GUI.Button(Rect(leftx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
        
        // NEXT ELEMENT
       /* 
        leftx = Screen.width / 4.0;
        rightx = 3.0 * Screen.width / 4.0 - IPD * Screen.width;
        GUI.SetNextControlName ("Play");
        
        y = Screen.height / 2.0 - sizey / 2.0 - 0.2 * Screen.height;
        if (GUI.Button(Rect(leftx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        
        y = Screen.height / 2.0 - sizey / 2.0;
        if (GUI.Button(Rect(leftx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        
        y = Screen.height / 2.0 - sizey / 2.0 + 0.2 * Screen.height;
        if (GUI.Button(Rect(leftx,y,30,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
        if (GUI.Button(Rect(rightx,y,30,30), "Button 2 (play, id:1)")) {
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