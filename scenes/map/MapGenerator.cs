namespace DeckBuilder;

using Godot;
using Godot.Collections;

public partial class MapGenerator : Node
{

    public static readonly int X_DIST = 30;
    public static readonly int Y_DIST = 25;
    public static readonly int PLACEMENT_RANDOMNESS = 5;
    public static readonly int FLOORS = 15;
    public static readonly int MAP_WIDTH = 7;
    public static readonly int PATHS = 6;
    public static readonly float MONSTER_ROOM_WEIGHT = 10.0f;
    public static readonly float SHOP_ROOM_WEIGHT = 2.5f;
    public static readonly float CAMPFIRE_ROOM_WEIGHT = 4.0f;

    public Godot.Collections.Dictionary<Room.Type, float> randomRoomTypeWeights = new()
    {
        { Room.Type.MONSTER, 0.0f },
        { Room.Type.SHOP, 0.0f },
        { Room.Type.CAMPFIRE, 0.0f }
    };

    public float randomRoomTypeTotalWeight = 0.0f;
    public Array<Array<Room>> mapData = new();

    public Array<Array<Room>> GenerateMap()
    {
        mapData = GenerateInitialGrid();
        Array<int> startingPoints = GetRandomStartingPoints();

        foreach (int point in startingPoints)
        {
            int currentPoint = point;
            for(int f = 0; f < FLOORS - 1; f++)
            {
                currentPoint = SetupConnection(f, currentPoint);
            }
        }

        SetupBossRoom();
        SetupRandomRoomWeights();
        SetupRoomTypes();

        return mapData;
    }

    public void SetupBossRoom()
    {
        int middle = Mathf.FloorToInt(MAP_WIDTH * 0.5f);
        Room bossRoom = mapData[FLOORS - 1][middle];
        
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            Room currentRoom = mapData[FLOORS - 2][x];
            if (currentRoom.nextRooms.Count > 0)
            {
                currentRoom.nextRooms.Clear();
                currentRoom.nextRooms.Add(bossRoom);
            }
        }

