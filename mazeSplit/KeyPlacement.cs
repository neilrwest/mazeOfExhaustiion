namespace AdventureGameNamespace {
    public partial class AdventureGame
{
    // Checks if the room at (roomI, roomJ) can be reached without the key that might appear there.
    private bool IsAccessibleWithoutKey(int roomI, int roomJ)
    {
        HashSet<Room> visited = new HashSet<Room>();
        return DfsWithoutKey(currentRoom, grid[roomI, roomJ], visited);
    }

    // Depth First Search for key placement  check
    private bool DfsWithoutKey(Room current, Room target, HashSet<Room> visited)
    {
        if (current == target)
        {
            return true;
        }
        visited.Add(current);

        foreach (var kvp in current.Exits)
        {
            Exit exit = kvp.Value;
            Room neighbor = exit.GetOtherRoom(current);
            if (!exit.IsLocked() && !visited.Contains(neighbor))
            {
                if (DfsWithoutKey(neighbor, target, visited))
                {
                    return true;
                }
            }
        }
        return false;
    }

        // This method often gets stuck in Infinite Loops, set a timer to reinitialise the grid
        // and try again if it takes too long.
    private void AssignKeysToRoomsWithTimeout()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(5)); // Timeout after 2 seconds

        try
        {
            Task assignTask = Task.Run(() => AssignKeysToRooms(cts.Token), cts.Token);
            assignTask.Wait(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Key assignment timed out. Reinitializing program...");
            ReinitializeProgram();
        }
        catch (AggregateException ex) when (ex.InnerException is OperationCanceledException)
        {
            Console.WriteLine("Key assignment timed out. Reinitializing program...");
            ReinitializeProgram();
        }
        catch (InvalidOperationException ex)
        {
            //Console.WriteLine($"Key assignment failed: {ex.Message}. Reinitializing program...");
            Console.WriteLine($"Key assignment failed. Reinitializing program...");
            ReinitializeProgram();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Key assignment failed: {ex.Message}. Reinitializing program...");
            ReinitializeProgram();
        }
    }

    
    
    // Calls the DFS to ensure the keys are not placed creating a deadlock
private void AssignKeysToRooms(CancellationToken token)
{
    var keyEnumerator = keyList.ToList(); // Snapshot
    bool placementFailed = false;
    foreach (var key in keyEnumerator)
    {
        int roomI, roomJ;
        int attempts = 0;
        const int maxAttempts = 1000;
        do
        {
            token.ThrowIfCancellationRequested(); // Respect cancellation requests
            roomI = random.Next(GRID_SIZE);
            roomJ = random.Next(GRID_SIZE);
            attempts++;
            if (attempts >= maxAttempts)
            {
                Console.WriteLine($"Failed to place key {key.GetNumber()} after {maxAttempts} attempts.");
                placementFailed = true; // Flag failure
                break;
            }
        }
        while (grid[roomI, roomJ] == null 
               || grid[roomI, roomJ].HasKey() 
               || grid[roomI, roomJ].IsFinal() 
               || !IsAccessibleWithoutKey(roomI, roomJ));

        if (!placementFailed)
        {
            grid[roomI, roomJ].SetKey(key);
            keyList.Remove(key);
        }
    }
    if (placementFailed)
    {
        throw new InvalidOperationException("Key placement failed. Triggering reinitialization.");
    }
}
}
}