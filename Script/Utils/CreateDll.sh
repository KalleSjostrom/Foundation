#!/bin/sh
mv Log.txt Log.cs
mv DebugScreenText.txt DebugScreenText.cs
mcs -r:/Applications/Unity/Unity.app/Contents/Frameworks/Managed/UnityEngine.dll -target:library Log.cs DebugScreenText.cs
mv DebugScreenText.cs DebugScreenText.txt
mv Log.cs Log.txt
