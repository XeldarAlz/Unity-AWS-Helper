# Unity AWS Helper
I couldn't find a starting point or a working guide how to implement AWS into Unity. All official repositories are out dated. So I decided to create my own. Feel free to contribute. This repository show examples how to use popular web services such as Cognito or S3 in Unity.

# About repository
A helper repository for basic AWS setup on unity. In order to use this repository, you need to change values inside of the src/UnityAWSHelper/Assets/Scripts/Managers/AwsSdkManager.cs with your own credentials. Use AWS console and use the related services such as Cognito. Then copy your configs into the scripts to related values. 

Currently the confirmation for the new users is happening with the confirmation code sent by email. Newly registered users needs to input the confirmation code sent by email and confirm their accounts by themselves. This behaviour can be change from AWS Cognito Console.

# This project contains: 

* Sign up
* Confirm User
* Login
* Auto login if logged in before
* Delete User
* Recover - Forgot Password
* Confirm Forgot Password
* Google Play Sign In
* Unity deeplink connection
* Also have the integrated Cognito Hosted Ui
* Logout
* S3 integration & Upload files. Currently implemented a native gallery to choose a picture then upload it
* Lambda integration & Invoke functions
* DynamoDB integration & Upload data to data base tables
* Api gateway integration & Example usage api to work with lambda
