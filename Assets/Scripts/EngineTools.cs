using Godot;
using System;

class EngineTools
{
    public void DebugPrint(string message)
    {
        GD.Print("[DEBUG] " + message);
    }

    public void ErrorPrint(string message)
    {
        GD.Print("[ERROR] " + message);
    }
}

class CustomErrors
{
    class ChildDoesNotExistException : Exception
    {
        public ChildDoesNotExistException(string message) : base(message)
        {
            new EngineTools
        }
    }
}