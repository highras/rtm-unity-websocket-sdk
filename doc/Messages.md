# RTM Client Unity SDK Messages API Docs

# Index

[TOC]

### Send P2P Message

	public bool SendMessage(MessageIdDelegate callback, long uid, byte mtype, string message, string attrs = "", int timeout = 0);
	public bool SendMessage(MessageIdDelegate callback, long uid, byte mtype, byte[] message, string attrs = "", int timeout = 0);
	
Send P2P message.

Parameters:

+ `MessageIdDelegate callback`

		public delegate void MessageIdDelegate(long messageId, int errorCode);

	Callabck for async method. Please refer [MessageIdDelegate](Delegates.md#MessageIdDelegate).

+ `long uid`

	Receiver user id.

+ `byte mtype`

	Message type for message. MUST large than 50.

+ `string message`

	Text message.

+ `byte[] message`

	Binary message.

+ `string attrs`

	Message attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Send Group Messsage

	public bool SendGroupMessage(MessageIdDelegate callback, long groupId, byte mtype, string message, string attrs = "", int timeout = 0);
	public bool SendGroupMessage(MessageIdDelegate callback, long groupId, byte mtype, byte[] message, string attrs = "", int timeout = 0);
	
Send message in group.

Parameters:

+ `MessageIdDelegate callback`

		public delegate void MessageIdDelegate(long messageId, int errorCode);

	Callabck for async method. Please refer [MessageIdDelegate](Delegates.md#MessageIdDelegate).

+ `long groupId`

	Group id.

+ `byte mtype`

	Message type for message. MUST large than 50.

+ `string message`

	Text message.

+ `byte[] message`

	Binary message.

+ `string attrs`

	Message attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Send Room Message

	public bool SendRoomMessage(MessageIdDelegate callback, long roomId, byte mtype, string message, string attrs = "", int timeout = 0);
	public bool SendRoomMessage(MessageIdDelegate callback, long roomId, byte mtype, byte[] message, string attrs = "", int timeout = 0);
	
Send message in room.

Parameters:

+ `MessageIdDelegate callback`

		public delegate void MessageIdDelegate(long messageId, int errorCode);

	Callabck for async method. Please refer [MessageIdDelegate](Delegates.md#MessageIdDelegate).

+ `long roomId`

	Room id.

+ `byte mtype`

	Message type for message. MUST large than 50.

+ `string message`

	Text message.

+ `byte[] message`

	Binary message.

+ `string attrs`

	Message attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Get P2P Message

	public bool GetP2PMessage(HistoryMessageDelegate callback, long peerUid, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for P2P message.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `long peerUid`

	Peer user id.

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `long lastId`

	Last data id returned when last calling. First calling using 0.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Group Messsage

	public bool GetGroupMessage(HistoryMessageDelegate callback, long groupId, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for group message.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `long groupId`

	Group id.

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `long lastId`

	Last data id returned when last calling. First calling using 0.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Room Message

	public bool GetRoomMessage(HistoryMessageDelegate callback, long roomId, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for room message.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `long roomId`

	Room id.

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `long lastId`

	Last data id returned when last calling. First calling using 0.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Broadcast Message

	public bool GetBroadcastMessage(HistoryMessageDelegate callback, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for broadcast message.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `long lastId`

	Last data id returned when last calling. First calling using 0.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Delete Message

	public bool DeleteMessage(DoneDelegate callback, long fromUid, long toId, long messageId, MessageCategory messageCategory, int timeout = 0);
	
Delete a sent message.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `fromUid`

	Uid of the message sender, which message is wanted to be deleted.

+ `toId`

	If the message is P2P message, `toId` means the uid of peer;  
	If the message is group message, `toId` means the `groupId`;  
	If the message is room message, `toId` means the `roomId`.

+ `messageId`

	Message id for the message which wanted to be deleted.

+ `messageCategory`

	MessageCategory enumeration.

	Can be MessageCategory.P2PMessage, MessageCategory.GroupMessage, MessageCategory.RoomMessage.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Message

	public bool GetMessage(Action<RetrievedMessage, int> callback, long fromUid, long toId, long messageId, MessageCategory messageCategory, int timeout = 0);
	
Retrieve a sent message.

Parameters:

+ `Action<RetrievedMessage, int> callback`

	Callabck for async method.  
	First `RetrievedMessage` is retrieved data, please refer [RetrievedMessage](Structures.md#RetrievedMessage);  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `fromUid`

	Uid of the message sender, which message is wanted to be retrieved.

+ `toId`

	If the message is P2P message, `toId` means the uid of peer;  
	If the message is group message, `toId` means the `groupId`;  
	If the message is room message, `toId` means the `roomId`;  
	If the message is broadcast message, `toId` is `0`.

+ `messageId`

	Message id for the message which wanted to be retrieved.

+ `messageCategory`

	MessageCategory enumeration.

	Can be MessageCategory.P2PMessage, MessageCategory.GroupMessage, MessageCategory.RoomMessage, MessageCategory.BroadcastMessage.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get P2P Message By Message ID

	public bool GetP2PMessageByMessageId(HistoryMessageDelegate callback, long peerUid, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for P2P message by message id.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `long peerUid`

	Peer user id.

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long messageId`

	Message id.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Group Messsage By Message ID

	public bool GetGroupMessageByMessageId(HistoryMessageDelegate callback, long groupId, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for group message by message id.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `long groupId`

	Group id.

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long messageId`

	Message id.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Room Message By Message ID

	public bool GetRoomMessageByMessageId(HistoryMessageDelegate callback, long roomId, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for room message by message id.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `long roomId`

	Room id.

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long messageId`

	Message id.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Broadcast Message By Message ID

	public bool GetBroadcastMessage(HistoryMessageDelegate callback, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, List<byte> mtypes = null, int timeout = 0);
	
Get history data for broadcast message by message id.

Parameters:

+ `HistoryMessageDelegate callback`

		public delegate void HistoryMessageDelegate(int count, long lastId, long beginMsec, long endMsec, List<HistoryMessage> messages, int errorCode);

	Callabck for async method. Please refer [HistoryMessageDelegate](Delegates.md#HistoryMessageDelegate).

+ `bool desc`

	* true: desc order;
	* false: asc order.

+ `int count`

	Count for retrieving. Max is 20 for each calling.

+ `long messageId`

	Message id.

+ `long beginMsec`

	Beginning timestamp in milliseconds.

+ `long endMsec`

	Ending timestamp in milliseconds.

+ `List<byte> mtypes`

	Message types for retrieved message. `null` means all types.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

