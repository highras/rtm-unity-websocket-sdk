# RTM Client Unity SDK Friends API Docs

# Index

[TOC]

### Add Friends

	public bool AddFriends(DoneDelegate callback, HashSet<long> uids, int timeout = 0);

Add friends.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `HashSet<long> uids`

	Friends' uids set. Max 100 users for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Delete Friends

	public bool DeleteFriends(DoneDelegate callback, HashSet<long> uids, int timeout = 0);

Delete friends.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `HashSet<long> uids`

	Friends' uids set. Max 100 users for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Friends

	public bool GetFriends(Action<HashSet<long>, int> callback, int timeout = 0);

Get friends.

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is gotten friends' uids;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Add Blacklist

	public bool AddBlacklist(DoneDelegate callback, HashSet<long> uids, int timeout = 0);

Add users to blacklist.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `HashSet<long> uids`

	Uids set. Max 100 users for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Delete Blacklist

	public bool DeleteBlacklist(DoneDelegate callback, HashSet<long> uids, int timeout = 0);

Delete from blacklist.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `HashSet<long> uids`

	Uids set. Max 100 users for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Blacklist

	public bool GetBlacklist(Action<HashSet<long>, int> callback, int timeout = 0);

Get blocked uids from blacklist.

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is gotten uids;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