        bossRoom.type = Room.Type.BOSS;
    }

    public void SetupRandomRoomWeights()
    {
        randomRoomTypeWeights[Room.Type.MONSTER] = MONSTER_ROOM_WEIGHT;
        randomRoomTypeWeights[Room.Type.CAMPFIRE] = MONSTER_ROOM_WEIGHT + CAMPFIRE_ROOM_WEIGHT;
        randomRoomTypeWeights[Room.Type.SHOP] = MONSTER_ROOM_WEIGHT + CAMPFIRE_ROOM_WEIGHT + SHOP_ROOM_WEIGHT;

        randomRoomTypeTotalWeight = randomRoomTypeWeights[Room.Type.SHOP];
    }

    public void SetupRoomTypes()
    {
        // First floor is always a battle
        foreach (Room room in mapData[0])
        {
            if (room.nextRooms.Count > 0) {
                room.type = Room.Type.MONSTER;
            }
        }

        // 9th floor is always a treasure
        foreach (Room room in mapData[8])
        {
            if (room.nextRooms.Count > 0) {
                room.type = Room.Type.TREASURE;
            }
        }

        // Last floor before the boss is always a campfire
        foreach (Room room in mapData[13])
        {
            if (room.nextRooms.Count > 0) {
                room.type = Room.Type.CAMPFIRE;
            }
        }

        foreach (Array<Room> floor in mapData)
        {
            foreach(Room room in floor)
            {
                foreach(Room nextRoom in room.nextRooms)
                {
                    if (nextRoom.type == Room.Type.NOT_ASSIGNED)
                    {
                        SetRoomRandomly(nextRoom);
                    }
                }
            }
        }
    }

    public void SetRoomRandomly(Room room)
    {
        bool campfireBelow4 = true;
        bool consecutiveCampfire = true;
        bool consecutiveShop = true;
        bool campfireOn13 = true;

        Room.Type typeCandidate = Room.Type.NOT_ASSIGNED;

        while (campfireBelow4 || consecutiveCampfire || consecutiveShop || campfireOn13)
        {
            typeCandidate = GetRandomRoomByWeight();

            bool isCampfire = typeCandidate == Room.Type.CAMPFIRE;
            bool hasCampfireParent = RoomHasParentOfType(room, Room.Type.CAMPFIRE);
            bool isShop = typeCandidate == Room.Type.SHOP;
            bool hasShopParent = RoomHasParentOfType(room, Room.Type.SHOP);

            campfireBelow4 = isCampfire && room.row < 3;
            consecutiveCampfire = isCampfire && hasCampfireParent;
            consecutiveShop = isShop && hasShopParent;
            campfireOn13 = isCampfire && room.row == 12;
        }

        room.type = typeCandidate;
    }

    public bool RoomHasParentOfType(Room room, Room.Type type)
    {
        Array<Room> parents = new();

        // Left parent
        if (room.column > 0 && room.row > 0)
        {
            Room parentCandidate = mapData[room.row - 1][room.column - 1];
            if (parentCandidate.nextRooms.Contains(room))
            {
                parents.Add(parentCandidate);
            }
        }

        // Below parent
        if (room.row > 0)
        {
            Room parentCandidate = mapData[room.row - 1][room.column];
            if (parentCandidate.nextRooms.Contains(room))
            {
                parents.Add(parentCandidate);
            }
        }

        // Right parent
        if (room.column < MAP_WIDTH - 1 && room.row > 0)
        {
            Room parentCandidate = mapData[room.row - 1][room.column + 1];
            if (parentCandidate.nextRooms.Contains(room))
            {
                parents.Add(parentCandidate);
            }
        }

        foreach (Room parent in parents)
        {
            if (parent.type == type)
            {
                return true;
            }
        }

        return false;
    }

    public Room.Type GetRandomRoomByWeight()
    {
        double roll = GD.RandRange(0.0f, randomRoomTypeTotalWeight);

        foreach (Room.Type type in randomRoomTypeWeights.Keys)
        {
            if (randomRoomTypeWeights[type] > roll)
            {
                return type;
            }
        }

        return Room.Type.MONSTER;
    }

    public int SetupConnection(int floor, int column)
    {
        Room nextRoom = null;
        Room currentRoom = mapData[floor][column];

        while (nextRoom == null || WouldCrossExistingPath(floor, column, nextRoom))
        {
            int randomColumn = Mathf.Clamp(GD.RandRange(column - 1, column + 1), 0, MAP_WIDTH - 1);
            nextRoom = mapData[floor + 1][randomColumn];
        }

        currentRoom.nextRooms.Add(nextRoom);

        return nextRoom.column;
    }

    public bool WouldCrossExistingPath(int floor, int column, Room room)
    {
        Room leftNeighbour = null;
        Room rightNeighbour = null;

        // if column is 0, there is no left neighbour
        if (column > 0)
        {
            leftNeighbour = mapData[floor][column - 1];
        }

        // if column is MAP_WIDTH - 1, there is no right neighbour
        if (column < MAP_WIDTH - 1)
        {
            rightNeighbour = mapData[floor][column + 1];
        }

        // Can't cross in right dir if right neighbour goes to the left
        if (rightNeighbour != null && room.column > column)
        {
            foreach (Room nextRoom in rightNeighbour.nextRooms)
            {
                if (nextRoom.column < column)
                {
                    return true;
                }
            }
        }

        // Can't cross in left dir if left neighbour goes to the right
        if (leftNeighbour != null && room.column < column)
        {
            foreach (Room nextRoom in leftNeighbour.nextRooms)
            {
                if (nextRoom.column > column)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static Array<int> GetRandomStartingPoints()
    {
        Array<int> yCoordinates = new();
        int uniquePoints = 0;

        while(uniquePoints < 2)
        {
            uniquePoints = 0;
            yCoordinates = new();

            for (int i = 0; i < PATHS; i++)
            {
                int startingPoint = GD.RandRange(0, MAP_WIDTH - 1);
                if (!yCoordinates.Contains(startingPoint))
                {
                    uniquePoints++;
                }

                yCoordinates.Add(startingPoint);
            }
        }

        return yCoordinates;
    }

    public static Array<Array<Room>> GenerateInitialGrid()
    {
        Array<Array<Room>> result = new();

        for (int y = 0; y < FLOORS; y++)
        {
            Array<Room> adjacentRooms = new();

            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Vector2 offset = new Vector2(GD.Randf(), GD.Randf()) * PLACEMENT_RANDOMNESS;
                Room currentRoom = new()
                {
                    position = new Vector2(x * X_DIST, y * -Y_DIST) + offset,
                    row = y,
                    column = x,
                    nextRooms = new Array<Room>()
                };

                if (y == (FLOORS - 1))
                {
                    currentRoom.position.Y = (y + 1) * -Y_DIST;
                }

                adjacentRooms.Add(currentRoom);
            }

            result.Add(adjacentRooms);
        }

        return result;
    }

}
