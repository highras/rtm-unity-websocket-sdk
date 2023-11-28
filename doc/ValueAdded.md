# RTM Client Unity SDK Value-Added API Docs

# Index

[TOC]

### Set Translated Language

	public bool SetTranslatedLanguage(DoneDelegate callback, TranslateLanguage targetLanguage, int timeout = 0);
	
Set target language to enable auto-translating.

Parameters:

+ `DoneDelegate callback`

		public delegate void DoneDelegate(int errorCode);

	Callabck for async method. Please refer [DoneDelegate](Delegates.md#DoneDelegate).

+ `TranslateLanguage targetLanguage`

	Target language enum.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### Translate

    public bool Translate(Action<TranslatedInfo, int> callback, string text,
            string destinationLanguage, string sourceLanguage = "",
            TranslateType type = TranslateType.Chat, ProfanityType profanity = ProfanityType.Off,
            int timeout = 0);
	
Translate text to target language.

Parameters:

+ `Action<TranslatedInfo>, int> callback`

	Callabck for async method.  
	First `TranslatedInfo` is translation message result, please refer [TranslatedInfo](Structures.md#TranslatedInfo);  
	Second `int` is the error code indicating the calling is successful or the failed reasons.

+ `string text`

	The text need to be translated.

+ `string destinationLanguage`

	Target language. Please refer the 'Language support' section in [document](https://docs.ilivedata.com/stt/production/) for language value.

+ `string sourceLanguage`

	Source language. Empty string means automatic recognition. Please refer the 'Language support' section in [document](https://docs.ilivedata.com/stt/production/) for language value.

+ `TranslateType type`

	TranslateType.Chat or TranslateType.Mail. Default is TranslateType.Chat.

+ `ProfanityType profanity`

	Profanity filter action.

	* ProfanityType.Off (**Default**)
	* ProfanityType.Stop
	* ProfanityType.Censor

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### SpeechToText

	public bool SpeechToText(Action<string, string, int> callback, byte[] audioBinaryContent, string language, string codec = null, int sampleRate = 0, int timeout = 120);
	
Speech Recognition, convert speech to text.

Parameters:

+ `Action<string, string, int> callback`

	Callabck for async method.  
	First `string` is the text converted from recognized speech;  
	Second `string` is the recognized language.  
	Thrid `int` is the error code indicating the calling is successful or the failed reasons.

+ `byte[] audioBinaryContent`

	Speech binary data.

+ `language`

	Speech language when recording. Available language please refer the documents in [https://www.ilivedata.com/](https://docs.ilivedata.com/stt/production/).

	[Current Chinese document](https://docs.ilivedata.com/stt/production/)

+ `codec`

	Codec for speech binary. If codec is `null` means `AMR_WB`.

+ `sampleRate`

	Sample rate for speech binary. If `0` means 16000.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### TextCheck

	public bool TextCheck(Action<TextCheckResult, int> callback, string text, string strategyId = null, int timeout = 120);
	
Text moderation.

Parameters:

+ `Action<TextCheckResult, int> callback`

	Callabck for async method.  
	First `TextCheckResult` is the result for text moderation;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.  
	`TextCheckResult` can be refered [TextCheckResult](Structures.md#TextCheckResult).

+ `string text`

	The text need to be audited.

+ `string strategeId`

	The strategy ID of text check.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### ImageCheck

	public bool ImageCheck(Action<CheckResult, int> callback, string imageUrl, string strategyId = null, int timeout = 120);
	public bool ImageCheck(Action<CheckResult, int> callback, byte[] imageContent, string strategyId = null, int timeout = 120);
	
Image review.

Parameters:

+ `Action<CheckResult, int> callback`

	Callabck for async method.  
	First `CheckResult` is the result for image review;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.  
	`CheckResult` can be refered [CheckResult](Structures.md#CheckResult).

+ `string imageUrl`

	Image's http/https url for auditing.

+ `byte[] imageContent`

	Image binary data for auditing.

+ `string strategeId`

	The strategy ID of image check.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### AudioCheck

	public bool AudioCheck(Action<CheckResult, int> callback, string audioUrl, string language, string codec = null, int sampleRate = 0, string strategyId = null, int timeout = 120);
	public bool AudioCheck(Action<CheckResult, int> callback, byte[] audioContent, string language, string codec = null, int sampleRate = 0, string strategyId = null, int timeout = 120);
	
Audio check.

Parameters:

+ `Action<CheckResult, int> callback`

	Callabck for async method.  
	First `CheckResult` is the result for audio checking;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.  
	`CheckResult` can be refered [CheckResult](Structures.md#CheckResult).

+ `string audioUrl`

	Http/https url for speech binary to be checking.

+ `byte[] audioContent`

	Audio binary data for checking.

+ `string strategeId`

	The strategy ID of audio check.

+ `language`

	Audio language when recording. Available language please refer the documents in [https://www.ilivedata.com/](https://docs.ilivedata.com/stt/production/).

	[Current Chinese document](https://docs.ilivedata.com/audiocheck/techdoc/submit/)  
	[Current Chinese document (live audio)](https://docs.ilivedata.com/audiocheck/livetechdoc/livesubmit/)

+ `codec`

	Codec for audio content. If codec is `null` means `AMR_WB`.

+ `sampleRate`

	Sample rate for audio content. If `0` means 16000.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.


### VideoCheck

	public bool VideoCheck(Action<CheckResult, int> callback, string videoUrl, string videoName, string strategyId = null, int timeout = 120);
	public bool VideoCheck(Action<CheckResult, int> callback, byte[] videoContent, string videoName, string strategyId = null, int timeout = 120);
	
Video review.

Parameters:

+ `Action<CheckResult, int> callback`

	Callabck for async method.  
	First `CheckResult` is the result for video review;  
	Second `int` is the error code indicating the calling is successful or the failed reasons.  
	`CheckResult` can be refered [CheckResult](Structures.md#CheckResult).

+ `string videoUrl`

	Video's http/https url for auditing.

+ `byte[] videoContent`

	Video binary data for auditing.

+ `string videoName`

	Video name.

+ `string strategeId`

	The strategy ID of video check.

+ `int timeout`

	Timeout in second.

	0 means using default setting.


Return Values:

+ true: Async calling is start.
+ false: Start async calling is failed.

