# Alembic Spawning Problem

Instantiating an alembic during runtime can't find the alembic in the build. Works in the Editor and an Alembic as a simple GameObject in the Scene works as well. Only alembics that are added afterwards to the scene won't be bundled in the Build.

## Reproduce

Build the project for Windows Standalone as a development build. Run the project.
### Expected Result
Two cubes are visible
### Actual Result
The Alembic cube that is set in the scene is visible, the cube that should be instantiated, is not visible since the file can't be found in the streaming assets 

## Tech

* Unity 2019.1.10
* High Definition Renderpipeline
* Windows 10 with IL2CPP