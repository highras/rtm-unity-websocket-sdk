# RTM Client Unity SDK Groups API Docs

# Index

[TOC]

### Add Group Members

	public bool AddGroupMembers(DoneDelegate callback, long groupId, HashSet<long> uids, int timeout = 0);

Add group members. Note: Current user MUST be the group member.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `long groupId`

	Group id.

+ `HashSet<long> uids`

	New members' uids set. Max 100 users for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Delete Group Members

	public bool DeleteGroupMembers(DoneDelegate callback, long groupId, HashSet<long> uids, int timeout = 0);

Delete group members. Note: Current user MUST be the group member.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `long groupId`

	Group id.

+ `HashSet<long> uids`

	The members' uids set. Max 100 users for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Group Members

	public bool GetGroupMembers(Action<HashSet<long>, int> callback, long groupId, int timeout = 0);
	public bool GetGroupMembers(Action<HashSet<long>, HashSet<long>, int> callback, long groupId, int timeout = 0);

Get group members.

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is gotten group members' uids;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `Action<HashSet<long>, HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is gotten group members' uids;  
	Second `HashSet<long>` is gotten group online members' uids;  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `long groupId`

	Group id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Group Count

	public bool GetGroupCount(Action<int, int> callback, long groupId, int timeout = 0);
	public bool GetGroupCount(Action<int, int, int> callback, long groupId, int timeout = 0);

Get group member count.

+ `Action<int, int> callback`

	Callabck for async method.  
	First `int` is group member count;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `Action<int, int, int> callback`

	Callabck for async method.  
	First `int` is group member count;  
	Second `int` is group online member count;  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `long groupId`

	Group id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get User Groups

	public bool GetUserGroups(Action<HashSet<long>, int> callback, int timeout = 0);

Get current user's all groups.

+ `Action<HashSet<long>, int> callback`

	Callabck for async method.  
	First `HashSet<long>` is gotten current user's group ids;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

### Set Group Info

	public bool SetGroupInfo(DoneDelegate callback, long groupId, string publicInfo = null, string privateInfo = null, int timeout = 0);
	
Set group public info and private info. Note: Current user MUST be the group member.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `long groupId`

	Group id.

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


### Get Group Info

	public bool GetGroupInfo(Action<string, string, int> callback, long groupId, int timeout = 0);
	
Get group public info and private info. Note: Current user MUST be the group member.

Parameters:

+ `Action<string, string, int> callback`

	Callabck for async method.  
	First `string` is gotten public info of this group;  
	Second `string` is gotten private info of this group;  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `long groupId`

	Group id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Group Public Info

	public bool GetGroupPublicInfo(Action<string, int> callback, long groupId, int timeout = 0);
	
Get group public info.

Parameters:

+ `Action<string, int> callback`

	Callabck for async method.  
	First `string` is gotten public info of the group;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `long groupId`

	Group id.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Get Groups Public Infos

	public bool GetGroupsPublicInfo(Action<Dictionary<long, string>, int> callback, HashSet<long> groupIds, int timeout = 0);

Get groups' public infos.

Parameters:

+ `Action<Dictionary<long, string>, int> callback`

	Callabck for async method.  
	First `Dictionary<long, string>` is gotten groups' public infos. Key is group id, value is the public info;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `HashSet<long> groupIds`

	Groups' ids. Max 100 groups for each calling.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

