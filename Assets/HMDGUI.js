    // Array of menu item control names.
    var menuOptions = new String[4];
     
    menuOptions[0] = "Tutorial";
    menuOptions[1] = "Play";
    menuOptions[2] = "High Scores";
    menuOptions[3] = "Exit";
     
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
        if (GUI.Button(Rect(10,70,170,30), "Button 1 (tutorial,id:0)")) {
            selectedIndex = 0;
            handleSelection();
        }
     
        GUI.SetNextControlName ("Play");
        if (GUI.Button(Rect(10,100,170,30), "Button 2 (play, id:1)")) {
            selectedIndex = 1;
            handleSelection();
        }
     
        GUI.SetNextControlName ("High Scores");
        if (GUI.Button(Rect(10,130,170,30), "Button 3 (high scores, id:2)")) {
            selectedIndex = 2;
            handleSelection();
        }
     
        GUI.SetNextControlName ("Exit");
        if (GUI.Button(Rect(10,160,170,30), "Button 4 (exit, id:3)")) {
            selectedIndex = 3;
            handleSelection();
        }
     
        GUI.FocusControl (menuOptions[selectedIndex]);
    }