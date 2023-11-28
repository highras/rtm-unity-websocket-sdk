# RTM Client Unity SDK Rooms API Docs

# Index

[TOC]

### Enter Room

	public bool EnterRoom(DoneDelegate callback, long roomId, int timeout = 0);

Enter room.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `long roomId`

	Room id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Leave Room

	public bool LeaveRoom(DoneDelegate callback, long roomId, int timeout = 0);

Leave room.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `long roomId`

	Room id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get User Rooms

	public bool GetUserRooms(Action<HashSet<long>, int> callback, int timeout = 0);

Get current user's all groups.

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is gotten current user's room ids;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Set Room Info

	public bool SetRoomInfo(DoneDelegate callback, long roomId, string publicInfo = null, string privateInfo = null, int timeout = 0);
	
Set room public info and private info. Note: Current user MUST in the room.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `long roomId`

	Room id.

+ `string publicInfo`

	New public info for room. `null` means don't change the public info. Max length is 65535 bytes.

+ `string privateInfo`

	New private info for room. `null` means don't change the private info. Max length is 65535 bytes.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Room Info

	public bool GetRoomInfo(Action<string, string, int> callback, long roomId, int timeout = 0);
	
Get room public info and private info. Note: Current user MUST in the room.

Parameters:

+ `Action<string, string, int> callback`

	Callabck for async method.  
	First `string` is gotten public info of this room;  
	Second `string` is gotten private info of this room;  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `long roomId`

	Room id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Room Public Info

	public bool GetRoomPublicInfo(Action<string, int> callback, long roomId, int timeout = 0);
	
Get Room public info.

Parameters:

+ `Action<string, int> callback`

	Callabck for async method.  
	First `string` is gotten public info of the room;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `long roomId`

	Room id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Rooms Public Infos

	public bool GetRoomsPublicInfo(Action<Dictionary<long, string>, int> callback, HashSet<long> roomIds, int timeout = 0);
	
Get rooms' public infos.

Parameters:

+ `Action<Dictionary<long, string>, int> callback`

	Callabck for async method.  
	First `Dictionary<long, string>` is gotten rooms' public infos. Key is room id, value is the public info;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<long> roomIds`

	Rooms' ids. Max 100 rooms for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Room Members

	public bool GetRoomMembers(Action<HashSet<long>, int> callback, long roomId, int timeout = 0);
	
Get room member list.

Parameters:

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is the member list of indicated room;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `long roomId`

	Id of room which member list is wanted to be gotten.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Room Member Count

	public bool GetRoomMemberCount(Action<Dictionary<long, int>, int> callback, HashSet<long> roomIds, int timeout = 0);
	
Get rooms' member counts.

Parameters:

+ `Action<Dictionary<long, int>, int> callback`

	Callabck for async method.  
	First `Dictionary<long, int>` is the directionary for room ids and member counts. Key is room id, value is member count of this room;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<long> roomIds`

	Ids of rooms which member counts are wanted to be gotten.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

