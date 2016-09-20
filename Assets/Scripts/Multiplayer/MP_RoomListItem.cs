using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class MP_RoomListItem : MonoBehaviour
{
	// Want to join match - join room delegate
    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;

	// Match description
    private MatchInfoSnapshot match;

	// Setup game
	public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallBack)
	{
        match = _match;
        joinRoomCallback = _joinRoomCallBack;

        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }

	public void JoinRoom()
	{
		// Link to delegate
		joinRoomCallback.Invoke(match);
	}
}
