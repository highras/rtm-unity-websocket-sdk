# RTM Client Unity SDK Login & Logout API Docs

# Index

[TOC]

### Login

	//-- Async Method
	public bool Login(AuthDelegate callback, string token, int timeout = 0);
	public bool Login(AuthDelegate callback, string token, Dictionary<string, string> attr, TranslateLanguage language = TranslateLanguage.None, int timeout = 0);
	
User login.

Parameters:

+ `AuthDelegate callback`

		public delegate void AuthDelegate(long pid, long uid, bool authStatus, int errorCode);

	Callabck for async method. Please refer [AuthDelegate](Delegates.md#AuthDelegate).

+ `string token`

	Login/auth token, which can be gotten from your bueiness server or game server who quest for the current user from RTM Server-end endpoints.

+ `Dictionary<string, string> attr`

	Session or connection attributes. That can be fetch by all sessions of the user.

+ `TranslateLanguage language`

	The target language for enable auto-translating.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

**Notice:**

If returned value is zero (FPNN_EC_OK) and the output parameter ok is false in sync calling, or errorCode is zero (FPNN_EC_OK) and authStatus is false in async calling, means the token is invalid, which need to be fetched again from RTM server by business server-end.

### Bye

	public void Bye(bool async = true);

Logout and close the current session.

Parameters:

+ `bool async`

	In async way or sync way.

### Close

	public void Close(bool waitConnectingCannelled = true);

Without logout, close the current session directly.

Parameters:

+ `bool waitConnectingCannelled`

	Waiting for current connecting has been cannelled or not.

