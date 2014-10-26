#!/bin/sh
mv Log.txt Log.cs
mcs -r:/Applications/Unity/Unity.app/Contents/Frameworks/Managed/UnityEngine.dll -target:library Log.cs
mv Log.cs Log.txt
