namespace AdventureGameNamespace {
    public partial class AdventureGame
{
    
    // Randomly places rooms in the grid, ensuring connectivity.
    private void SetupRoomsRandomly()
    {
        // Ensure at least GRID_SIZE rooms, up to about half the grid
        //int roomCount = random.Next(GRID_SIZE * GRID_SIZE / 2) + GRID_SIZE;
        //hardcode to 12 for now
        int roomCount = GRID_SIZE * GRID_SIZE / 2;
        int i = random.Next(GRID_SIZE);
        int j = random.Next(GRID_SIZE);
        grid[i, j] = new Room("Room 1");
        allRooms.Add(grid[i, j]);

        // Assign rooms based on an adjacency basis
        for (int k = 1; k < roomCount; k++)
        {
            int newI, newJ;
            do
            {
                newI = random.Next(GRID_SIZE);
                newJ = random.Next(GRID_SIZE);
            }
            while (grid[newI, newJ] != null || !IsAdjacent(newI, newJ));

            // Create room and connect to neighbors
            grid[newI, newJ] = new Room("Room " + (k + 1));
            allRooms.Add(grid[newI, newJ]);
            ConnectRooms(newI, newJ);
        }
    }
    
    // Checks if (i, j) is adjacent (N/S/E/W) to at least one existing room.
    private bool IsAdjacent(int i, int j)
    {
        return ((i + 1 < GRID_SIZE) && grid[i + 1, j] != null) ||
               ((i - 1 >= 0)       && grid[i - 1, j] != null) ||
               ((j + 1 < GRID_SIZE) && grid[i, j + 1] != null) ||
               ((j - 1 >= 0)       && grid[i, j - 1] != null);
               
    }

    /*
    private bool IsAdjacent(int i, int j)
    {
        return ((i + 1 < GRID_SIZE) && grid[i + 1, j] != null) ||
               ((i - 1 >= 0)       && grid[i - 1, j] != null) ||
               ((j + 1 < GRID_SIZE) && grid[i, j + 1] != null) ||
               ((j - 1 >= 0)       && grid[i, j - 1] != null) ||
               ((i + 1 < GRID_SIZE && j + 1 < GRID_SIZE) && grid[i + 1, j + 1] != null) ||
               ((i - 1 >= 0 && j - 1 >= 0) && grid[i - 1, j - 1] != null)  ||
               ((i + 1 < GRID_SIZE && j - 1 >= 0) && grid[i + 1, j - 1] != null) ||
               ((i -1 >= 0 && j + 1 < GRID_SIZE) && grid[i - 1, j + 1] != null);
               
    }
    */

    // Connect newly created room with all existing neighbors.
    /*private void ConnectRooms(int i, int j)
    {
        ConnectIfExists(i, j, i + 1, j, "North", "South");
        ConnectIfExists(i, j, i - 1, j, "South", "North");
        ConnectIfExists(i, j, i, j + 1, "East", "West");
        ConnectIfExists(i, j, i, j - 1, "West", "East");
        ConnectIfExists(i, j, i + 1, j + 1, "North-East", "South-West");
        ConnectIfExists(i, j, i - 1, j - 1, "South-West", "North-East");
        ConnectIfExists(i, j, i + 1, j - 1, "North-West", "South-East");
        ConnectIfExists(i, j, i - 1, j + 1, "South-East", "North-West");
    }*/

private void ConnectRooms(int i, int j)
    {
        ConnectIfExists(i, j, i + 1, j, "North", "South");
        ConnectIfExists(i, j, i - 1, j, "South", "North");
        ConnectIfExists(i, j, i, j + 1, "East", "West");
        ConnectIfExists(i, j, i, j - 1, "West", "East");
    }
    
    // 40% chance of locking the exit, which generates a new key.
    private void ConnectIfExists(int i1, int j1, int i2, int j2, string dir1, string dir2)
{
    if (i2 >= 0 && i2 < GRID_SIZE && j2 >= 0 && j2 < GRID_SIZE && grid[i2, j2] != null)
    {
        // 57% chance of locking
        int result = random.Next(7); // 0-4
        if (result < 4)
        {
            Key key = new Key(keyList.Count);
            keyList.Add(key);
            grid[i1, j1].SetExit(dir1, grid[i2, j2], true, key);
            grid[i2, j2].SetExit(dir2, grid[i1, j1], true, key);
        }
        else
        {
            grid[i1, j1].SetExit(dir1, grid[i2, j2]);
            grid[i2, j2].SetExit(dir2, grid[i1, j1]);
        }
    }
}
}
}

/*namespace AdventureGameNamespace {
    public partial class AdventureGame
{
    
    // Randomly places rooms in the grid, ensuring connectivity.
    private void SetupRoomsRandomly()
    {
        // Ensure at least GRID_SIZE rooms, up to about half the grid
        //int roomCount = random.Next(GRID_SIZE * GRID_SIZE / 2) + GRID_SIZE;
        //hardcode to 12 for now
        int roomCount = GRID_SIZE * GRID_SIZE / 2;
        numRooms = roomCount;
        int i = (GRID_SIZE / 2) - 1;
        int j = (GRID_SIZE / 2) - 1;
        grid[i, j] = new Room("Room 1");
        allRooms.Add(grid[i, j]);
        // Assign rooms based on an adjacency basis
        for (int k = 1; k < roomCount; k++)
        {
            int newI, newJ;
            do
            {
                newI = random.Next(2) == 0 ? i + 1 : i - 1;
                newJ = random.Next(2) == 0 ? j + 1 : j - 1;
            }
            while (grid[newI, newJ] != null || !IsAdjacent(newI, newJ));

            // Create room and connect to neighbors
            grid[newI, newJ] = new Room("Room " + (k + 1));
            allRooms.Add(grid[newI, newJ]);
            ConnectRooms(newI, newJ);
            i = newI;
            j = newJ;
        }
    }
    
    // Checks if (i, j) is adjacent (N/S/E/W) to at least one existing room.
    private bool IsAdjacent(int i, int j)
    {
        return ((i + 1 < GRID_SIZE) && grid[i + 1, j] != null) ||
               ((i - 1 >= 0)       && grid[i - 1, j] != null) ||
               ((j + 1 < GRID_SIZE) && grid[i, j + 1] != null) ||
               ((j - 1 >= 0)       && grid[i, j - 1] != null);
    }

    // Connect newly created room with all existing neighbors.
    private void ConnectRooms(int i, int j)
    {
        ConnectIfExists(i, j, i + 1, j, "North", "South");
        ConnectIfExists(i, j, i - 1, j, "South", "North");
        ConnectIfExists(i, j, i, j + 1, "East", "West");
        ConnectIfExists(i, j, i, j - 1, "West", "East");
    }
    
    // 40% chance of locking the exit, which generates a new key.
    private void ConnectIfExists(int i1, int j1, int i2, int j2, string dir1, string dir2)
{
    if (i2 >= 0 && i2 < GRID_SIZE && j2 >= 0 && j2 < GRID_SIZE && grid[i2, j2] != null)
    {
        // 40% chance of locking
        int result = random.Next(5); // 0-4
        if (result < 2)
        {
            Key key = new Key(keyList.Count);
            keyList.Add(key);
            grid[i1, j1].SetExit(dir1, grid[i2, j2], true, key);
            grid[i2, j2].SetExit(dir2, grid[i1, j1], true, key);
        }
        else
        {
            grid[i1, j1].SetExit(dir1, grid[i2, j2]);
            grid[i2, j2].SetExit(dir2, grid[i1, j1]);
        }
    }
}
}
}*/