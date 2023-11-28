# RTM Client Unity SDK Conversation API Docs

# Index

[TOC]

### Get P2P Conversation List

	public bool GetP2PConversationList(Action<List<Conversation>, int> callback, HashSet<byte> mTypes = null, long startTime = 0, int timeout = 0);

Get P2P conversation list.

Parameters:

+ `Action<List<Conversation>, int> callback`

	Callabck for async method.  
	First `List<Conversation>` is conversation data, please refer [Conversation](Structures.md#Conversation);  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<byte> mTypes`

	Which message types will be checked. If set is null or empty, only chat messages, cmd messages and file messages will be checked.

+ `long startTime`

	The timestamp in millisecond which indicated the start time to calculate the unread messages. `0` means using the last offline/logout time. 

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Get P2P Unread Conversation List

	public bool GetP2PUnreadConversationList(Action<List<Conversation>, int> callback, HashSet<byte> mTypes = null, long startTime = 0, int timeout = 0);

Get P2P unread conversation list.

Parameters:

+ `Action<List<Conversation>, int> callback`

	Callabck for async method.  
	First `List<Conversation>` is conversation data, please refer [Conversation](Structures.md#Conversation);  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<byte> mTypes`

	Which message types will be checked. If set is null or empty, only chat messages, cmd messages and file messages will be checked.

+ `long startTime`

	The timestamp in millisecond which indicated the start time to calculate the unread messages. `0` means using the last offline/logout time. 

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Get Group Conversation List

	public bool GetGroupConversationList(Action<List<Conversation>, int> callback, HashSet<byte> mTypes = null, long startTime = 0, int timeout = 0);

Get group conversation list.

Parameters:

+ `Action<List<Conversation>, int> callback`

	Callabck for async method.  
	First `List<Conversation>` is conversation data, please refer [Conversation](Structures.md#Conversation);  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<byte> mTypes`

	Which message types will be checked. If set is null or empty, only chat messages, cmd messages and file messages will be checked.

+ `long startTime`

	The timestamp in millisecond which indicated the start time to calculate the unread messages. `0` means using the last offline/logout time. 

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Get Group Unread Conversation List

	public bool GetGroupUnreadConversationList(Action<List<Conversation>, int> callback, HashSet<byte> mTypes = null, long startTime = 0, int timeout = 0);

Get group unread conversation list.

Parameters:

+ `Action<List<Conversation>, int> callback`

	Callabck for async method.  
	First `List<Conversation>` is conversation data, please refer [Conversation](Structures.md#Conversation);  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<byte> mTypes`

	Which message types will be checked. If set is null or empty, only chat messages, cmd messages and file messages will be checked.

+ `long startTime`

	The timestamp in millisecond which indicated the start time to calculate the unread messages. `0` means using the last offline/logout time. 

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Get Unread Conversation List

	public bool GetUnreadConversationList(Action<List<Conversation>, List<Conversation>, int> callback, bool clear = true, HashSet<byte> mTypes = null, long startTime = 0, int timeout = 0);

Get unread conversation list.

Parameters:

+ `Action<List<Conversation>, int> callback`

	Callabck for async method.  
	First `List<Conversation>` is group conversation data, please refer [Conversation](Structures.md#Conversation);  
	Second `List<Conversation>` is p2p conversation data, please refer [Conversation](Structures.md#Conversation);  
	Third `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<byte> mTypes`

	Which message types will be checked. If set is null or empty, only chat messages, cmd messages and file messages will be checked.

+ `long startTime`

	The timestamp in millisecond which indicated the start time to calculate the unread messages. `0` means using the last offline/logout time. 

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.

