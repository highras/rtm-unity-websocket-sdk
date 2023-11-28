# RTM Client Unity SDK System API Docs

# Index

[TOC]

### Add Attributes

	public bool AddAttributes(DoneDelegate callback, Dictionary<string, string> attrs, int timeout = 0);
	
Add session or connection attributes. That can be fetch by all sessions of the user.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `Dictionary<string, string> attrs`

	Session or connection attributes. That can be fetch by all sessions of the user.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Attributes

	public bool GetAttributes(Action<Dictionary<string, string>, int> callback, int timeout = 0);
	
Get session or connection attributes.

Parameters:

+ `Action<Dictionary<string, string>, int> callback`

	Callabck for async method.  
	First `Dictionary<string, string>` is the attributes dictionary for current session;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

### Add Debug Log

	public bool AddDebugLog(DoneDelegate callback, string message, string attrs, int timeout = 0);
	
Add debug log.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `string message`

	log text.

+ `string attrs`

	Attributes for log text.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Add Device

	public bool AddDevice(DoneDelegate callback, string appType, string deviceToken, int timeout = 0);
	
Add device infos.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `string appType`

	Application information. apns or fcm

+ `string deviceToke`

	Device Token

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Remove Device

	public bool RemoveDevice(DoneDelegate callback, string deviceToken, int timeout = 0);
	
Remove device infos.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `string deviceToke`

	Device Token

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Add Device Push Option

	public bool AddDevicePushOption(DoneDelegate callback, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null, int timeout = 0);
	
Set disabled session for chat & messages push.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `MessageCategory messageCategory`

	Only `MessageCategory.P2PMessage` & `MessageCategory.GroupMessage` can be used.

+ `long targetId`

	Peer uid for `MessageCategory.P2PMessage` and group id for `MessageCategory.GroupMessage`.

+ `HashSet<byte> mTypes`

	Disabled message types. If `mTypes` is `null` or empty, means all message types are disalbed for push.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Remove Device Push Option

	public bool RemoveDevicePushOption(DoneDelegate callback, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null, int timeout = 0);
	
Remove disabled option for chat & messages push.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `MessageCategory messageCategory`

	Only `MessageCategory.P2PMessage` & `MessageCategory.GroupMessage` can be used.

+ `long targetId`

	Peer uid for `MessageCategory.P2PMessage` and group id for `MessageCategory.GroupMessage`.

+ `HashSet<byte> mTypes`

	Disabled message types. If `mTypes` is `null` or empty, means all message types are removed disalbe attributes for push.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Device Push Option

	public bool GetDevicePushOption(Action<Dictionary<long, HashSet<byte>>, Dictionary<long, HashSet<byte>>, int> callback, int timeout = 0);
	
Get disabled option for chat & messages push.

Parameters:

+ `Action<Dictionary<long, HashSet<byte>>, Dictionary<long, HashSet<byte>>, int> callback`

	Callabck for async method.  
	First `Dictionary<long, HashSet<byte>>` is peer user id with associated disabled message types set for P2P sessions;  
	Second `Dictionary<long, HashSet<byte>>` is group id with associated disabled message types set for group sessions;  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

	