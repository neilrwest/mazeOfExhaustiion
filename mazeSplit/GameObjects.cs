namespace AdventureGameNamespace {
public class Room
{
    public string Name;
    // Use a Dictionary<string, Exit> in C#
    public Dictionary<string, Exit> Exits;
    private Key key;
    private bool isFinal;

    private Bomb bomb;

    public Room(string name)
    {
        this.Name = name;
        this.Exits = new Dictionary<string, Exit>();
        this.isFinal = false;
    }

    public void SetExit(string direction, Room neighbor, bool locked, Key key)
    {
        Exit exit = new Exit(this, neighbor, locked, key);
        Exits[direction] = exit;
        neighbor.Exits[GetOppositeDirection(direction)] = exit;
    }

    public void SetExit(string direction, Room neighbor, bool locked)
    {
        SetExit(direction, neighbor, locked, null);
    }

    public void SetExit(string direction, Room neighbor)
    {
        SetExit(direction, neighbor, false, null);
    }

    public Exit GetExit(string direction)
    {
        return Exits.ContainsKey(direction) ? Exits[direction] : null;
    }

    public bool IsExitLocked(string direction)
    {
        Exit exit = GetExit(direction);
        return exit != null && exit.IsLocked();
    }

    public void SetKey(Key key)
    {
        this.key = key;
    }

    public Key GetKey()
    {
        return key;
    }

    public void SetBomb(Bomb bomb)
    {
        this.bomb = bomb;
    }

    public bool HasBomb()
    {
        return bomb != null;
    }

    public Bomb GetBomb()
    {
        return bomb;
    }

    public void SetFinal(bool isFinal)
    {
        this.isFinal = isFinal;
    }

    public bool IsFinal()
    {
        return isFinal;
    }

    public bool HasKey()
    {
        return key != null;
    }

    private string GetOppositeDirection(string direction)
    {
        switch (direction)
        {
            case "North": return "South";
            case "South": return "North";
            case "East": return "West";
            case "West": return "East";
            case "North-East": return "South-West";
            case "North-West": return "South-East";
            case "South-East": return "North-West";
            case "South-West": return "North-East";
        }
        return "";
    }

    public override string ToString()
    {
        return Name;
    }

    public void LockExit(string direction)
    {
        Exit exit = GetExit(direction);
        if (exit != null)
        {
            exit.SetLocked(true);
        }
    }
}

/// Represents a connection between two Rooms.

public class Exit
{
    private Room room1;
    private Room room2;
    private bool locked;
    private Key key;

    public Exit(Room room1, Room room2, bool locked, Key key)
    {
        this.room1 = room1;
        this.room2 = room2;
        this.locked = locked;
        this.key = key;
    }

    public Room GetOtherRoom(Room currentRoom)
    {
        return currentRoom == room1 ? room2 : room1;
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void SetLocked(bool value)
    {
        locked = value;
    }

    public Key GetKey()
    {
        return key;
    }
}

// Represents a Key that can lock/unlock certain exits. 
// Each key has a unique integer 'number' to identify it.

public class Key
{
    private int number;

    public Key(int number)
    {
        this.number = number;
    }

    public int GetNumber()
    {
        return this.number;
    }

    public override bool Equals(object obj)
    {
        if (this == obj) return true;
        if (obj == null || this.GetType() != obj.GetType()) return false;
        Key other = (Key)obj;
        return this.number == other.number;
    }

    public override int GetHashCode()
    {
        return number.GetHashCode();
    }

    public override string ToString()
    {
        return "" + this.number;
    }
}

// Corresponds to JSON anagram object, bomb <=> anagram
public class Bomb
{
    public string Anagram { get; set; }
    public string Context { get; set; }
    public string Solution { get; set; }
}


// Generic Class for Breadth First Search
public class Deque<T>
{
    private LinkedList<T> list = new LinkedList<T>();

    public void AddFirst(T item) => list.AddFirst(item);
    public void AddLast(T item) => list.AddLast(item);
    public T RemoveFirst()
    {
        var val = list.First.Value;
        list.RemoveFirst();
        return val;
    }
    public int Count => list.Count;
}
}
