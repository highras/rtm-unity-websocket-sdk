# RTM Client Unity SDK Files API Docs

# Index

[TOC]

### Send P2P File

	public bool SendFile(MessageIdDelegate callback, long peerUid, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120);
	
Send P2P file.

Parameters:

+ `MessageIdDelegate callback`

		public delegate void MessageIdDelegate(long messageId, int errorCode);

	Callabck for async method. Please refer [MessageIdDelegate](Delegates.md#MessageIdDelegate).

+ `long peerUid`

	Receiver user id.

+ `MessageType type`

	Message type for file.

+ `byte[] fileContent`

	File content.

+ `string filename`

	File name.

+ `string fileExtension`

	File extension.

+ `string attrs`

	Text file attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Send Group File

	public bool SendGroupFile(MessageIdDelegate callback, long groupId, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120);
	
Send file in group.

Parameters:

+ `MessageIdDelegate callback`

		public delegate void MessageIdDelegate(long messageId, int errorCode);

	Callabck for async method. Please refer [MessageIdDelegate](Delegates.md#MessageIdDelegate).

+ `long groupId`

	Group id.

+ `MessageType type`

	Message type for file.

+ `byte[] fileContent`

	File content.

+ `string filename`

	File name.

+ `string fileExtension`

	File extension.

+ `string attrs`

	Text file attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Send Room File

	public bool SendRoomFile(MessageIdDelegate callback, long roomId, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120);
	
Send file in room.

Parameters:

+ `MessageIdDelegate callback`

		public delegate void MessageIdDelegate(long messageId, int errorCode);

	Callabck for async method. Please refer [MessageIdDelegate](Delegates.md#MessageIdDelegate).

+ `long roomId`

	Room id.

+ `MessageType type`

	Message type for file.

+ `byte[] fileContent`

	File content.

+ `string filename`

	File name.

+ `string fileExtension`

	File extension.

+ `string attrs`

	Text file attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.


### Upload File

	public bool UploadFile(Action<string, uint, int> callback, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120);
	
Upload file.

Parameters:

+ `Action<string, uint, int> callback`

	Callabck for async method.  
	`string` is url of the uploaded file.  
	`uint` is size of the uploaded file.
	`int` is the error code indicating the calling is successful or the failed reasons.
		
+ `MessageType type`

	Message type for file.

+ `byte[] fileContent`

	File content.

+ `string filename`

	File name.

+ `string fileExtension`

	File extension.

+ `string attrs`

	Text file attributes in Json.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async sending is start.
+ false: Start async sending is failed.

