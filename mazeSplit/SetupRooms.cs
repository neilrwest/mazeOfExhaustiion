namespace AdventureGameNamespace {
    public partial class AdventureGame
{
    
    // Randomly places rooms in the grid, ensuring connectivity.
    private void SetupRoomsRandomly()
    {
        // Ensure at least GRID_SIZE rooms, up to about half the grid
        //int roomCount = random.Next(GRID_SIZE * GRID_SIZE / 2) + GRID_SIZE;
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

private void ConnectRooms(int i, int j)
    {
        ConnectIfExists(i, j, i + 1, j, "North", "South");
        ConnectIfExists(i, j, i - 1, j, "South", "North");
        ConnectIfExists(i, j, i, j + 1, "East", "West");
        ConnectIfExists(i, j, i, j - 1, "West", "East");
    }
    
    private void ConnectIfExists(int i1, int j1, int i2, int j2, string dir1, string dir2)
{
    if (i2 >= 0 && i2 < GRID_SIZE && j2 >= 0 && j2 < GRID_SIZE && grid[i2, j2] != null)
    {
        // 57% chance of locking the new exit which generates a new key
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

