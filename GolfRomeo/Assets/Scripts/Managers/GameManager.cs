using System;

public class GameManager : Singleton<GameManager>
{
    private static State e_state;

    private new void Awake()
    {
        base.Awake();
        SetState(State.Menu);

        MapperConfiguration.Configure();
    }

    /// <summary>
    /// Returns current state
    /// </summary>
    /// <returns>State</returns>
    public static State GetState()
    {
        return e_state;
    }

    /// <summary>
    /// Set state
    /// </summary>
    /// <param name="state">state</param>
    public static void SetState(State state)
    {
        e_state = state;
    }

    /// <summary>
    /// Add state
    /// </summary>
    /// <param name="state">state</param>
    public static void AddState(State state)
    {
        e_state |= state;
    }

    /// <summary>
    /// Remove state
    /// </summary>
    /// <param name="state">state</param>
    public static void RemoveState(State state)
    {
        e_state &= ~state;

    }

    /// <summary>
    /// Check is state equal
    /// </summary>
    /// <param name="state">state</param>
    /// <returns>bool</returns>
    public static bool CheckState(State state)
    {
        if ((e_state & state) == state)
        {
            return true;
        }
        return false;
    }
}

[Flags]
public enum State
{
    SplashScreen = 1,
    Menu = 2,
    Game = 4,
    Edit = 8,
    Pause = 16,
}

