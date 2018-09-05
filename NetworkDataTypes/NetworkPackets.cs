
namespace NetworkDataTypes
{
    //Sent form the server
    // Client has to listen for these
    public enum ServerPackets
    {
        SConnectionOK = 1,
        SUsernameResponse = 2,
        SGameStarting = 3,
        SHand = 4,
        SPossibleOptions = 5,
        SChosenOptions = 6,
        SResolution = 7,
        SRoundResult = 8,
        SGameEnd = 9,
    }

    //Sent form the clients
    // Server has to listen for these
    public enum ClientPackets
    {
        CRequestUsername = 1,
        CNewGame = 2,
        CChoice = 3,
    }
}