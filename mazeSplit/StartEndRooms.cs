namespace AdventureGameNamespace {
    public partial class AdventureGame
{
    private void InitializeCurrentRoom()
{
    // 1) Find the two farthest Rooms by shortest distance
    Room bestA = null;
    Room bestB = null;
    int maxDistance = -1;

    foreach (var roomA in allRooms)
    {
        foreach (var roomB in allRooms)
        {
            if (roomA == roomB) continue;
            
            int dist = GetDistance(roomA, roomB);
            if (dist != int.MaxValue && dist > maxDistance)
            {
                maxDistance = dist;
                bestA = roomA;
                bestB = roomB;
            }
        }
    }

    // Set the farthest pair as start & final
    currentRoom = bestA;
    bestB.SetFinal(true);
    finalRoom = bestB;
}

    // Helper function  
private int GetDistance(Room start, Room end)
{
    if (start == end) return 0;

    // Distances to each room, default = very large
    Dictionary<Room, int> distMap = new Dictionary<Room, int>();
    foreach (Room r in allRooms)
    {
        distMap[r] = int.MaxValue;
    }
    distMap[start] = 0;

    // 0-1 BFS uses a deque
    Deque<Room> dq = new Deque<Room>();
    dq.AddLast(start);

    while (dq.Count > 0)
    {
        Room current = dq.RemoveFirst();
        int currentDist = distMap[current];

        foreach (Exit exit in current.Exits.Values) 
        {   
            // Weight the edges so locked exits have a lower cost than unlocked exits
            // Encouraging paths through exits
            // Unlocked edges to count as cost=2
            int cost = exit.IsLocked() ? 1 : 2;

            Room neighbor = exit.GetOtherRoom(current);
            int newDist = currentDist + cost;

            if (newDist < distMap[neighbor])
            {
                distMap[neighbor] = newDist;

                // 0-1 BFS trick: 
                //   If cost=1, push to front 
                //   If cost=2, push to back
                if (cost == 1)
                    dq.AddFirst(neighbor);
                else
                    dq.AddLast(neighbor);
            }
        }
    }

    return distMap[end];
}

    }
}