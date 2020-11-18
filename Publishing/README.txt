Instructions to create a new PlayStore ready APK.

Requirements:
1. Correct Identification
2. Signed Keystore

Identification

1. File -> Build Settings -> Player Settings -> Other Settings

2. Bundle Identifier : com.appsoplever.trivia    [com.companyname.gamename]

3. Version : 1.0.0    [PlayStore does not allow versions below 1.0.0]

4. Bundle Version Code : 20170605    [Date of release in YYYYMMDD format]


Signed Keystore

	* 100% neccessary and non-replacable, if lost or corrupted, that game cannot be updated in the PlayStore.
	
	* A copy can be found in the Gitlab Repository.
	
	filename: trivia.keystore
	
Credentials:

CVBhc3N3b3JkOiBPcGxldmVyMjAyMAoJCglBbGlhczogb3BsZSB2ZXJhY3J1egoJCglQYXNzd29yZDogT3BsZXZlcjIwMjAKCQoJT3JnYW5pemF0aW9uOiBPUExFIFZlcmFjcnV6