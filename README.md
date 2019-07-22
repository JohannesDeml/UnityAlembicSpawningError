# Alembic Spawning Problem

Instantiating an alembic during runtime can't find the alembic in the build. Works in the Editor and an Alembic as a simple GameObject in the Scene works as well. Only alembics that are added afterwards to the scene won't be bundled in the Build.

## Reproduce

Build the project for Windows Standalone as a development build. Run the project. Alternatively you can also download the build in the release section

### Expected Result
Two cubes are visible

### Actual Result
The Alembic cube that is set in the scene is visible, the cube that should be instantiated, is not visible since the file can't be found in the streaming assets  
Error:  
```
failed to load alembic at C:/Data/Documents/Unity/BugReports/AlembicSpawning/Builds/AlembicSpawningProblem/AlembicSpawningProblem_Data/StreamingAssets\Assets/_Project/Meshes/Cube.abc
UnityEngine.DebugLogHandler:Internal_Log(LogType, LogOption, String, Object)
UnityEngine.DebugLogHandler:LogFormat(LogType, Object, String, Object[])
UnityEngine.Logger:Log(LogType, Object)
UnityEngine.Debug:LogError(Object)
UnityEngine.Formats.Alembic.Importer.AlembicStream:AbcLoad(Boolean, Boolean) (at C:\AlembicSpawning\Library\PackageCache\com.unity.formats.alembic@1.0.5\Runtime\Scripts\Importer\AlembicStream.cs:149)
UnityEngine.Formats.Alembic.Importer.AlembicStreamPlayer:LoadStream(Boolean) (at C:\AlembicSpawning\Library\PackageCache\com.unity.formats.alembic@1.0.5\Runtime\Scripts\Importer\AlembicStreamPlayer.cs:75)
UnityEngine.Formats.Alembic.Importer.AlembicStreamPlayer:OnEnable() (at C:\AlembicSpawning\Library\PackageCache\com.unity.formats.alembic@1.0.5\Runtime\Scripts\Importer\AlembicStreamPlayer.cs:137)
UnityEngine.Object:Internal_CloneSingleWithParent(Object, Transform, Boolean)
UnityEngine.Object:Instantiate(Object, Transform, Boolean) (at C:\buildslave\unity\build\Runtime\Export\Scripting\UnityEngineObject.bindings.cs:255)
UnityEngine.Object:Instantiate(GameObject, Transform, Boolean) (at C:\buildslave\unity\build\Runtime\Export\Scripting\UnityEngineObject.bindings.cs:291)
UnityEngine.Object:Instantiate(GameObject, Transform) (at C:\buildslave\unity\build\Runtime\Export\Scripting\UnityEngineObject.bindings.cs:286)
SpawnPrefab:InstantiatePrefab() (at C:\AlembicSpawning\Assets\_Project\Scripts\SpawnPrefab.cs:19)
SpawnPrefab:Start() (at C:\AlembicSpawning\Assets\_Project\Scripts\SpawnPrefab.cs:12)
```

## Tech

* Unity 2019.1.10
* Alembic Package 1.0.5
* High Definition RP Package 5.7.2
* Windows 10 with IL2CPP