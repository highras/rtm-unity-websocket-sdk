# RTM Client Unity SDK Users API Docs

# Index

[TOC]

### Get Online Users

	public bool GetOnlineUsers(Action<HashSet<long>, int> callback, HashSet<long> uids, int timeout = 0);

Get online users.

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is the online users' ids;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<long> uids`

	The users' ids which want to be checked.

	Max 200 uids for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Set User Info

	public bool SetUserInfo(DoneDelegate callback, string publicInfo = null, string privateInfo = null, int timeout = 0);
	
Set user's public info and private info.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `string publicInfo`

	New public info for group. `null` means don't change the public info. Max length is 65535 bytes.

+ `string privateInfo`

	New private info for group. `null` means don't change the private info. Max length is 65535 bytes.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get User Info

	public bool GetUserInfo(Action<string, string, int> callback, int timeout = 0);
	
Get user's public info and private info.

Parameters:

+ `Action<string, string, int> callback`

	Callabck for async method.  
	First `string` is gotten public info of current user;  
	Second `string` is gotten private info of current user;  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Users Public Infos

	public bool GetUserPublicInfo(Action<Dictionary<long, string>, int> callback, HashSet<long> uids, int timeout = 0);
	
Get users' public infos.

Parameters:

+ `Action<Dictionary<long, string>, int> callback`

	Callabck for async method.  
	First `Dictionary<long, string>` is gotten users' public infos. Key is uid, value is the public info;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

