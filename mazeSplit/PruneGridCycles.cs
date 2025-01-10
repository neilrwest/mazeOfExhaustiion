namespace AdventureGameNamespace {
    public partial class AdventureGame
{
    private void PruneCycles()
    {
        HashSet<Room> visited = new HashSet<Room>();
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (grid[i, j] != null && !visited.Contains(grid[i, j]))
                {
                    PruneCyclesDFS(grid[i, j], visited, null);
                }
            }
        }
    }

    private void PruneCyclesDFS(Room room, HashSet<Room> visited, Room parent)
    {
        visited.Add(room);

        var exitsToRemove = new List<string>();
        var neighborExitsToRemove = new List<KeyValuePair<Room, string>>();

        // CopyOnWrite approach: snapshot the dictionary entries
        var entries = room.Exits.ToList(); 
        foreach (var entry in entries)
        {
            var neighbor = entry.Value.GetOtherRoom(room);
            if (!visited.Contains(neighbor))
            {
                PruneCyclesDFS(neighbor, visited, room);
            }
            else if (neighbor != parent)
            {
                // Cycle detected: remove connection with 50% chance
                if (random.Next(7) < 4 && room.Exits.Count > 1 && neighbor.Exits.Count > 1)
                {
                    //if (!entry.Value.IsLocked()) {
                    // Mark for removal
                    exitsToRemove.Add(entry.Key);
                    neighborExitsToRemove.Add(new KeyValuePair<Room, string>(neighbor, GetOppositeDirection(entry.Key)));

                    // If there's a key associated, remove it from keyList
                    Key key = entry.Value.GetKey();
                    if (key != null)
                    {
                        KeyList.Remove(key);
                    }
                    break; // optional
                    //}
                }
            }
        }

        // Remove collected exits
        foreach (string direction in exitsToRemove)
        {
            room.Exits.Remove(direction);
        }
        foreach (var kvp in neighborExitsToRemove)
        {
            kvp.Key.Exits.Remove(kvp.Value);
        }
    }
        // Helper function to return opposite exit direction
    private string GetOppositeDirection(string direction)
    {
        switch (direction)
        {
            case "North": return "South";
            case "South": return "North";
            case "East":  return "West";
            case "West":  return "East";
            case "North-East": return "South-West";
            case "North-West": return "South-East";
            case "South-East": return "North-West";
            case "South-West": return "North-East";
        }
        return "";
    }

}
}
