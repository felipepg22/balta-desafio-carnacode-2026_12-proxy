namespace DesignPatternChallenge;

public class User
{
    public string Username { get; set; }
    public int ClearanceLevel { get; set; }

    public User(string username, int clearanceLevel)
    {
        Username = username;
        ClearanceLevel = clearanceLevel;
    }
}
