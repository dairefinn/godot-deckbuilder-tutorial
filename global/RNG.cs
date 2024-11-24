namespace DeckBuilder;

using Godot;
using Godot.Collections;


public partial class RNG : Node
{

	public static RandomNumberGenerator Instance { get; private set; }

	public override void _Ready()
	{
		Initialize();
	}

	public static void Initialize()
	{
		Instance = new RandomNumberGenerator();
		Instance.Randomize();
	}

	public static void SetFromSaveData(ulong whichSeed, ulong state)
	{
        Instance = new RandomNumberGenerator
        {
            Seed = whichSeed,
            State = state
        };
    }

	public static T ArrayPickRandom<[MustBeVariant] T>(Array<T> array) where T : class
	{
		return array[(int)(Instance.Randi() % (ulong)array.Count)] as T;
	}

	public static void ArrayShuffle<[MustBeVariant] T>(Array<T> array)
	{
		if (array.Count < 2)
		{
			return;
		}

		for (int i = array.Count - 1; i > 0; i--)
		{
			int j = (int)(Instance.Randi() % (ulong)(i + 1));
			T tmp = array[j];
			array[j] = array[i];
			array[i] = tmp;
		}
    }

}

